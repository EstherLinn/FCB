using Feature.Wealth.Component.Models.SearchBar;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class SearchBarController : Controller
    {
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;
            return View("~/Views/Feature/Wealth/Component/SearchBar/SearchBar.cshtml", new SearchBarModel(model));
        }
    }
}
