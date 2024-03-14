using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.IndexBanner;
using Sitecore.Mvc.Presentation;

namespace Feature.Wealth.Component.Controllers
{
    /// <summary>
    /// B01-首頁Banner元件
    /// </summary>
    public class IndexBannerController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.Current.Rendering.Item;
            var childItems = dataSourceItem.Children.ToList();

            var model = new IndexBannerModel
            {
                DataSource = dataSourceItem,
                Items = childItems.Select(item => new IndexBannerModel.Banner(item)).ToList()
            };

            return View("/Views/Feature/Wealth/Component/IndexBanner/IndexBanner.cshtml", model);
        }
    }
}