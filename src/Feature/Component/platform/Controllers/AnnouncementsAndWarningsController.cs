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
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var imageUrl = ItemUtils.ImageUrl(item, Templates.AnnouncementsandWarnings.Fields.Image);

            var model = new AnnouncementsAndWarningsModel(item);

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/AnnouncementsAndWarnings.cshtml", model);
        }

    }
}
