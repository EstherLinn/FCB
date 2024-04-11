using Feature.Wealth.Component.Models.LongCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class LongCardsController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl = ItemUtils.ImageUrl(item, Templates.LongCards.Fields.Image);
            var buttonLink = ItemUtils.GeneralLink(item, Templates.LongCards.Fields.ButtonLink)?.Url;

            var model = new LongCardsModel()
            {
                Item = item,
                ImageUrl = imageUrl,
                ButtonLink = buttonLink,
            };

            return View("/Views/Feature/Wealth/Component/LongCards/LongCards.cshtml", model);
        }
    }
}
