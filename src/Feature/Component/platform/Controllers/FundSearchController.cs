using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundSearch;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Mvc.Extensions;


namespace Feature.Wealth.Component.Controllers
{
    public class FundSearchController : Controller
    {
        private FundSearchRepository _repository = new FundSearchRepository();

        public ActionResult Index()
        {
            var items = _repository.GetFundSearchData();

            var regions = items
                        .OrderBy(f=>f.InvestmentRegionID)
                        .SelectMany(f =>
                        {
                            if (!string.IsNullOrEmpty(f.InvestmentRegionName))
                            {
                                return f.InvestmentRegionName.Split(',');
                            }
                            else
                            {
                                return new string[] { }; 
                            }
                        })
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                        .ToList();



            var searchbar = new SearchBarData
            {
                Currencies = items.OrderBy(f=>f.CurrencyCode).Select(f=>f.CurrencyName).Distinct().ToList(),
                FundCompanies = items.Where(f => f.FundCompanyName != null).OrderBy(f=>f.FundCompanyName).Select(f => f.FundCompanyName).Distinct().ToList(),
                InvestmentRegions = regions,
                InvestmentTargets = items.OrderBy(t => t.InvestmentTargetID).Select(f => f.InvestmentTargetName).Distinct().ToList(),
                FundTypeNames = items.OrderBy(f=>f.FundTypeName).Select(f => f.FundTypeName).Distinct().ToList()
            };

            var viewModel = new FundSearchViewModel
            {
                FundSearchData = items,
                SearchBarData = searchbar
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