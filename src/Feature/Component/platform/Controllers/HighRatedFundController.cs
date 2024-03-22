using Feature.Wealth.Component.Models.HighRatedFund;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Linq;
using System.Web.Mvc;
using static Feature.Wealth.Component.Models.HighRatedFund.HighRatedFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class HighRatedFundController : Controller
    {
        private HighRatedFundRepository _repository = new HighRatedFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var funds = _repository.GetFundData();
            var viewModel = new HighRatedFundModel
            {
                Item = dataSourceItem,
                HighRatedFunds = funds
            };

            return View("/Views/Feature/Wealth/Component/HighRatedFund/HighRatedFund.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult GetSortedHighRatedFund(string orderby = "SixMonthReturnOriginalCurrency", string desc = "is-desc")
        {
            var funds = _repository.GetFundData();

            var property = typeof(Funds).GetProperty(orderby);
            if (desc == "is-desc")
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
                HighRatedFunds = renderDatas
            };
            return View("/Views/Feature/Wealth/Component/HighRatedFund/HighRatedFundReturnView.cshtml", viewModel);
        }

    }
}