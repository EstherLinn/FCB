using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.NewFund;
using static Feature.Wealth.Component.Models.NewFund.NewFundModel;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Controllers
{
    public class NewFundController : Controller
    {
        private NewFundRepository _repository = new NewFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var fund = _repository.GetFundData();
            var newfund = fund.Where(f => f.ListingDateFormat >= DateTime.Today.AddYears(-1));

            var total = newfund.Count();
            var model = new NewFundModel()
            {
                Item = dataSourceItem,
                TotalPages = total,
                NewFunds = newfund
            };

            return View("/Views/Feature/Wealth/Component/NewFund/NewFund.cshtml", model);
        }


        [HttpPost]
        public ActionResult GetSortedNewFund(string page, string pageSize, string orderby, string desc)
        {
            var fund = _repository.GetFundData();
            var newfund = fund.Where(f => f.ListingDateFormat >= DateTime.Today.AddYears(-1));

            if (page == null) { page = "1"; }
            if (pageSize == null) { pageSize = "10"; }
            if (orderby == null) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc == null) { desc = "is-desc"; }

            var property = typeof(Funds).GetProperty(orderby);
            bool isDesc = desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase);

            newfund = isDesc
                 ? newfund.OrderByDescending(f => property.GetValue(f, null)).ToList()
                 : newfund.OrderBy(f => property.GetValue(f, null)).ToList();


            var totalRecords = newfund.Count();
            int totalPages = 1;
            List<Funds> renderDatas = new List<Funds>();
            int pageSizeInt = 0;
            int pageInt = 1;


            if (pageSize.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                totalPages = 1;
                renderDatas = newfund?.ToList();
            }
            else if (int.TryParse(pageSize, out pageSizeInt) && int.TryParse(page, out pageInt))
            {
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                renderDatas = newfund?
                    .Skip((pageInt - 1) * pageSizeInt)
                    .Take(pageSizeInt)
                    .ToList();
            }

            var model = new NewFundModel()
            {
                TotalPages = totalPages,
                CurrentPage = pageInt.ToString(),
                PageSize = pageSize,
                NewFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/NewFund/NewFundReturn.cshtml", model);
        }


    }
}