using Feature.Wealth.Component.Models.CommonTools;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class CommonToolsController : Controller
    {
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;
            return View("~/Views/Feature/Wealth/Component/CommonTools/CommonTools.cshtml", new CommonToolsModel(model));
        }
    }
}
