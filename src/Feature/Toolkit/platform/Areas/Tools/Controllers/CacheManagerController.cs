using Newtonsoft.Json;
using System.Web.Mvc;
using Feature.Wealth.Toolkit.Areas.Tools.Models.CacheManager;
using Feature.Wealth.Toolkit.Areas.Tools.Services;
using Feature.Wealth.Toolkit.Attributes;
using Xcms.Sitecore.Foundation.Basic.Attributes;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Toolkit.Areas.Tools.Controllers
{
    [ToolkitRole]
    public class CacheManagerController : Controller
    {
        private readonly CacheDataHandler _httpRunTimeCache;
        private readonly CacheDataHandler _memoryCache;
        private readonly SitecoreCacheHandler _sitecoreCache;
        public CacheManagerController()
        {
            var service = new CacheManagerService();
            this._httpRunTimeCache = service.GetHttpRunTimeCache();
            this._memoryCache = service.GetMemoryCache();
            this._sitecoreCache = service.GetSitecoreCache();

            this.ViewBag.Title = "Cache Manager";
        }

        /// <summary>
        /// Cache Manager 主畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new CacheManagerViewModal
            {
                SitecoreCaches = this._sitecoreCache.GetCaches(),
                RuntimeCaches = this._httpRunTimeCache.GetCaches(),
                MemoryCaches = this._memoryCache.GetCaches()
            };

            return View("~/Areas/Tools/Views/CacheManager/index.cshtml", model);
        }

        private new JsonNetResult Json(object data) => new(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        /// <summary>
        /// Asp.net 2.0 Cache 處理方法
        /// </summary>
        /// <param name="action">Method {string}</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        [HttpPost]
        [SkipAnalyticsTracking]
        public JsonResult AspNetMethod(string action, string key)
        {
            var result = new Return();
            string method = action.ToUpper();
            (bool Success, string Message) status;
            switch (method)
            {
                case "CONTENT":
                    result.Message = this._httpRunTimeCache.GetCache(key);
                    break;
                case "DELETE":
                    status = this._httpRunTimeCache.RemoveCache(key);
                    result.Success = status.Success;
                    result.Message = status.Message;
                    break;
                case "CLEAR":
                    status = this._httpRunTimeCache.RemoveCaches();
                    result.Success = status.Success;
                    result.Message = status.Message;
                    break;
            }

            result.Caches = this._httpRunTimeCache.GetCaches();
            return Json(result);
        }

        /// <summary>
        /// Asp.net 4.0 Cache 處理方法
        /// </summary>
        /// <param name="action">mehtod(string)</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        [HttpPost]
        [SkipAnalyticsTracking]
        public JsonResult MemoryMethod(string action, string key)
        {
            var result = new Return();
            string method = action.ToUpper();
            (bool success, string message) status;
            switch (method)
            {
                case "CONTENT":
                    result.Message = this._memoryCache.GetCache(key);
                    break;
                case "DELETE":
                    status = this._memoryCache.RemoveCache(key);
                    result.Success = status.success;
                    result.Message = status.message;
                    break;
                case "CLEAR":
                    status = this._memoryCache.RemoveCaches();
                    result.Success = status.success;
                    result.Message = status.message;
                    break;
            }

            result.Caches = this._memoryCache.GetCaches();
            return Json(result);
        }

        /// <summary>
        /// Sitecore Cache 處理方法
        /// </summary>
        /// <param name="action">mehtod(string)</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        [HttpPost]
        [SkipAnalyticsTracking]
        public JsonResult SitecoreMethod(string action, string key)
        {
            var result = new Return();

            string method = action.ToUpper();

            switch (method)
            {
                case "DELETE":
                    result.Caches = this._sitecoreCache.RemoveCache(key);
                    break;
                case "CLEAR":
                    result.Caches = this._sitecoreCache.RemoveCaches();
                    break;
                case "RELOAD":
                    result.Caches = this._sitecoreCache.GetCaches();
                    break;
            }

            return Json(result);
        }
    }
}