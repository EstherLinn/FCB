using Feature.Wealth.Component.Models.StructuredProduct;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class StructuredProductController : Controller
    {
        private readonly StructuredProductRepository _structuredProductRepository = new StructuredProductRepository();

        public ActionResult Index()
        {
            var model = new StructuredProductSearchViewModel();

            // datasource(後台資料源)
            var datasourceId = RenderingContext.CurrentOrNull?.Rendering.Item.ID ?? null;

            model.DatasourceId = datasourceId?.ToString();
            model.KeywordOptions = _structuredProductRepository.GetTagOptionsWithProducts(datasourceId, _StructProductTagDatasource.Fields.KeywordDatasource);
            model.TopicOptions = _structuredProductRepository.GetTagOptionsWithProducts(datasourceId, _StructProductTagDatasource.Fields.TopicDatasource);
            model.DetailPageItemUrl = _structuredProductRepository.GetDetailPageItemUrl(datasourceId);

            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductSearch.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetAllStructuredProducts(string datasourceIdStr)
        {
            var datasourceId = datasourceIdStr?.ToId();
            var res = new List<StructuredProductModel>();

            if (!datasourceId.IsNullOrEmpty())
            {
                res = _structuredProductRepository.GetAllStructuredProducts(datasourceId)?.ToList();
            }

            return new JsonNetResult(res);
        }

        public ActionResult Detail()
        {
            // product code(querystring)
            Uri createdUri = null;
            Uri.TryCreate(Request.Url.AbsoluteUri, UriKind.Absolute, out createdUri);

            string querystring = createdUri.Query;
            if (!string.IsNullOrEmpty(querystring) && querystring.Contains("?"))
            {
                querystring = querystring.Replace("?", string.Empty);
            }
            var query = HttpUtility.ParseQueryString(querystring);
            var productCode = query.Get("id");

            // querystring無輸入id 
            if (productCode == null)
            {
                return new EmptyResult();
            }


            // datasource(後台資料源)
            var datasourceId = RenderingContext.CurrentOrNull?.Rendering.Item?.ID ?? null;

            // 建立StructuredProductDetailViewModel
            var viewmodel = new StructuredProductDetailViewModel();

            // 取得商品基本資訊
            viewmodel.StructuredProduct = _structuredProductRepository.GetStructuredProduct(productCode, datasourceId);


            // querystring id查無此商品
            if (viewmodel.StructuredProduct == null)
            {
                return new EmptyResult();
            }

            // 確認有此商品 取得其他資訊
            viewmodel.HistoryBankSellPrice = _structuredProductRepository.GetHistoryBankSellPrice(productCode);
            viewmodel.ThirtyDayBankSellPriceWithChange = _structuredProductRepository.GetThirtyDayBankSellPriceWithChange(productCode);
            viewmodel.HistoryDividend = _structuredProductRepository.GetHistoryDividend(productCode);

            return View("/Views/Feature/Wealth/Component/StructuredProduct/StructuredProductDetail.cshtml", viewmodel);
        }
    }
}