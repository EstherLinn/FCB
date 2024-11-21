using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class InterestedFundController : Controller
    {
        private readonly InterestedFundRepository _interestedFundRepository = new InterestedFundRepository();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();
        private readonly MemoryCache _cache = MemoryCache.Default;

        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/InterestedFund/InterestedFund.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetInterestedFund()
        {
            return new JsonNetResult(_interestedFundRepository.GetOrSetCacheFundData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetCloseYearPerformanceBatch(string fundIDs)
        {
            var fundIdArray = fundIDs.ToUpper().Split(',');
            JArray performanceDataArray = new JArray();

            foreach (var fundID in fundIdArray)
            {
                string cacheKey = $"CloseYearPerformance_{fundID}";
                var cachedData = _cache.Get(cacheKey) as JObject;

                JObject resultObject = new JObject
                {
                    ["fundId"] = fundID,
                    ["resultSet"] = new JObject()
                };

                if (cachedData != null)
                {
                    resultObject["resultSet"] = cachedData["resultSet"];
                }
                else
                {
                    var resp = await _djMoneyApiRespository.GetGetCloseYearPerformance(fundID);
                    _cache.Add(cacheKey, resp, DateTimeOffset.Now.AddMinutes(30));
                    resultObject["resultSet"] = resp["resultSet"];
                }

                performanceDataArray.Add(resultObject);
            }

            return new JsonNetResult(performanceDataArray);
        }
    }
}