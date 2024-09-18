using Feature.Wealth.Component.Models.AwardFund;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Component.Models.AwardFund.AwardFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class AwardFundController : Controller
    {
        private static AwardFundRepository _repository = new AwardFundRepository();
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var viewModel = new AwardFundModel { Item = dataSourceItem };

            List<Funds> awardFunds = _repository.GetOrSetAwardFundCache();
            viewModel.AwardFunds = awardFunds.OrderByDescending(a => a.Year).ThenBy(a => a.AwardName).ThenBy(a => a.ProductCode).ToList();
            viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();

            return View("/Views/Feature/Wealth/Component/AwardFund/AwardFund.cshtml", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetSortedAwardFund(string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "Year"; }
            if (desc.IsNullOrEmpty()) { desc = "is-desc"; }

            List<Funds> awardFunds = _repository.GetOrSetAwardFundCache();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
            {
                awardFunds = awardFunds.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                awardFunds = awardFunds.OrderBy(f => property.GetValue(f, null)).ToList();
            }

            var viewModel = new AwardFundModel
            {
                AwardFunds = awardFunds,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };
            return View("/Views/Feature/Wealth/Component/AwardFund/AwardFundReturnView.cshtml", viewModel);
        }

    }
}
