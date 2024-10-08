using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Controllers
{
    public class EtfSearchController : Controller
    {
        private readonly EtfSearchRepository _searchRepository = new EtfSearchRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string EtfSearchCacheKey = $"Fcb_EtfSearchCache";
        private readonly string cacheTime = Settings.GetSetting("EtfSearchCacheTime");
        public ActionResult Index()
        {
            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _searchRepository.GetETFSearchModel(datasource);
            return View("/Views/Feature/Wealth/Component/ETFSearch/EtfSearchIndex.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetSearchResultData()
        {
            IEnumerable<EtfSearchResult> resp;
            resp = (IEnumerable<EtfSearchResult>)_cache.Get(EtfSearchCacheKey);

            if (resp == null) {
                resp = _searchRepository.GetResultList();
                _cache.Set(EtfSearchCacheKey, resp, _commonRespository.GetCacheExpireTime(cacheTime));
            }
            return new JsonNetResult(resp);
        }
    }
}
