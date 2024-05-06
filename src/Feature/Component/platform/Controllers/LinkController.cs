using Feature.Wealth.Component.Models.Link;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class LinkController : Controller
    {
        public ActionResult Link()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<LinkModel.Link>();

            foreach (var childItem in childItems)
            {
                items.Add(new LinkModel.Link(childItem));
            }

            var model = new LinkModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/InnerLink/Link.cshtml", model);
        }
    }
}
