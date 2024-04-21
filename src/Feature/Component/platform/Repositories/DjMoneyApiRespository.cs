using Feature.Wealth.Component.Models.FundDetail;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Feature.Wealth.Component.Repositories
{
    /// <summary>
    ///  界接嘉實api
    /// </summary>
    public class DjMoneyApiRespository
    {
        private readonly string _route = Settings.GetSetting("MoneyDjApiRoute");
        private readonly string _token = Settings.GetSetting("MoneyDjToken");
        private readonly string _today = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow).ToString("yyyy/MM/dd");

        public async Task<JObject> GetSameLevelFund(string fundId)
        {
            JObject result = null;
            var request = await _route.
                AppendPathSegments("api", "fund", fundId, "most-recent-five-year-roi-and-fee").
                WithOAuthBearerToken(_token).
                GetAsync().
                ReceiveString();

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public async Task<JObject> GetGetCloseYearPerformance(string fundId)
        {
            JObject result = null;
            var request = await _route.
           AppendPathSegments("api", "fund", fundId, "roi-duringdate").
           SetQueryParams(new
           {
               startdate = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow.AddYears(-1)).ToString("yyyy/MM/dd"),
               enddate = _today,
               getTWD = 0
           }).
           WithOAuthBearerToken(_token).
           GetAsync().
           ReceiveString();

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }
        public async Task<JObject> GetDocLink(string fundId, string idx)
        {
            JObject result = null;
            var request = await _route.
           AppendPathSegments("api", "fund", fundId, "funddoc").
           SetQueryParams(new
           {
               idx = idx,

           }).WithOAuthBearerToken(_token).
           GetAsync().
           ReceiveString();

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }
        public async Task<JObject> GetRuleText(string fundId, string type, string indicator)
        {
            JObject result = null;
            var route = indicator == nameof(FundEnum.D) ? "fundtraderule" : "wfundtraderule";
            var isOverseas = indicator == nameof(FundEnum.D) ? "domestic" : "foreign";

            var request = await _route.
             AppendPathSegments("api", "fund", isOverseas, fundId, route, type).
             WithOAuthBearerToken(_token).
             GetAsync().
             ReceiveString();

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public async Task<JObject> GetReturnAndNetValueTrend(string fundId, string trend, string range, string startdate, string enddate)
        {
            JObject result = null;
            var route = string.Empty;
            var request = string.Empty;
            switch (trend.ToLower())
            {
                case nameof(FundRateTrendEnum.ori):
                case nameof(FundRateTrendEnum.twd):
                    route = "roi-duringdate";
                    break;
                case nameof(FundRateTrendEnum.networth):
                    route = "nav-duringdate";
                    break;
            }
            if (range.Equals("60m", StringComparison.OrdinalIgnoreCase) || range.Equals("establishment", StringComparison.OrdinalIgnoreCase) || range.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                route += "-all";
            }
            switch (trend.ToLower())
            {
                case nameof(FundRateTrendEnum.ori):
                    request = await _route.AppendPathSegments("api", "fund", fundId, route)
                     .SetQueryParams(new
                     {
                         startdate = startdate,
                         enddate = enddate,
                         getTWD = 0
                     }).WithOAuthBearerToken(_token).
                         GetAsync().
                         ReceiveString();
                    break;
                case nameof(FundRateTrendEnum.twd):
                    request = await _route.AppendPathSegments("api", "fund", fundId, route)
                   .SetQueryParams(new
                   {
                       startdate = startdate,
                       enddate = enddate,
                       getTWD = 1
                   }).WithOAuthBearerToken(_token).
                     GetAsync().
                     ReceiveString();
                    break;
                case nameof(FundRateTrendEnum.networth):
                    request = await _route.AppendPathSegments("api", "fund", fundId, route)
                   .SetQueryParams(new
                   {
                       startdate = startdate,
                       enddate = enddate,
                   }).WithOAuthBearerToken(_token).
                     GetAsync().
                     ReceiveString();
                    break;

            }

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public JObject GetGlobalInedxRelevantInformation(string indexCode, int type)
        {
            JObject result = null;
            var request = _route.
            AppendPathSegments("api", "Finance", "finance", "Related", indexCode, type).
            WithOAuthBearerToken(_token).
            GetAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public JObject GetGlobalInedxPriceData(string indexCode, string cycle)
        {
            JObject result = null;
            var request = _route.
            AppendPathSegments("api", "Finance", "finance", "Price", indexCode, cycle).
            WithOAuthBearerToken(_token).
            GetAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }
    }
}
