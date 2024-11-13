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
        public ActionResult GetInterestedFund()
        {
            return new JsonNetResult(_interestedFundRepository.GetOrSetCacheFundData());
        }

        [HttpPost]
        public async Task<ActionResult> GetCloseYearPerformance(string fundID)
        {
            string cacheKey = $"CloseYearPerformance_{fundID.ToUpper()}";
            var cachedData = _cache.Get(cacheKey) as JObject;

            if (cachedData != null)
            {
                return new JsonNetResult(cachedData);
            }

            var resp = await _djMoneyApiRespository.GetGetCloseYearPerformance(fundID.ToUpper());
            _cache.Add(cacheKey, resp, DateTimeOffset.Now.AddMinutes(30));
            return new JsonNetResult(resp);
        }
    }
}