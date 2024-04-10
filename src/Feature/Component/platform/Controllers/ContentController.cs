using Feature.Wealth.Component.Models.Notice;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Index()
        {
            var model = new NoticeModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/Content/Content.cshtml", model);
        }
    }
}
