using Feature.Wealth.Component.Models.FeaturedCards;
using Sitecore.Mvc.Presentation;
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
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<FeaturedCardsModel.FeaturedCards>();
            foreach (var childItem in childItems)
            {
                items.Add(new FeaturedCardsModel.FeaturedCards(childItem));
            }

            var model = new FeaturedCardsModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/FeaturedCards/FeaturedCards.cshtml", model);
        }
    }
}
