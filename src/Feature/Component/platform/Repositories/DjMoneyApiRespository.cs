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

        #region 基金

        public async Task<JObject> GetSameLevelFund(string fundId)
        {
            JObject result = null;
            try
            {
                var request = await _route.
                    AppendPathSegments("api", "fund", fundId, "most-recent-five-year-roi-and-fee").
                    WithOAuthBearerToken(_token).
                    AllowAnyHttpStatus().
                    GetAsync();

                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info(ex.Message);
            }
            return result;
        }

        public async Task<JObject> GetGetCloseYearPerformance(string fundId)
        {
            JObject result = null;
            try
            {
                var request = await _route.
               AppendPathSegments("api", "fund", fundId, "roi-duringdate").
               SetQueryParams(new
               {
                   startdate = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow.AddYears(-1)).ToString("yyyy/MM/dd"),
                   enddate = _today,
                   getTWD = 0
               }).
               WithOAuthBearerToken(_token).
               AllowAnyHttpStatus().
               GetAsync();
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info(ex.Message);
            }
            return result;
        }

        public async Task<JObject> GetDocLink(string fundId, string idx)
        {
            JObject result = null;
            try
            {
                var request = await _route.
               AppendPathSegments("api", "fund", fundId, "funddoc").
               SetQueryParams(new
               {
                   idx = idx,
               }).WithOAuthBearerToken(_token).
               AllowAnyHttpStatus().
               GetAsync();
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info(ex.Message);
            }
            return result;
        }

        public async Task<JObject> GetRuleText(string fundId, string type, string indicator)
        {
            JObject result = null;
            try
            {
                var route = indicator == nameof(FundEnum.D) ? "fundtraderule" : "wfundtraderule";
                var isOverseas = indicator == nameof(FundEnum.D) ? "domestic" : "foreign";
                var request = await _route.
                 AppendPathSegments("api", "fund", isOverseas, fundId, route, type).
                 WithOAuthBearerToken(_token).
                 AllowAnyHttpStatus().
                 GetAsync();
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info(ex.Message);
            }
            return result;
        }

        public async Task<JObject> GetReturnAndNetValueTrend(string fundId, string trend, string range, string startdate, string enddate)
        {
            JObject result = null;
            var route = string.Empty;
            FlurlResponse request = null;
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
                switch (trend.ToLower())
                {
                    case nameof(FundRateTrendEnum.ori):
                        request = (FlurlResponse)await _route.AppendPathSegments("api", "fund", fundId, route)
                         .SetQueryParams(new
                         {
                             startdate = startdate,
                             enddate = enddate,
                             getTWD = 0
                         }).WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync();
                        break;

                    case nameof(FundRateTrendEnum.twd):
                        request = (FlurlResponse)await _route.AppendPathSegments("api", "fund", fundId, route)
                       .SetQueryParams(new
                       {
                           startdate = startdate,
                           enddate = enddate,
                           getTWD = 1
                       }).WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync();
                        break;

                    case nameof(FundRateTrendEnum.networth):
                        request = (FlurlResponse)await _route.AppendPathSegments("api", "fund", fundId, route)
                       .SetQueryParams(new
                       {
                           startdate = startdate,
                           enddate = enddate,
                       }).WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync();
                        break;
                }
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info(ex.Message);
            }
            return result;
        }

        #endregion 基金

        public JObject GetGlobalInedxRelevantInformation(string indexCode, int type)
        {
            JObject result = null;
            var request = _route.
            AppendPathSegments("api", "Finance", "finance", "Related", indexCode, type).
            WithOAuthBearerToken(_token).
            AllowAnyHttpStatus().
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
            AllowAnyHttpStatus().
            GetAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public JObject GetNewsForMarketTrend(string id)
        {
            JObject result = null;
            var request = _route.
            AppendPathSegments("api", "News", "kmdjnews", "type", id, 5).
            WithOAuthBearerToken(_token).
            GetAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public JObject GetMarketNewsData(string id, string count, string startDatetime, string endDatetime)
        {
            JObject result = null;
            var request = _route.
                AppendPathSegments("api", "News", "kmdjnews", "type", id, count).
                SetQueryParams(new
                {
                    StartDatetime = startDatetime,
                    EndDatetime = endDatetime
                }).
                WithOAuthBearerToken(_token).
                GetAsync().
                ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        public JObject GetMarketNewsDetailData(string guid)
        {
            JObject result = null;
            var request = _route.
               AppendPathSegments("api", "News", "kmdjnews", "content", guid).
               WithOAuthBearerToken(_token).
               GetAsync().
               ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }

        #region ETF

        private readonly ILog _log = Logger.Api;

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
            try
            {
                switch (type)
                {
                    case EtfReturnTrend.MarketPrice:
                        request = await _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                            .SetQueryParams(new { etfId = etfId, startdate = startdate, enddate = enddate, flag = 1 })
                            .WithOAuthBearerToken(_token)
                            .AllowAnyHttpStatus()
                            .GetAsync()
                            .ReceiveString();
                        break;

                    case EtfReturnTrend.NetAssetValue:
                        request = await _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                            .SetQueryParams(new { etfId = etfId, startdate = startdate, enddate = enddate, flag = 2 })
                            .WithOAuthBearerToken(_token)
                            .AllowAnyHttpStatus()
                            .GetAsync()
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
                this._log.Error($"Error returned from {ex.Call.Request.Url}: {ex}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
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
            try
            {
                var request = await _route.AppendPathSegments("api", "etf", "getreturnchartdata")
                    .SetQueryParams(new { etfId = etfId, startdate = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow.AddYears(-1)).ToString("yyyy/MM/dd"), enddate = _today })
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveString();

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                this._log.Error($"Error returned from {ex.Call.Request.Url}: {ex}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
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

            if (Enum.TryParse(type, false, out EtfKLineChart kLineType))
            {
                period = Xcms.Sitecore.Foundation.Basic.Extensions.EnumUtil.GetEnumDescription(kLineType);
            }

            JObject result = null;
            try
            {
                var request = await _route.AppendPathSegments("api", "etf", "kline")
                    .SetQueryParams(new { etfId = etfId, period = period })
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveString();

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                this._log.Error($"Error returned from {ex.Call.Request.Url}: {ex}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
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
            try
            {
                var request = await _route.AppendPathSegments("api", "etf", etfId, "etfdoc")
                    .SetQueryParams(new { idx = idx })
                    .WithOAuthBearerToken(_token)
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveString();

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
                }
            }
            catch (FlurlHttpException ex)
            {
                this._log.Error($"Error returned from {ex.Call.Request.Url}: {ex}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
            }

            return result;
        }

        #endregion ETF
    }
}