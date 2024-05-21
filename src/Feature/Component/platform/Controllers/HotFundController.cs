using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.HotFund;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.HotFund.HotFundModel;
using Feature.Wealth.Component.Models.FundDetail;
using Template = Feature.Wealth.Component.Models.HotFund.Template;

namespace Feature.Wealth.Component.Controllers
{
    public class HotFundController : Controller
    {
        private HotFundRepository _HotFundRepository = new HotFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, Template.HotFund.Fields.FundID);
            var viewModel = new HotFundModel { Item = dataSourceItem };
            if (multilineField != null)
            {
                viewModel.SelectedId = multilineField;
                var funds = _HotFundRepository.GetFundData();
                var hotFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                hotFunds = hotFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ThenBy(f => f.ProductCode).ToList();
                viewModel.HotFunds = _HotFundRepository.GetFundRenderData(hotFunds);
                viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();
            }
            return View("/Views/Feature/Wealth/Component/HotFund/HotFund.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedHotFund(string[] selectedId, string orderby , string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc.IsNullOrEmpty()) { orderby = "is-desc"; }

            var funds = _HotFundRepository.GetFundData();
            var hotFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
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
                HotFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/HotFund/HotFundReturnView.cshtml", viewModel);
        }


    }
}