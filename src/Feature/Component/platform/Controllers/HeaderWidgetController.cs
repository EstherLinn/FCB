using Feature.Wealth.Account.Pipelines.DI;
using Feature.Wealth.Component.Models.HeaderWidget;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System.Web;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class HeaderWidgetController : Controller
    {

        public ActionResult Index()
        {
            var service = ServiceLocator.ServiceProvider.GetService<IMemberLoginService>();
            if (service.IsAppLogin && !service.AppLoginSuccess)
            {
                ViewData["LoginStatus"] = false;
            }
            return View("/Views/Feature/Wealth/Component/HeaderWidget/HeaderWidget.cshtml", CreateModel());
        }

        protected HeaderWidgetModel CreateModel()
        {
            var model = new HeaderWidgetModel();
            return model;
        }
    }
}