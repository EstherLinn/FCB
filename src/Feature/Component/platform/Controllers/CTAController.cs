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

            var imageField = (ImageField)item.Fields[Template.Image];
            string imageUrl = imageField != null ? FieldUtils.ImageUrl(imageField) : string.Empty;

            var btnField = item.Fields[Template.ButtonLink];
            var btnLink = btnField != null ? FieldUtils.GeneralLink(btnField).Url : string.Empty;

            var showIcon = FieldUtils.IsChecked(item.Fields[Template.ShowIcon]);

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