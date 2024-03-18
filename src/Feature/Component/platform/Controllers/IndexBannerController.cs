using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.IndexBanner;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
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
            var childItems = dataSourceItem.Children.ToList();

            var items = new List<IndexBannerModel.Banner>();

            foreach (var childItem in childItems)
            {
                var imageField = (ImageField)childItem.Fields[IndexBannerModel.Banner.Image];
                string imageUrl = imageField != null ? FieldUtils.ImageUrl(imageField) : string.Empty;

                var btnField = childItem.Fields[IndexBannerModel.Banner.ButtonLink];
                var btnLink = btnField != null ? FieldUtils.GeneralLink(btnField).Url : string.Empty;

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