using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Feature.Wealth.Component.Models.NewFund;
using static Feature.Wealth.Component.Models.NewFund.NewFundModel;

namespace Feature.Wealth.Component.Controllers
{
    public class NewFundController : Controller
    {
        private NewFundRepository _repository = new NewFundRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var newFund = _repository.GetFundData();
            var model = new NewFundModel()
            {
                Item = dataSourceItem,
                NewFunds = newFund
            };

            return View("/Views/Feature/Wealth/Component/NewFund/NewFund.cshtml", model);
        }


        [HttpPost]
        public ActionResult GetSortedNewFund(int page, string pageSize, string orderby, string desc)
        {
            var funds = _repository.GetFundData();

            if (page == null) { page = 1; }
            if (pageSize == null) { pageSize = "10"; }
            if (orderby == null) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc == null) { desc = "is-desc"; }

            var property = typeof(Funds).GetProperty(orderby);
            bool isDesc = desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase);

            funds = isDesc
                 ? funds.OrderByDescending(f => property.GetValue(f, null)).ToList()
                 : funds.OrderBy(f => property.GetValue(f, null)).ToList();


            var totalRecords = funds.Count();
            int totalPages;
            List<Funds> renderDatas;

            if (pageSize.ToLower() == "all")
            {
                totalPages = 1;
                renderDatas = _repository.GetFundRenderData(funds).ToList();
            }
            else
            {
                int pageSizeInt = Convert.ToInt32(pageSize);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                renderDatas = _repository.GetFundRenderData(funds)
                    .Skip((page - 1) * pageSizeInt)
                    .Take(pageSizeInt)
                    .ToList();
            }

            var model = new NewFundModel()
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                NewFunds = renderDatas
            };

            return View("/Views/Feature/Wealth/Component/NewFund/NewFundReturn.cshtml", model);
        }


    }
}