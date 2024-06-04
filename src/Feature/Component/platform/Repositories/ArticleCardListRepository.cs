using Feature.Wealth.Component.Models.ArticleCardList;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class ArticleCardListRepository
    {
        private readonly VisitCountRepository _visitCountRepository = new VisitCountRepository();

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
                
                var targetItem = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).TargetItem;

                if (targetItem != null)
                {
                    string pageItemId = targetItem.ID.ToString();

                    cardItem.Link = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).Url + "?id=" + pageItemId;
                    cardItem.LinkTarget = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).Target;
                    cardItem.LinkTitle = ItemUtils.GeneralLink(child, Templates.ArticleCardItem.Fields.Link).Title;

                    var visitCount = _visitCountRepository.GetVisitCount(pageItemId.ToGuid(), cardItem.Link);
                    cardItem.ViewCount = visitCount?.ToString("N0") ?? "0";
                }
                else
                {
                    cardItem.Link = string.Empty;
                    cardItem.LinkTarget = string.Empty;
                    cardItem.LinkTitle = string.Empty;
                    cardItem.ViewCount = string.Empty;
                }
                
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