using Feature.Wealth.Component.Models.Banner;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class BannerController : Controller
    {
        public ActionResult InnerBanner()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl = ItemUtils.ImageUrl(item, Templates.InnerBanner.Fields.Image);

            var model = new InnerBannerModel()
            {
                Item = item,
                ImageUrl = imageUrl,
            };

            return View("/Views/Feature/Wealth/Component/Banner/InnerBanner.cshtml", model);
        }
    }
}
