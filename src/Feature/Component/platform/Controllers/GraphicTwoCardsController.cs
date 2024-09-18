using Feature.Wealth.Component.Models.GraphicTwoCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class GraphicTwoCardsController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new GraphicTwoCardsModel(item);

            return View("/Views/Feature/Wealth/Component/GraphicTwoCards/GraphicTwoCards.cshtml", model);
        }
    }
}
