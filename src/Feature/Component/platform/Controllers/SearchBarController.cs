using Feature.Wealth.Component.Models.SearchBar;
using Feature.Wealth.Component.Models.SiteProductSearch;
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
        [OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult GetSearchResult() => new JsonNetResult(_searchRepository.GetResultList());

        [HttpPost]
        [OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult GetFundResult() => new JsonNetResult(_searchRepository.MapperFundResult());

        [HttpPost]
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetETFResult() => new JsonNetResult(_searchRepository.MapperETFResult());

        [HttpPost]
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetForeignStockResult() => new JsonNetResult(_searchRepository.MapperForeignStockResult());

        [HttpPost]
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetStructuredProductResult() => new JsonNetResult(_searchRepository.MapperStructuredProductResult());
    }
}
