using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.AwardFund;
using static Feature.Wealth.Component.Models.AwardFund.AwardFundModel;
using Feature.Wealth.Component.Models.FundDetail;


namespace Feature.Wealth.Component.Controllers
{
    public class AwardFundController : Controller
    {
        private static AwardFundRepository _repository = new AwardFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var viewModel = new AwardFundModel { Item = dataSourceItem };

            Task<List<Funds>> award12024 = Task.Run(() => _repository.JsonPostAsync());

            viewModel.AwardFunds = award12024.Result;
            viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();

            return View("/Views/Feature/Wealth/Component/AwardFund/AwardFund.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedAwardFund(string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "Year"; }
            if (desc.IsNullOrEmpty()) { desc = "is-desc"; }

            Task<List<Funds>> award12024 = Task.Run(() => _repository.JsonPostAsync());
            var awardFunds = award12024.Result;

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
                AwardFunds = awardFunds
            };
            return View("/Views/Feature/Wealth/Component/AwardFund/AwardFundReturnView.cshtml", viewModel);
        }

    }
}
