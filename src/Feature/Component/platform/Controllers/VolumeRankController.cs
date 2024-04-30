using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.VolumeRank;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Component.Models.VolumeRank.VolumeRankModel;

namespace Feature.Wealth.Component.Controllers
{
    public class VolumeRankController : Controller
    {
        private VolumeRankRepository _repository = new VolumeRankRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var etfs = _repository.GetFundData();
            var viewModel = new VolumeRankModel
            {
                Item = dataSourceItem,
                VolumeRanks = etfs,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/VolumeRank/VolumeRank.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult GetSortedVolumeRank(string orderby, string desc)
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

            var viewModel = new VolumeRankModel
            {
                VolumeRanks = etfs,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/VolumeRank/VolumeRankReturnView.cshtml", viewModel);
        }
    }
}
