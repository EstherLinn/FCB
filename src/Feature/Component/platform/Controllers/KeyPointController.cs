using Feature.Wealth.Component.Models.KeyPoint;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class KeyPointController : Controller
    {
        public ActionResult KeyPoint()
        {
            var model = new KeyPointModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item

            };
            return View("/Views/Feature/Wealth/Component/KeyPoint/KeyPoint.cshtml", model);
        }
    }
}
