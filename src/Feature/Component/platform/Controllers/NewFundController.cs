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
        public ActionResult GetSortedNewFund(int page, string pageSize, string orderby, string desc)
        {
            var fund = _repository.GetFundData();
            var newfund = fund.Where(f => f.ListingDateFormat >= DateTime.Today.AddYears(-1));

            if (page == null) { page = 1; }
            if (pageSize == null) { pageSize = "10"; }
            if (orderby == null) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc == null) { desc = "is-desc"; }

            var property = typeof(Funds).GetProperty(orderby);
            bool isDesc = desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase);

            newfund = isDesc
                 ? newfund.OrderByDescending(f => property.GetValue(f, null)).ToList()
                 : newfund.OrderBy(f => property.GetValue(f, null)).ToList();


            var totalRecords = newfund.Count();
            int totalPages;
            List<Funds> renderDatas;
            

            if (pageSize.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                totalPages = 1;
                renderDatas = newfund?.ToList();
            }
            else
            {
                int pageSizeInt = Convert.ToInt32(pageSize);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                renderDatas = newfund?
                    .Skip((page - 1) * pageSizeInt)
                    .Take(pageSizeInt)
                    .ToList();
            }

            var model = new NewFundModel()
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                NewFunds = renderDatas,
                DetailLink = FundRelatedSettingModel.GetFundDetailsUrl()
            };

            return View("/Views/Feature/Wealth/Component/NewFund/NewFundReturn.cshtml", model);
        }


    }
}