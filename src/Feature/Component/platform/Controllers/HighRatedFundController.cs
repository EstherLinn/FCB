using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.HighRatedFund;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Component.Models.HighRatedFund.HighRatedFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class HighRatedFundController : Controller
    {
        private HighRatedFundRepository _repository = new HighRatedFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;

            var funds = _repository.GetFundData();
            var viewModel = new HighRatedFundModel
            {
                Item = dataSourceItem,
                HighRatedFunds = funds,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/HighRatedFund/HighRatedFund.cshtml", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSortedHighRatedFund(string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc.IsNullOrEmpty()) { desc = "is-desc"; }

            var funds = _repository.GetFundData();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
            {
                funds = funds.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                funds = funds.OrderBy(f => property.GetValue(f, null)).ToList();
            }

            var renderDatas = _repository.GetFundRenderData(funds);

            var viewModel = new HighRatedFundModel
            {
                HighRatedFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/HighRatedFund/HighRatedFundReturnView.cshtml", viewModel);
        }

    }
}