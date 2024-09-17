using Feature.Wealth.Component.Models.DiscountContent;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountContentController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new DiscountContentModel(item);

            return View("/Views/Feature/Wealth/Component/DiscountContent/DiscountContent.cshtml", model);
        }
    }
}