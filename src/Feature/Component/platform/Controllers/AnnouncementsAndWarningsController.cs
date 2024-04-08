using Feature.Wealth.Component.Models.AnnouncementsAndWarnings;
using Feature.Wealth.Component.Models.Notice;
using Feature.Wealth.Component.Models.Tab;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.AnnouncementsAndWarnings.Templates;

namespace Feature.Wealth.Component.Controllers
{
    public class AnnouncementsAndWarningsController : Controller
    {
        public ActionResult Index()
        {
            var datasource = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/AnnouncementsAndWarnings.cshtml", CreateModel(datasource));
        }

        protected TabModel CreateModel(Item item)
        {
            var model = new TabModel(item);
            return model;
        }

        public ActionResult Announcements()
        {
            var model = new AnnouncementsAndWarningsModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/Announcements.cshtml", model);
        }

        public ActionResult MoreAnnouncements()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl = ItemUtils.ImageUrl(item, Templates.MoreAnnouncement.Fields.Image);
            var image3xUrl = ItemUtils.ImageUrl(item, Templates.MoreAnnouncement.Fields.Image3x);

            var model = new AnnouncementsAndWarningsModel()
            {
                Item = item,
                ImageUrl = imageUrl,
                Image3xUrl = image3xUrl,
            };

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/MoreAnnouncements.cshtml", model);
        }

        public ActionResult Warnings()
        {
            var model = new NoticeModel()
            {
                Item = RenderingContext.CurrentOrNull?.Rendering.Item
            };

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/Warnings.cshtml", model);
        }
    }
}
