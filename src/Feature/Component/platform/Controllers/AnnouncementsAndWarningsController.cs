using Feature.Wealth.Component.Models.AnnouncementsAndWarnings;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class AnnouncementsAndWarningsController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new AnnouncementsAndWarningsModel(item);

            return View("/Views/Feature/Wealth/Component/AnnouncementsAndWarnings/AnnouncementsAndWarnings.cshtml", model);
        }

    }
}
