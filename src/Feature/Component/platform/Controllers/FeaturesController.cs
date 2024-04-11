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
                var imageUrl = ItemUtils.ImageUrl(childItem, Templates.Features.Fields.Image);
                var btnlink = ItemUtils.GeneralLink(childItem, Templates.Features.Fields.ButtonLink)?.Url;
                var linkUrl1 = ItemUtils.GeneralLink(childItem, Templates.Features.Fields.Link1)?.Url;
                var linkUrl2 = ItemUtils.GeneralLink(childItem, Templates.Features.Fields.Link2)?.Url;

                items.Add(new FeaturesModel.Features(childItem)
                {
                    ImageUrl = imageUrl,
                    ButtonLink = btnlink,
                    LinkUrl1 = linkUrl1,
                    LinkUrl2 = linkUrl2,
                });
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
