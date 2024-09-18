using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.NewArrivalETF;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Feature.Wealth.Component.Models.NewArrivalETF.NewArrivalEtfModel;

namespace Feature.Wealth.Component.Controllers
{
    public class NewArrivalEtfController : Controller
    {
        private readonly NewArrivalEtfRepository _repository;
        /// <summary>
        /// 建構子
        /// </summary>
        public NewArrivalEtfController()
        {
            _repository = new NewArrivalEtfRepository();
        }

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;

            var etf = _repository.GetFundData();
            var newETF = etf.Where(f => f.ListingDateFormat >= DateTime.Today.AddYears(-1));

            var total = newETF.Count();
            var model = new NewArrivalEtfModel()
            {
                Item = dataSourceItem,
                TotalPages = total,
                NewETFs = newETF
            };

            return View("/Views/Feature/Wealth/Component/NewArrivalEtf/NewArrivalEtf.cshtml", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSortedNewArrivalEtf(string page, string pageSize, string orderby, string desc)
        {
            var etf = _repository.GetFundData();
            var etfs = etf.Where(f => f.ListingDateFormat >= DateTime.Today.AddYears(-1));

            if (page == null) { page = "1"; }
            if (pageSize == null) { pageSize = "10"; }
            if (orderby == null) { orderby = "SixMonthReturnMarketPriceOriginalCurrency"; }
            if (desc == null) { desc = "is-desc"; }

            var property = typeof(ETFs).GetProperty(orderby);
            bool isDesc = desc.Equals("is-desc", StringComparison.OrdinalIgnoreCase);

            etfs = isDesc
                 ? etfs.OrderByDescending(f => property.GetValue(f, null))
                 : etfs.OrderBy(f => property.GetValue(f, null));


            var totalRecords = etfs.Count();
            int totalPages = 1;
            int pageSizeInt = 0;
            int pageInt = 1;

            if (pageSize.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                totalPages = 1;
            }
            else if (int.TryParse(pageSize, out pageSizeInt) && int.TryParse(page, out pageInt))
            {
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                etfs = etfs.Skip((pageInt - 1) * pageSizeInt)
                    .Take(pageSizeInt);
            }
            else
            {
                throw new HttpException(404, $"BadRequest error: {(pageSizeInt > 0 ? string.Empty : "pageSizeInt")} {(pageInt > 0 ? string.Empty : "pageSizeInt")}");
            }

            var model = new NewArrivalEtfModel()
            {
                TotalPages = totalPages,
                CurrentPage = pageInt.ToString(),
                PageSize = pageSize,
                NewETFs = etfs.ToList(),
                DetailLink = EtfRelatedLinkSetting.GetETFDetailUrl()
            };

            return View("/Views/Feature/Wealth/Component/NewArrivalEtf/NewArrivalEtfReturnView.cshtml", model);
        }
    }
}
