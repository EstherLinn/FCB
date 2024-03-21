using Feature.Wealth.Component.Models.Banner;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class BannerController : Controller
    {
        public ActionResult InnerBanner()
        {
            var model = new InnerBannerModel()
            {
                Item = RenderingContext.Current?.Rendering.Item
            };
            return View("/Views/Feature/Component/Banner/InnerBanner.cshtml", model);
        }
    }

}
