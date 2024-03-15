using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.CTA;
using Sitecore.Mvc.Presentation;

namespace Feature.Wealth.Component.Controllers
{
    public class CTAController : Controller
    {
        public ActionResult Index()
        {
            var model = new CTAModel()
            {
                Item = RenderingContext.Current?.Rendering.Item
            };
            return View("/Views/Feature/Wealth/Component/CTA/CTA.cshtml", model);
        }
    }
}