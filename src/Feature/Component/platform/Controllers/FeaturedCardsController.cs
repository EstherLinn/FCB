using Feature.Wealth.Component.Models.FeaturedCards;
using FluentFTP;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FeaturedCardsController : Controller
    {
        public ActionResult FeaturedCards()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem);

            var items = new List<FeaturedCards>();
            foreach (var childItem in childItems ?? Enumerable.Empty<Item>())
            {
                var content = ItemUtils.GetFieldValue(childItem, Templates.FeaturedCards.Fields.Content);

                items.Add(new FeaturedCards(childItem)
                {
                    Content = content
                });
            }

            var model = new FeaturedCardsModel(dataSourceItem, items);


            return View("/Views/Feature/Wealth/Component/FeaturedCards/FeaturedCards.cshtml", model);
        }
    }
}
