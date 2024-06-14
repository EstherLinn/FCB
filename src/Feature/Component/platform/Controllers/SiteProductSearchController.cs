using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class SiteProductSearchController : Controller
    {
        private readonly SiteProductSearchRepository _searchRepository = new SiteProductSearchRepository();
        public ActionResult Index()
        {
            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _searchRepository.GetSiteProductSearchViewModel(datasource);
            return View("/Views/Feature/Wealth/Component/SiteProductSearch/SiteProductSearchIndex.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetSearchResultData()
        {
            var resp = _searchRepository.GetResultList();
            return new JsonNetResult(resp);
        }
    }
}
