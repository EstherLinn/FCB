using Feature.Wealth.Component.Models.CTA;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class CTAController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new CTAModel(item);

            return View("/Views/Feature/Wealth/Component/CTA/CTA.cshtml", model);
        }
    }
}