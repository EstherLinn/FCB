using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;
using System.Runtime.Caching;
using Feature.Wealth.Component.Models.SiteProductSearch;

namespace Feature.Wealth.Component.Controllers
{
    public class SiteProductSearchController : Controller
    {
        private readonly SiteProductSearchRepository _searchRepository = new SiteProductSearchRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string SiteProductSearchCacheKey = $"Fcb_SiteProductSearchCache";
        private readonly string cacheTime = Settings.GetSetting("SiteProductSearchCacheTime");
        public ActionResult Index()
        {
            var datasource = ItemUtils.GetItem(RenderingContext.Current.Rendering.DataSource);
            var model = _searchRepository.GetSiteProductSearchViewModel(datasource);
            return View("/Views/Feature/Wealth/Component/SiteProductSearch/SiteProductSearchIndex.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetSearchResultData()
        {
            RespProduct resp;
            resp = (RespProduct)_cache.Get(SiteProductSearchCacheKey);

            if (resp == null) {
                resp = _searchRepository.GetResultList();
                _cache.Set(SiteProductSearchCacheKey, resp, _commonRespository.GetCacheExpireTime(cacheTime));
            }
            
            return new JsonNetResult(resp);
        }
    }
}
