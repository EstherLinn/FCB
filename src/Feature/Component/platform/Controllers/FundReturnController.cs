using Feature.Wealth.Component.Models.FundReturn;
using Feature.Wealth.Component.Repositories;
using System;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class FundReturnController : Controller
    {
        private readonly FundReturnRepository _fundReturnRepository = new FundReturnRepository();

        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/FundReturn/FundReturn.cshtml", new FundReturnSearchFilterResponse());
        }

        [HttpPost]
        public ActionResult GetFundReturnSearchFilterData()
        {
            var res = new FundReturnSearchFilterResponse()
            {
                FundReturnSearch = _fundReturnRepository.GetFundReturnSearchData(),
                FundReturnFilter = _fundReturnRepository.GetFundReturnFilterData()
            };

            return new JsonNetResult(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFundReturnDetail(string productCode)
        {
            if (productCode == null)
            {
                return new EmptyResult();
            }

            var fundReturnDetail = _fundReturnRepository.GetFundReturnDetail(productCode);
            return new JsonNetResult(this.RenderRazorViewToString("/Views/Feature/Wealth/Component/FundReturn/_FundReturnDetail.cshtml", fundReturnDetail).Replace(Environment.NewLine, string.Empty));
        }
    }
}