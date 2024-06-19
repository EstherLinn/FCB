using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.NewArrivalETF;
using Feature.Wealth.Component.Models.NewFund;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Feature.Wealth.Component.Models.NewArrivalETF.NewArrivalEtfModel;

namespace Feature.Wealth.Component.Controllers
{
    public class NewArrivalEtfController : Controller
    {
        private NewArrivalEtfRepository _repository = new NewArrivalEtfRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

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
                 ? etfs.OrderByDescending(f => property.GetValue(f, null)).ToList()
                 : etfs.OrderBy(f => property.GetValue(f, null)).ToList();


            var totalRecords = etfs.Count();
            int totalPages = 1;
            List<ETFs> renderDatas = new List<ETFs>();
            int pageSizeInt = 0;
            int pageInt = 1;

            if (pageSize.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                totalPages = 1;
                renderDatas = etfs?.ToList();
            }
            else if (int.TryParse(pageSize, out pageSizeInt) && int.TryParse(page, out pageInt))
            {
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSizeInt);
                renderDatas = etfs?
                    .Skip((pageInt - 1) * pageSizeInt)
                    .Take(pageSizeInt)
                    .ToList();
            }

            var model = new NewArrivalEtfModel()
            {
                TotalPages = totalPages,
                CurrentPage = pageInt.ToString(),
                PageSize = pageSize,
                NewETFs = renderDatas,
                DetailLink = EtfRelatedLinkSetting.GetETFDetailUrl()
            };

            return View("/Views/Feature/Wealth/Component/NewArrivalEtf/NewArrivalEtfReturnView.cshtml", model);
        }
    }
}
