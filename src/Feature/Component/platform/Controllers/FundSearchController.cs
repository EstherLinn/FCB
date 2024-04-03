using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundSearch;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;


namespace Feature.Wealth.Component.Controllers
{
    public class FundSearchController : Controller
    {
        private FundSearchRepository _repository = new FundSearchRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var hotkeywordtags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.hotKeywordtags).ToList();
            var hotproducttags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.hotProductags).ToList();

            var items = _repository.GetFundSearchData();
            var searchbar = new SearchBarData
            {
                FundCompanies = items.Select(f => f.FundCompanyName).Distinct().ToList(),
                InvestmentRegions = items.OrderBy(t=>t.InvestmentRegionID).Select(f => f.InvestmentRegionName).Distinct().ToList(),
                InvestmentTargets = items.OrderBy(t=>t.InvestmentTargetID).Select(f => f.InvestmentTargetName).Distinct().ToList(),
                FundTypeNames = items.Select(f => f.FundTypeName).Distinct().ToList()
            };

            var ge=GetAllFunds();

            var viewModel = new FundSearchViewModel
            {
                Item = dataSourceItem,
                FundSearchData = items,
                SearchBarData = searchbar,
                HotKeywordTags = hotkeywordtags,
                HotProductTags = hotproducttags
            };

            return View("/Views/Feature/Wealth/Component/FundSearch/FundSearch.cshtml", viewModel);

        }


        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        public ActionResult GetAllFunds()
        {
            var items = _repository.GetFundSearchData();
            var funds = _repository.GetFundRenderData(items);
            return new JsonNetResult(funds);
        }

        /// <summary>
        ///autocomplete串接資料
        /// </summary>
        [HttpGet]
        public JsonResult GetFundNames()
        {
            var fundItems = _repository.GetAutoCompleteData();
            return Json(fundItems, JsonRequestBehavior.AllowGet);
        }


    }
}