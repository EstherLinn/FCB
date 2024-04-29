using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.CostRank;
using Feature.Wealth.Component.Models.FundDetail;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Component.Models.CostRank.CostRankModel;

namespace Feature.Wealth.Component.Controllers
{
    public class CostRankController : Controller
    {
        private CostRankRepository _repository = new CostRankRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var etfs = _repository.GetFundData();
            var viewModel = new CostRankModel
            {
                Item = dataSourceItem,
                CostRanks = etfs,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/CostRank/CostRank.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult GetSortedCostRank(string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "DiscountPremium"; }
            if (desc.IsNullOrEmpty()) { desc = "is-desc"; }

            var etfs = _repository.GetFundData();

            var property = typeof(ETFs).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
            {
                etfs = etfs.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                etfs = etfs.OrderBy(f => property.GetValue(f, null)).ToList();
            }

            var viewModel = new CostRankModel
            {
                CostRanks = etfs
            };

            return View("/Views/Feature/Wealth/Component/CostRank/CostRankReturnView.cshtml", viewModel);
        }
    }
}
