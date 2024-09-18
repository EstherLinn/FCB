using Feature.Wealth.Component.Models.GraphicCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class GraphicCardsController : Controller
    {
        public ActionResult Graphic3Cards()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new GraphicCardsModel(item);

            return View("/Views/Feature/Wealth/Component/GraphicCards/Graphic3Cards.cshtml", model);
        }
    }
}
