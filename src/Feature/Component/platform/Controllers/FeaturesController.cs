using Feature.Wealth.Component.Models.Features;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FeaturesController : Controller
    {
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<FeaturesModel.Features>();

            foreach (var childItem in childItems)
            {

                items.Add(new FeaturesModel.Features(childItem));
            }

            var model = new FeaturesModel
            {
                DataSource = dataSourceItem,
                Items = items
            };

            return View("/Views/Feature/Wealth/Component/Features/Features.cshtml", model);
        }
    }
}
