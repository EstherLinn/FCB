using Feature.Wealth.Component.Models.HorizontalGraphics;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class HorizontalGraphicsController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<HorizontalGraphicsModel.HorizontalGraphics>();

            foreach (var childItem in childItems)
            {
                var image = ItemUtils.ImageUrl(childItem, Templates.HorizontalGraphics.Fields.Image);

                items.Add(new HorizontalGraphicsModel.HorizontalGraphics(childItem)
                {
                    ImageUrl = image,
                });
            }

            var model = new HorizontalGraphicsModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/HorizontalGraphics/HorizontalGraphics.cshtml", model);
        }
    }
}
