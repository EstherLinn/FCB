using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.PerformanceEtfRank;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Component.Models.PerformanceEtfRank.PerformanceEtfRankModel;

namespace Feature.Wealth.Component.Controllers
{
    public class PerformanceEtfRankController : Controller
    {
        private PerformanceEtfRankRepository _repository = new PerformanceEtfRankRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;

            var etfs = _repository.GetFundData();
            var viewModel = new PerformanceEtfRankModel
            {
                Item = dataSourceItem,
                PerformanceEtfRanks = etfs,
                DetailLink = EtfRelatedLinkSetting.GetETFDetailUrl()
            };

            return View("/Views/Feature/Wealth/Component/PerformanceEtfRank/PerformanceEtfRank.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult GetSortedPerformanceEtfRank(string orderby, string desc)
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

            var viewModel = new PerformanceEtfRankModel
            {
                PerformanceEtfRanks = etfs,
                DetailLink = EtfRelatedLinkSetting.GetETFDetailUrl()
            };

            return View("/Views/Feature/Wealth/Component/PerformanceEtfRank/PerformanceEtfRankReturnView.cshtml", viewModel);
        }
    }
}
