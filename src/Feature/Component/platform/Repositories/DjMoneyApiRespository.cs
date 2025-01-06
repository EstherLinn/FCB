using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Models.FundDetail;
using Flurl;
using Flurl.Http;
using log4net;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Foundation.Wealth.Extensions;
using System.Reflection;
using System.Diagnostics;
namespace Feature.Wealth.Component.Repositories
{
    /// <summary>
    ///  介接嘉實api
    /// </summary>
    public class DjMoneyApiRespository
    {
        private readonly string _route = Settings.GetSetting("MoneyDjApiRoute");
        private readonly string _token = Settings.GetSetting("MoneyDjToken");
        private readonly string _today = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow).ToString("yyyy/MM/dd");
        private readonly ILog _log = Logger.Api;

        #region 基金

        public async Task<JObject> GetSameLevelFund(string fundId)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                    AppendPathSegments("api", "fund", fundId, "most-recent-five-year-roi-and-fee");
                var request = await url.                   
                    WithOAuthBearerToken(_token).
                    AllowAnyHttpStatus().
                    GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);

                var resp = await request.GetStringAsync();
                if (!string.IsNullOrEmpty(resp))
                {
                    result = JObject.Parse(resp);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public async Task<JObject> GetGetCloseYearPerformance(string fundId)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                        AppendPathSegments("api", "fund", fundId, "roi-duringdate").
                        SetQueryParams(new
                        {
                            startdate = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow.AddDays(-1).AddYears(-1)).ToString("yyyy/MM/dd"),
                            enddate = Convert.ToDateTime(_today).AddDays(-1).ToString("yyyy/MM/dd"),
                            getTWD = 0
                        });
                var request = await url.
                WithOAuthBearerToken(_token).
                AllowAnyHttpStatus().
                GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);

                var resp = await request.GetStringAsync();
                if (!string.IsNullOrEmpty(resp))
                {
                    result = JObject.Parse(resp);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public async Task<JObject> GetDocLink(string fundId, string idx)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                AppendPathSegments("api", "fund", fundId, "funddoc").
                SetQueryParams(new
                {
                    idx,
                });
                var request = await url
               .WithOAuthBearerToken(_token).
               AllowAnyHttpStatus().
               GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);
                var resp = await request.GetStringAsync();
                if (!string.IsNullOrEmpty(resp))
                {
                    result = JObject.Parse(resp);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public JObject GetDocLink2(string fundId, string idx)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                    AppendPathSegments("api", "fund", fundId, "funddoc").
                    SetQueryParams(new
                    {
                        idx = idx,
                    });
                var request = url
                    .WithOAuthBearerToken(_token).
                    AllowAnyHttpStatus().
                    GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                    ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public async Task<JObject> GetRuleText(string fundId, string type, string indicator)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                var route = indicator == nameof(FundEnum.D) ? "fundtraderule" : "wfundtraderule";
                var isOverseas = indicator == nameof(FundEnum.D) ? "domestic" : "foreign";
                url = _route.
                 AppendPathSegments("api", "fund", isOverseas, fundId, route, type);
                var request = await url.
                 WithOAuthBearerToken(_token).
                 AllowAnyHttpStatus().
                 GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);
                var resp = await request.GetStringAsync();
                if (!string.IsNullOrEmpty(resp))
                {
                    result = JObject.Parse(resp);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public async Task<JObject> GetReturnAndNetValueTrend(string fundId, string trend, string range, string startdate, string enddate)
        {
            JObject result = null;
            var route = string.Empty;
            FlurlResponse request = null;
            var url = string.Empty;
            try
            {
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
                switch (range.ToLower())
                {
                    case "3m":
                    case "6m":
                    case "12m":
                    case "24m":
                    case "36m":
                    case "60m":
                        startdate = Convert.ToDateTime(startdate).AddDays(-1).ToString("yyyy-MM-dd");
                        enddate = Convert.ToDateTime(enddate).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                    case "sinceyear":                       
                    case "establishment":
                        enddate = Convert.ToDateTime(enddate).AddDays(-1).ToString("yyyy-MM-dd");
                        break;
                }
                switch (trend.ToLower())
                {
                    case nameof(FundRateTrendEnum.ori):
                        url = _route.AppendPathSegments("api", "fund", fundId, route)
                         .SetQueryParams(new
                         {
                             startdate = startdate,
                             enddate = enddate,
                             getTWD = 0
                         });
                        request = (FlurlResponse)await url.
                            WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);
                        break;

                    case nameof(FundRateTrendEnum.twd):
                        url = _route.AppendPathSegments("api", "fund", fundId, route)
                       .SetQueryParams(new
                       {
                           startdate = startdate,
                           enddate = enddate,
                           getTWD = 1
                       });
                        request = (FlurlResponse)await url.
                            WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);
                        break;

                    case nameof(FundRateTrendEnum.networth):
                        url = _route.AppendPathSegments("api", "fund", fundId, route)
                       .SetQueryParams(new
                       {
                           startdate = startdate,
                           enddate = enddate,
                       });
                        request = (FlurlResponse)await url.
                            WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url);
                        break;
                }

                var resp = await request.GetStringAsync();
                if (!string.IsNullOrEmpty(resp))
                {
                    result = JObject.Parse(resp);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        #endregion 基金

        public JObject GetBondClass()
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                AppendPathSegments("api", "bond", "Class");
                var request = url.
                WithOAuthBearerToken(_token).
                AllowAnyHttpStatus().
                GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }

        public JObject GetGlobalInedxRelevantInformation(string indexCode, int type)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                AppendPathSegments("api", "Finance", "finance", "Related", indexCode, type);
                var request = url.
                WithOAuthBearerToken(_token).
                AllowAnyHttpStatus().
                GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);

            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }

        public JObject GetGlobalInedxPriceData(string indexCode, string cycle)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                AppendPathSegments("api", "Finance", "finance", "Price", indexCode, cycle);
                var request = url.
                WithOAuthBearerToken(_token).
                AllowAnyHttpStatus().
                GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public JObject GetMarketNewsData(string id, string count, string startDatetime, string endDatetime)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                AppendPathSegments("api", "News", "kmdjnews", "type", id, count).
                SetQueryParams(new
                {
                    StartDatetime = startDatetime,
                    EndDatetime = endDatetime
                });
                var request = url.
                    WithOAuthBearerToken(_token).
                    AllowAnyHttpStatus().
                    GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                    ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }

            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        public JObject GetMarketNewsDetailData(string guid)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.
                   AppendPathSegments("api", "News", "kmdjnews", "content", guid);
                var request = url.
                   WithOAuthBearerToken(_token).
                   AllowAnyHttpStatus().
                   GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url).
                   ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        #region ETF

        /// <summary>
        /// ETF - 績效走勢資訊
        /// </summary>
        /// <param name="etfId">ETF 代碼</param>
        /// <param name="type">市價/淨值</param>
        /// <param name="startdate">起日</param>
        /// <param name="enddate">迄日</param>
        /// <returns></returns>
        public async Task<JObject> GetReturnChartData(string etfId, EtfReturnTrend type, string startdate, string enddate)
        {
            JObject result = null;
            var request = string.Empty;
            var url = string.Empty;
            try
            {
                switch (type)
                {
                    case EtfReturnTrend.MarketPrice:
                        url = _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                            .SetQueryParams(new { etfId = etfId, startdate = startdate, enddate = enddate, flag = 1 });
                        request = await url
                            .WithOAuthBearerToken(_token)
                            .AllowAnyHttpStatus()
                            .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                            .ReceiveString();
                        break;

                    case EtfReturnTrend.NetAssetValue:
                        url = _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                            .SetQueryParams(new { etfId = etfId, startdate = startdate, enddate = enddate, flag = 2 });
                        request = await url
                            .WithOAuthBearerToken(_token)
                            .AllowAnyHttpStatus()
                            .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                            .ReceiveString();
                        break;
                }

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            return result;
        }

        /// <summary>
        /// ETF - 近一年績效走勢資訊
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        public async Task<JObject> GetPerformanceTrend(string etfId)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                    .SetQueryParams(new { etfId = etfId, startdate = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow.AddYears(-1)).ToString("yyyy/MM/dd"), enddate = _today });
                var response = await url
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                    .ReceiveString();

                if (!string.IsNullOrEmpty(response))
                {
                    result = JObject.Parse(response);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }

        /// <summary>
        /// ETF - 技術分析 k 線資訊
        /// </summary>
        /// <param name="etfId">ETF 代碼</param>
        /// <param name="type">週期</param>
        /// <returns></returns>
        public async Task<JObject> GetKLineChart(string etfId, string type)
        {
            var period = string.Empty;
            var url = string.Empty;
            if (Enum.TryParse(type, false, out EtfKLineChart kLineType))
            {
                period = Xcms.Sitecore.Foundation.Basic.Extensions.EnumUtil.GetEnumDescription(kLineType);
            }

            JObject result = null;
            try
            {
                url = _route.AppendPathSegments("api", "etf", "kline")
                    .SetQueryParams(new { etfId = etfId, period = period });
                var response = await url
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                    .ReceiveString();

                if (!string.IsNullOrEmpty(response))
                {
                    result = JObject.Parse(response);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }

        /// <summary>
        /// ETF - PDF文件下載點
        /// </summary>
        /// <param name="etfId">ETF 代碼</param>
        /// <param name="idx">文件類型</param>
        /// <returns></returns>
        /// <remarks>
        /// 文件類型：
        /// 1 基金概覽
        /// 2 公開說明書
        /// 3 簡式公開說明書
        /// 4 年報
        /// </remarks>
        public async Task<JObject> GetEtfDocLink(string etfId, string idx)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.AppendPathSegments("api", "etf", etfId, "etfdoc")
                    .SetQueryParams(new { idx = idx });
                var response = await url
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                    .ReceiveString();

                if (!string.IsNullOrEmpty(response))
                {
                    result = JObject.Parse(response);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);
            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }

        #endregion ETF

        /// <summary>
        /// ETF/基金 績效(報酬) 指標指數走勢資訊
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> GetBenchmarkROIDuringDate(string id, string startdate, string enddate)
        {
            JObject result = null;
            var url = string.Empty;
            try
            {
                url = _route.AppendPathSegments("api", "fund", id, "benchmark-roi-duringdate-all")
                    .SetQueryParams(new { startdate = startdate, enddate = enddate });
                var response = await url
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync().LogIfError(MethodBase.GetCurrentMethod().DeclaringType.FullName, url)
                    .ReceiveString();

                if (!string.IsNullOrEmpty(response))
                {
                    result = JObject.Parse(response);
                }
            }
            catch (FlurlHttpException ex)
            {
                DjMoneyExceptionLog(ex, url);

            }
            catch (Exception ex)
            {
                DjMoneyExceptionLog(ex, url);
            }

            return result;
        }
     

        private void DjMoneyExceptionLog(Exception ex, string requestUrl)
        {
            MethodBase method = (new StackFrame(1)).GetMethod();
            if (ex is FlurlHttpException flurlEx)
            {
                var status = flurlEx.StatusCode;
                var resp = flurlEx.GetResponseStringAsync();
                this._log.Error($"[Function] {method.DeclaringType.FullName} {Environment.NewLine} [Request Url] {requestUrl} {Environment.NewLine},Error returned  from {flurlEx.Call.Request.Url} {Environment.NewLine} [Message] {ex.ToString()} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            else
            {
                this._log.Error($"[Function] {method.DeclaringType.FullName} {Environment.NewLine} [Request Url] {requestUrl} {Environment.NewLine} [Exception] {ex.ToString()}");
            }
        }
    }
}