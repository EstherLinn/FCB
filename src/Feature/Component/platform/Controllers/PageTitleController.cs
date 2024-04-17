using Feature.Wealth.Component.Models.PageTitle;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class PageTitleController : Controller
    {
        public ActionResult Index()
        {
            var model = new PageTitleModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/PageTitle/PageTitle.cshtml", model);
        }
    }
}
