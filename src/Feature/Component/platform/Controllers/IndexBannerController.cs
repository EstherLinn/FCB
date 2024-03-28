using System.Web.Mvc;
using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Models.IndexBanner;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

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

            var items = new List<IndexBannerModel.Banner>();

            foreach (var childItem in childItems)
            {
                string imageUrl = ItemUtils.ImageUrl(childItem, IndexBannerModel.Banner.Template.Fields.Image);
                string btnLink = ItemUtils.GeneralLink(childItem, IndexBannerModel.Banner.Template.Fields.ButtonLink)?.Url;

                items.Add(new IndexBannerModel.Banner(childItem)
                {
                    imageUrl = imageUrl,
                    btnLink = btnLink
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