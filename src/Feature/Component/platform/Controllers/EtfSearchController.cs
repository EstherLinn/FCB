using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class EtfSearchController : Controller
    {
        private readonly EtfSearchRepository _searchRepository = new EtfSearchRepository();
        public ActionResult Index()
        {
            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _searchRepository.GetETFSearchModel(datasource);
            return View("/Views/Feature/Wealth/Component/ETFSearch/EtfSearchIndex.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetSearchResultData(ReqSearch req)
        {
            var resp = _searchRepository.GetResultList(req);
            return new JsonNetResult(resp);
        }
    }
}
