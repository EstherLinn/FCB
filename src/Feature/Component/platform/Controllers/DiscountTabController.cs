using Feature.Wealth.Component.Models.DiscountTab;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountTabController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;

            IEnumerable<DiscountTabItem> tabList = dataSource.GetChildren(Templates.DiscountTabItem.ID)
            .Where(children => dataSource.GetChildren(Templates.DiscountTabItem.ID).Any())
            .Select(x => new DiscountTabItem { Item = x });

            var model = new DiscountTabModel()
            {
                DataSource = dataSource,
                TabList = tabList
            };

            return View("/Views/Feature/Wealth/Component/DiscountTab/DiscountTab.cshtml", model);
        }
    }
}
