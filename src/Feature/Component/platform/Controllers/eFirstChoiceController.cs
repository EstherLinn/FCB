using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.eFirstChoice;
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
            var multilineField = ItemUtils.GetMultiLineText(dataSourceItem, Template.eFirstChoice.Fields.FundID);
            var viewModel = new eFirstChoiceModel { Item = dataSourceItem };
            if (multilineField != null)
            {
                viewModel.SelectedId = multilineField;
                var funds = _repository.GetFundData();
                var eFirstFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                eFirstFunds = eFirstFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ToList();
                viewModel.EFirstFunds = _repository.GetFundRenderData(eFirstFunds);
            }
            return View("/Views/Feature/Wealth/Component/eFirstChoice/eFirstChoice.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult GetSortedeFirstChoice(string[] selectedId, string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc.IsNullOrEmpty()) { orderby = "is-desc"; }

            var funds = _repository.GetFundData();
            var eFirstFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();
            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
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
                EFirstFunds = renderDatas
            };
            
            return View("/Views/Feature/Wealth/Component/eFirstChoice/eFirstChoiceReturnView.cshtml", viewModel);
        }
    }
}
