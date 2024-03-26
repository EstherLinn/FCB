using Feature.Wealth.Component.Models.SidebarWidget;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class SidebarWidgetController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/SidebarWidget/SidebarWidget.cshtml", CreateModel());
        }

        protected SidebarWidgetModel CreateModel()
        {
            var model = new SidebarWidgetModel();
            return model;
        }
    }
}