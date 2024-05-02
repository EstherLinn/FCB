using Feature.Wealth.Component.Models.StructuredProduct;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class StructuredProductController : Controller
    {
        private readonly StructuredProductSearchRepository _searchRepository = new();
        private readonly StructuredProductRepository _structuredProductRepository = new();

        /// <summary>
        /// 結構型商品搜尋
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var datasource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var model = this._searchRepository.GetStructuredProductSearch(datasource);
            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductSearch.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetStructuredProducts(string datasourceIdStr)
        {
            var datasource = ItemUtils.GetItem(datasourceIdStr);

            if (datasource != null)
            {
                return new JsonNetResult(this._structuredProductRepository.GetStructuredProducts(datasource));
            }

            return new JsonNetResult(new List<StructuredProductModel>());
        }

        /// <summary>
        /// 結構型商品
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            string productCode = WebUtil.GetSafeQueryString("id");

            var datasource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var model = this._structuredProductRepository.GetStructuredProduct(datasource, productCode);
            // querystring id查無此商品
            if (model?.StructuredProduct == null)
            {
                return new EmptyResult();
            }

            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductDetail.cshtml", model);
        }
    }
}