using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.EFirstChoice;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.Models.EFirstChoice.EFirstChoiceModel;
using Feature.Wealth.Component.Models.FundDetail;
using Template = Feature.Wealth.Component.Models.EFirstChoice.Template;

namespace Feature.Wealth.Component.Controllers
{
    public class EFirstChoiceController : Controller
    {
        private EFirstChoiceRepository _repository = new EFirstChoiceRepository();
        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var multilineField = dataSourceItem?.GetMultiLineText(Template.eFirstChoice.Fields.FundID);
            var viewModel = new EFirstChoiceModel { Item = dataSourceItem };
            if (multilineField != null)
            {
                viewModel.SelectedId = multilineField;
                var funds = _repository.GetFundData();
                var eFirstFunds = funds.Where(fund => multilineField.Contains(fund.ProductCode)).ToList();
                eFirstFunds = eFirstFunds.OrderByDescending(f => f.SixMonthReturnOriginalCurrency).ThenBy(f => f.ProductCode).ToList();
                viewModel.EFirstFunds = _repository.GetFundRenderData(eFirstFunds);
                viewModel.DetailLink = FundRelatedSettingModel.GetFundDetailsUrl();
            }
            return View("/Views/Feature/Wealth/Component/EFirstChoice/EFirstChoice.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSortedeFirstChoice(string[] selectedId, string orderby, string desc)
        {
            if (orderby.IsNullOrEmpty()) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc.IsNullOrEmpty()) { orderby = "is-desc"; }

            var funds = _repository.GetFundData();
            var eFirstFunds = funds.Where(fund => selectedId.Contains(fund.ProductCode)).ToList();
            var property = typeof(Funds).GetProperty(orderby);
            if (desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase))
            {
                eFirstFunds = eFirstFunds.OrderByDescending(f => property.GetValue(f, null)).ThenByDescending(f => f.ProductCode).ToList();
            }
            else
            {
                eFirstFunds = eFirstFunds.OrderBy(f => property.GetValue(f, null)).ThenBy(f => f.ProductCode).ToList();
            }
            var renderDatas = _repository.GetFundRenderData(eFirstFunds);
            var viewModel = new EFirstChoiceModel
            {
                EFirstFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/EFirstChoice/EFirstChoiceReturnView.cshtml", viewModel);
        }
    }
}
