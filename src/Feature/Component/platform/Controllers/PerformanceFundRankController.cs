using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.PerformanceFundRank;
using static Feature.Wealth.Component.Models.PerformanceFundRank.PerformanceFundRankModel;

namespace Feature.Wealth.Component.Controllers
{
    public class PerformanceFundRankController : Controller
    {
        private PerformanceFundRankRepository _performanceFundRankRepository = new PerformanceFundRankRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var performanceFund = _performanceFundRankRepository.GetFundData();
            var model = new PerformanceFundRankModel()
            {
                Item = dataSourceItem,
                PerformanceFunds = performanceFund
            };

            return View("/Views/Feature/Wealth/Component/PerformanceFundRank/PerformanceFundRank.cshtml", model);
        }


        [HttpPost]
        public ActionResult GetSortedPerformanceFundRank(string tab, string selectedValue, int page, string pageSize, string orderby, string desc)
        {
            var funds = _performanceFundRankRepository.GetFundData();

            if (tab.IsNullOrEmpty()) { tab = "tab-1"; }
            if (page == null) { page = 1; }
            if (pageSize.IsNullOrEmpty()) { pageSize = "10"; }
            if (orderby.IsNullOrEmpty()) { orderby = "SixMonthReturnOriginalCurrency"; }
            if (desc.IsNullOrEmpty()) { desc = "is-desc"; }


            switch (tab.ToLower())
            {
                case "tab-1":
                    funds = funds.Where(f => f.DomesticForeignFundIndicator == "D").ToList();
                    if (selectedValue != "全部")
                    {
                        funds = funds.Where(f => f.FundTypeName.Contains(selectedValue)).ToList();
                    }
                    break;
                case "tab-2":
                    funds = funds.Where(f => f.DomesticForeignFundIndicator == "O").ToList();
                    if (selectedValue != "全部")
                    {
                        funds = funds.Where(f => f.InvestmentTargetName.Contains(selectedValue)).ToList();
                    }
                    break;
            }

            var property = typeof(Funds).GetProperty(orderby);
            bool isDesc = desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase);

            funds = isDesc
                ? funds.OrderByDescending(f => property.GetValue(f, null)).ToList()
                : funds.OrderBy(f => property.GetValue(f, null)).ToList();


            var totalRecords = funds.Count();
            int totalPages;
            List<Funds> renderDatas;

            if (pageSize.ToLower().Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                totalPages = 1;
                renderDatas = _performanceFundRankRepository.GetFundRenderData(funds).ToList();
            }
            else
            {
                int pageSizeInt = Convert.ToInt32(pageSize);
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                renderDatas = _performanceFundRankRepository.GetFundRenderData(funds)
                    .Skip((page - 1) * pageSizeInt)
                    .Take(pageSizeInt)
                    .ToList();
            }

            var model = new PerformanceFundRankModel()
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                PerformanceFunds = renderDatas
            };

            return View("/Views/Feature/Wealth/Component/PerformanceFundRank/PerformanceFundReturn.cshtml", model);
        }


    }
}