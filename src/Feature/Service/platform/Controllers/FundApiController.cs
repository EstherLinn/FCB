using Feature.Wealth.Service.Models.FundApi;
using Feature.Wealth.Service.Repositories;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Controllers
{
    public class FundApiController : JsonNetController
    {
        private readonly FundApiRepository _fundRepository = new FundApiRepository();

        [HttpGet]
        [OutputCache(Duration = 30)]
        public ActionResult FundCorpJson()
        {
            var result = new FundCompanyList()
            {
                Funds = _fundRepository.GetFundCompany("O"),
                WFunds = _fundRepository.GetFundCompany("D")
            };
            return new JsonNetResult(result);
        }

        [HttpGet]
        [OutputCache(Duration = 30)]
        public ActionResult FundTargetJson()
        {
            var result = _fundRepository.GetInvestmentTargets();

            return new JsonNetResult(result);
        }
    }
}