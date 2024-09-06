using Feature.Wealth.Component.Models.SearchBar;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class SearchBarController : Controller
    {
        private readonly SearchBarRepository _searchRepository = new SearchBarRepository();
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;
            return View("~/Views/Feature/Wealth/Component/SearchBar/SearchBar.cshtml", new SearchBarModel(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSearchResult() => new JsonNetResult(_searchRepository.GetResultList());
    }
}