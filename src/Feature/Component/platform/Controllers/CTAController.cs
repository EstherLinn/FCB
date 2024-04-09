using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Models.CTA;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class CTAController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string imagePcUrl = ItemUtils.ImageUrl(item,Template.CTA.Fields.ImagePc);
            string imageMbUrl = ItemUtils.ImageUrl(item,Template.CTA.Fields.ImageMb);
            string btnLink = ItemUtils.GeneralLink(item, Template.CTA.Fields.ButtonLink)?.Url;
            bool showIcon = ItemUtils.IsChecked(item, Template.CTA.Fields.ShowIcon);
            string btnText = ItemUtils.GetFieldValue(item, Template.CTA.Fields.ButtonText);

            var model = new CTAModel()
            {
                Item = item,
                ImagePcUrl = imagePcUrl,
                ImageMbUrl = imageMbUrl,
                ButtonLink = btnLink,
                ButtonText = btnText,
                ShowIcon = showIcon
            };

            return View("/Views/Feature/Wealth/Component/CTA/CTA.cshtml", model);
        }
    }
}