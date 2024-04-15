using System.Web.Mvc;
using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Models.IndexBanner;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.IndexBanner.IndexBannerModel;

namespace Feature.Wealth.Component.Controllers
{
    /// <summary>
    /// B01-首頁Banner元件
    /// </summary>
    public class IndexBannerController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem);

            var items = new List<Banner>();

            foreach (var childItem in childItems)
            {
                string imageUrlPC = ItemUtils.ImageUrl(childItem, Template.IndexBanner.Fields.ImagePC);
                string imageUrlMB = ItemUtils.ImageUrl(childItem, Template.IndexBanner.Fields.ImageMB);
                string btnLink = ItemUtils.GeneralLink(childItem, Template.IndexBanner.Fields.ButtonLink)?.Url;
                bool checkedLightMode = ItemUtils.IsChecked(childItem, Template.IndexBanner.Fields.LightMode);
                string btnText = ItemUtils.GetFieldValue(childItem, Template.IndexBanner.Fields.ButtonText);

                items.Add(new Banner(childItem)
                {
                    IsLight = checkedLightMode,
                    ImageUrlPC = imageUrlPC,
                    ImageUrlMB = imageUrlMB,
                    ButtonLink = btnLink,
                    ButtonText = btnText
                });
            }

            var model = new IndexBannerModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/IndexBanner/IndexBanner.cshtml", model);
        }
    }
}