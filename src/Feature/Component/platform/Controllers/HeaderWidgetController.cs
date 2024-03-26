using Feature.Wealth.Component.Models.HeaderWidget;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class HeaderWidgetController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/HeaderWidget/HeaderWidget.cshtml", CreateModel());
        }

        protected HeaderWidgetModel CreateModel()
        {
            var model = new HeaderWidgetModel();
            return model;
        }
    }
}