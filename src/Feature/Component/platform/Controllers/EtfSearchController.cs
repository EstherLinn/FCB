using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class EtfSearchController : Controller
    {
        private readonly EtfSearchRepository _searchRepository = new EtfSearchRepository();
        public ActionResult Index()
        {
            var model = _searchRepository.GetETFSearchModel();
            return View("/Views/Feature/Wealth/Component/ETFSearch/EtfSearchIndex.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetSearchResultData()
        {
            var resp = _searchRepository.GetResultList();
            return new JsonNetResult(resp);
        }
    }
}
