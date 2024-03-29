using Feature.Wealth.Component.Models.Notice;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class NoticeController : Controller
    {
        public ActionResult Notice()
        {
            var model = new NoticeModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/Notice/Notice.cshtml", model);
        }

        public ActionResult BigNotice()
        {
            var model = new NoticeModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/Notice/BigNotice.cshtml", model);
        }
    }
}
