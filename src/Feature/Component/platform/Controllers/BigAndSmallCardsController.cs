using Feature.Wealth.Component.Models.BigAndSmallCards;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class BigAndSmallCardsController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new BigAndSmallCardsModel(item);

            return View("/Views/Feature/Wealth/Component/BigAndSmallCards/BigAndSmallCards.cshtml", model);
        }
    }
}
