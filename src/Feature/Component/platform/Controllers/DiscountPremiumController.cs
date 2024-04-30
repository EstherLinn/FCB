using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.DiscountPremium;
using static Feature.Wealth.Component.Models.DiscountPremium.DiscountPremiumModel;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountPremiumController : Controller
    {
        private DiscountPremiumRepository _repository = new DiscountPremiumRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var etfs = _repository.GetFundData();
            var viewModel = new DiscountPremiumModel
            {
                Item = dataSourceItem,
                DiscountPremiums = etfs,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/DiscountPremium/DiscountPremium.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult GetSortedDiscountPremium(string orderby, string desc)
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

            var viewModel = new DiscountPremiumModel
            {
                DiscountPremiums = etfs,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/DiscountPremium/DiscountPremiumReturnView.cshtml", viewModel);
        }
    }
}
