using Feature.Wealth.Component.Models.GraphicCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class GraphicCardsController : Controller
    {
        public ActionResult Graphic3Cards()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl1 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image1);
            var imageUrl2 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image2);
            var imageUrl3 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.Image3);
            var buttonLink1 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.ButtonLink1);
            var buttonLink2 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.ButtonLink2);
            var buttonLink3 = ItemUtils.ImageUrl(item, Templates.GraphicCards.Fields.ButtonLink3);

            var model = new GraphicCardsModel()
            {
                Item = item,
                ImageUrl1 = imageUrl1,
                ImageUrl2 = imageUrl2,
                ImageUrl3 = imageUrl3,
                ButtonLink1 = buttonLink1,
                ButtonLink2 = buttonLink2,
                ButtonLink3 = buttonLink3,
            };

            return View("/Views/Feature/Wealth/Component/GraphicCards/Graphic3Cards.cshtml", model);
        }
    }
}
