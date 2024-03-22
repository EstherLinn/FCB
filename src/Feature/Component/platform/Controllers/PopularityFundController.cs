using Feature.Wealth.Component.Models.PopularityFund;
using Feature.Wealth.Component.Repositories
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class PopularityFundController : Controller
    {
        private PopularityFundRepository _popularityFundRepository = new PopularityFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, PopularityFundModel.Template.PopularityFund.Fields.FundID);
            var viewModel = new PopularityFundModel { Item = dataSourceItem };

            if (multilineField != null)
            {
                viewModel.selectedId = multilineField;
                var funds = _popularityFundRepository.GetFundData();
                var popularFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                popularFunds=popularFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ToList();
                viewModel.PopularFunds = _popularityFundRepository.GetFundRenderData(popularFunds);
            }

            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularityFund.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedPopularityFund(string[] selectedId,string orderby= "SixMonthReturnOriginalCurrency", string desc="is-desc")
        {
            var funds = _popularityFundRepository.GetFundData();
            var popularFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc == "is-desc")
            {
                popularFunds = popularFunds.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                popularFunds = popularFunds.OrderBy(f => property.GetValue(f, null)).ToList();
            }

            var renderDatas = _popularityFundRepository.GetFundRenderData(popularFunds);

            var viewModel = new PopularityFundModel
            {
                PopularFunds = renderDatas
            };
            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularFundReturnView.cshtml", viewModel);
        }






    }
}