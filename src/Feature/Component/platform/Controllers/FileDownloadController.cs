using Feature.Wealth.Component.Models.FileDownload;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FileDownloadController : Controller
    {
        public ActionResult FileDownload()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var childItems = ItemUtils.GetChildren(dataSourceItem).ToList();

            var items = new List<FileDownloadModel.File>();

            foreach (var childItem in childItems)
            {
                var link = ItemUtils.GeneralLink(childItem, Templates.FileDownload.Fields.Link);

                items.Add(new FileDownloadModel.File(childItem)
                {
                    LinkUrl = link.Url,
                    LinkTarget = link.Target
                });
            }

            var model = new FileDownloadModel
            {
                DataSource = dataSourceItem,
                Items = items
            };
            return View("/Views/Feature/Wealth/Component/FileDownload/FileDownload.cshtml", model);
        }
    }
}
