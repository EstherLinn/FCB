using Feature.Wealth.Component.Models.DiscountContent;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountContentController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var pcBannerSrc = ItemUtils.ImageUrl(dataSource, Templates.DiscountContentDatasource.Fields.PCBanner);
            var mobileBannerSrc = ItemUtils.ImageUrl(dataSource, Templates.DiscountContentDatasource.Fields.MobileBanner);
            var displayDate = DiscountContentRepository.GetDisplayDate(dataSource);
            var displayTag = DiscountContentRepository.GetDisplayTag(dataSource);
            var buttonLink = ItemUtils.GeneralLink(dataSource, Templates.DiscountContentDatasource.Fields.ButtonLink).Url;

            var model = new DiscountContentModel()
            {
                DataSource = dataSource,
                PCBannerSrc = pcBannerSrc,
                MobileBannerSrc = mobileBannerSrc,
                DisplayDate = displayDate,
                DisplayTag = displayTag,
                ButtonLink = buttonLink
            };

            return View("/Views/Feature/Wealth/Component/DiscountContent/DiscountContent.cshtml", model);
        }
    }
}