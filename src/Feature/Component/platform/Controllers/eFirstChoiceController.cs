using Feature.Wealth.Component.Models.eFirstChoice;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.eFirstChoice.eFirstChoiceModel;

namespace Feature.Wealth.Component.Controllers
{
    public class eFirstChoiceController : Controller
    {
        private eFirstChoiceRepository _repository = new eFirstChoiceRepository();
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, eFirstChoiceModel.Template.HotFund.Fields.FundID);
            var viewModel = new eFirstChoiceModel { Item = dataSourceItem };
            if (multilineField != null)
            {
                viewModel.selectedId = multilineField;
                var funds = _repository.GetFundData();
                var eFirstFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                eFirstFunds = eFirstFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ToList();
                viewModel.eFirstFunds = _repository.GetFundRenderData(eFirstFunds);
            }
            return View("/Views/Feature/Wealth/Component/eFirstChoice/eFirstChoice.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedeFirstChoice(string[] selectedId, string orderby = "SixMonthReturnOriginalCurrency", string desc = "is-desc")
        {
            var funds = _repository.GetFundData();
            var eFirstFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();
            var property = typeof(Funds).GetProperty(orderby);
            if (desc == "is-desc")
            {
                eFirstFunds = eFirstFunds.OrderByDescending(f => property.GetValue(f, null)).ToList();
            }
            else
            {
                eFirstFunds = eFirstFunds.OrderBy(f => property.GetValue(f, null)).ToList();
            }
            var renderDatas = _repository.GetFundRenderData(eFirstFunds);
            var viewModel = new eFirstChoiceModel
            {
                eFirstFunds = renderDatas
            };
            return View("/Views/Feature/Wealth/Component/eFirstChoice/eFirstChoiceReturnView.cshtml", viewModel);
        }
    }
}
