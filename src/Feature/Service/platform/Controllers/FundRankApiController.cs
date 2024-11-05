using Feature.Wealth.Service.Repositories;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using static Feature.Wealth.Service.Models.FundRankApi.FundRankApiModel;

namespace Feature.Wealth.Service.Controllers
{
    public class FundRankApiController : JsonNetController
    {
        private readonly FundRankApiRepository _fundRankApiRepository = new FundRankApiRepository();

        [HttpPost]
        [OutputCache(Duration = 30, VaryByParam = "data")]
        public ActionResult GetFundRank(string data = "6")
        {
            var result = new ReturnFundSet
            {
                DomesticPerformanceRankFunds = _fundRankApiRepository.GetPerformanceFundRank("D", data),
                ForeignPerformanceRankFunds = _fundRankApiRepository.GetPerformanceFundRank("O", data),
                PopularityFunds = _fundRankApiRepository.GetPopularityFunds(data),
                AwardedFunds = _fundRankApiRepository.GetAwardFunds(data)
            };

            return new JsonNetResult(result);
        }
    }
}