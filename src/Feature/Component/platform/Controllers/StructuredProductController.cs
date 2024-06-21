using Feature.Wealth.Component.Models.StructuredProduct;
using Feature.Wealth.Component.Repositories;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class StructuredProductController : Controller
    {
        private readonly StructuredProductSearchRepository _searchRepository = new();
        private readonly StructuredProductRepository _structuredProductRepository = new();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string StructuredProductSearchCacheKey = $"Fcb_StructuredProductSearchCache";
        private readonly string cacheTime = Settings.GetSetting("StructuredProductCacheTime");

        /// <summary>
        /// 結構型商品搜尋
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StructuredProductSearchViewModel model;
            var datasource = RenderingContext.CurrentOrNull?.Rendering.Item;

            if (datasource != null && datasource.TemplateID.ToString() == StructProductListDatasource.Id.ToString())
            {
                model = this._searchRepository.GetStructuredProductSearch(datasource);
            }
            else
            {
                model = null;
            }

            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductSearch.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStructuredProducts()
        {
            List<StructuredProductModel> datas;

            datas = (List<StructuredProductModel>)_cache.Get(StructuredProductSearchCacheKey);

            if (datas == null)
            {
                datas = (List<StructuredProductModel>)this._structuredProductRepository.GetStructuredProducts();

                _cache.Set(StructuredProductSearchCacheKey, datas, _commonRespository.GetCacheExpireTime(cacheTime));
            }

            return new JsonNetResult(datas);
        }

        /// <summary>
        /// 結構型商品詳細頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            string productCode = WebUtil.GetSafeQueryString("id");

            var model = this._structuredProductRepository.GetStructuredProductDetail(productCode);
            // querystring id查無此商品
            if (model?.StructuredProduct == null)
            {
                return new EmptyResult();
            }

            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductDetail.cshtml", model);
        }
    }
}