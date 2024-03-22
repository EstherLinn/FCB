using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.HotFund;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.HotFund.HotFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class HotFundController : Controller
    {
        private HotFundRepository _HotFundRepository = new HotFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, HotFundModel.Template.HotFund.Fields.FundID);
            var viewModel = new HotFundModel { Item = dataSourceItem };
            if (multilineField != null)
            {
                viewModel.selectedId = multilineField;
                var funds = _HotFundRepository.GetFundData();
                var hotFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                hotFunds = hotFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ToList();
                viewModel.HotFunds = _HotFundRepository.GetFundRenderData(hotFunds);
            }
            return View("/Views/Feature/Wealth/Component/HotFund/HotFund.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedHotFund(string[] selectedId, string orderby = "SixMonthReturnOriginalCurrency", string desc = "is-desc")
        {
            var funds = _HotFundRepository.GetFundData();
            var hotFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc == "is-desc")
            {
                hotFunds = hotFunds.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                hotFunds = hotFunds.OrderBy(f => property.GetValue(f, null)).ToList();
            }

            var renderDatas = _HotFundRepository.GetFundRenderData(hotFunds);

            var viewModel = new HotFundModel
            {
                HotFunds = renderDatas
            };
            return View("/Views/Feature/Wealth/Component/HotFund/HotFundReturnView.cshtml", viewModel);
        }


    }
}