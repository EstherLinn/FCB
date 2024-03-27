using Feature.Wealth.Component.Models.Tab;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class TabController : Controller
    {
        public ActionResult Index()
        {
            var datasource = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/Tab/Tab.cshtml", CreateModel(datasource));
        }

        protected TabModel CreateModel(Item item)
        {
            var model = new TabModel(item);
            return model;
        }
    }
}