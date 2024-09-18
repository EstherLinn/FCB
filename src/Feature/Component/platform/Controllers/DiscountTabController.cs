using Feature.Wealth.Component.Models.DiscountTab;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountTabController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new DiscountTabModel(item);

            return View("/Views/Feature/Wealth/Component/DiscountTab/DiscountTab.cshtml", model);
        }
    }
}
