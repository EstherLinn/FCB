using Feature.Wealth.Component.Models.RelatedArticles;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class RelatedArticleRepository
    {
        public RelatedArticlesModel GetRelatedArticleInfo()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering?.Item;

            if (item == null)
            {
                return null;
            }

            IEnumerable<Item> settingListField = ItemUtils.GetMultiListValueItems(item, Templates.RelatedArticleDatasource.Fields.SettingList)?.Take(4);

            if (settingListField == null)
            {
                return null;
            }

            var relatedArticlesInfo = new RelatedArticlesModel
            {
                DataSource = item,
                MainTitle = ItemUtils.GetFieldValue(item, Templates.RelatedArticleDatasource.Fields.MainTitle),
                CardList = new List<RelatedArticleCardItemModel>()
            };

            foreach (var settingItem in settingListField)
            {
                if (settingItem.TemplateID == Models.ArticleCardList.Templates.ArticleCardItem.ID)
                {
                    var cardItem = new RelatedArticleCardItemModel();

                    var date = ItemUtils.GetLocalDateFieldValue(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.Date) ?? DateTime.MinValue;

                    cardItem.Item = settingItem;
                    cardItem.PCImage = ItemUtils.ImageUrl(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.PCImage);
                    cardItem.MobileImage = ItemUtils.ImageUrl(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.MobileImage);
                    cardItem.Title = ItemUtils.GetFieldValue(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.Title);
                    cardItem.Date = date != DateTime.MinValue ? date.ToString("yyyy/MM/dd") : string.Empty;
                    cardItem.Link = ItemUtils.GeneralLink(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.Link).Url;
                    var targetItem = ItemUtils.GeneralLink(settingItem, Models.ArticleCardList.Templates.ArticleCardItem.Fields.Link).TargetItem;
                    cardItem.TargetItemID = targetItem != null ? targetItem.ID.ToString() : string.Empty;

                    ((List<RelatedArticleCardItemModel>)relatedArticlesInfo.CardList).Add(cardItem);
                }
            }

            return relatedArticlesInfo;
        }
    }
}
