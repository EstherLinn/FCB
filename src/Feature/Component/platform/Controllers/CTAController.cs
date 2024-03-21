using System.Web.Mvc;
using Feature.Wealth.Component.Models.CTA;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class CTAController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl = ItemUtils.ImageUrl(item,Template.Image);
            var btnLink = ItemUtils.GeneralLink(item, Template.ButtonLink).Url;
            var showIcon = ItemUtils.IsChecked(item, Template.ShowIcon);

            var model = new CTAModel()
            {
                Item = item,
                ImageUrl = imageUrl,
                ButtonLink = btnLink,
                ShowIcon = showIcon
            };

            return View("/Views/Feature/Wealth/Component/CTA/CTA.cshtml", model);
        }
    }
}