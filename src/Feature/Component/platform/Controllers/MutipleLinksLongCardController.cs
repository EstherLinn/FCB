using Feature.Wealth.Component.Models.MutipleLinksLongCard;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class MutipleLinksLongCardController : Controller
    {
        public ActionResult Index()
        {
            var model = new MutipleLinksLongCardModel(RenderingContext.CurrentOrNull?.Rendering.Item);
            return View("/Views/Feature/Wealth/Component/MutipleLinksLongCard/MutipleLinksLongCard.cshtml", model);
        }
    }
}
