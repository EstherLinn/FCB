using Feature.Wealth.Component.Models.ArticleCardList;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class ArticleCardListRepository
    {
        public ArticleCardListModel GetArticleCardList()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering?.Item;

            if (dataSource == null)
            {
                return null;
            }

            var cardList = new List<ArticleCardItem>();

            var children = ItemUtils.GetChildren(dataSource, Templates.ArticleCardItem.ID);

            foreach (var child in children)
            {
                var cardItem = new ArticleCardItem();

                var date = ItemUtils.GetLocalDateFieldValue(child, Templates.ArticleCardItem.Fields.Date) ?? DateTime.MinValue;

                cardItem.Item = child;
                cardItem.PCImage = ItemUtils.ImageUrl(child, Templates.ArticleCardItem.Fields.PCImage);
                cardItem.MobileImage = ItemUtils.ImageUrl(child, Templates.ArticleCardItem.Fields.MobileImage);
                cardItem.Title = ItemUtils.GetFieldValue(child, Templates.ArticleCardItem.Fields.Title);
                cardItem.Content = ItemUtils.GetFieldValue(child, Templates.ArticleCardItem.Fields.Content);
                cardItem.Date = date != DateTime.MinValue ? date.ToString("yyyy/MM/dd") : string.Empty;
                cardItem.Link = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).Url;
                var targetItem = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).TargetItem;
                cardItem.TargetItemID = targetItem != null ? targetItem.ID.ToString() : string.Empty;

                cardList.Add(cardItem);
            }

            return new ArticleCardListModel
            {
                DataSource = dataSource,
                MainTitle = ItemUtils.GetFieldValue(dataSource, Templates.ArticleCardListDatasource.Fields.MainTitle),
                CardList = cardList
            };
        }
    }
}