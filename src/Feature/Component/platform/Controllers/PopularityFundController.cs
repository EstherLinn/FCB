using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.PopularityFund;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;
using Feature.Wealth.Component.Models.FundDetail;
using Template = Feature.Wealth.Component.Models.PopularityFund.Template;

namespace Feature.Wealth.Component.Controllers
{
    public class PopularityFundController : Controller
    {
        private PopularityFundRepository _popularityFundRepository = new PopularityFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, Template.PopularityFund.Fields.FundID);
            var viewModel = new PopularityFundModel { Item = dataSourceItem };

            if (multilineField != null)
            {
                viewModel.SelectedId = multilineField;
                var funds = _popularityFundRepository.GetFundData();
                var popularFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                var newfunds = popularFunds.OrderByDescending(f => f.ViewCountOrderBy).ToList();
                viewModel.PopularFunds = _popularityFundRepository.GetFundRenderData(newfunds);
                viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();
            }

            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularityFund.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedPopularityFund(string[] selectedId,string orderby, string desc)
        {
            if (orderby==null) { orderby = "ViewCountOrderBy"; }
            if (desc==null) { desc = "is-desc"; }

            var funds = _popularityFundRepository.GetFundData();
            var popularFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
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
                PopularFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularFundReturnView.cshtml", viewModel);
        }






    }
}