using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.PopularityFund;
using static Feature.Wealth.Component.Models.PopularityFund.PopularityFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class PopularityFundController : Controller
    {
        private readonly PopularityFundRepository _popularityFundRepository = new();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var funds = _popularityFundRepository.GetFundsDatas();
            var renderDatas = _popularityFundRepository.GetFundRenderData(funds);

            var viewModel = new PopularityFundModel
            {
                Item = dataSourceItem,
                PopularFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularityFund.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSortedPopularityFund(string orderby, string desc)
        {
            if (orderby == null) { orderby = "ViewCount"; }
            if (desc == null) { desc = "is-desc"; }

            var popularFunds = _popularityFundRepository.GetFundsDatas();

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

            return View("/Views/Feature/Wealth/Component/PopularityFund/PopularityFundReturnView.cshtml", viewModel);
        }






    }
}