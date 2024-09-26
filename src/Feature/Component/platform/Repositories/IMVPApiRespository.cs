using Feature.Wealth.Component.Models.Consult;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using log4net;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class IMVPApiRespository
    {
        private readonly string _route = Settings.GetSetting("IMVPApiRoute");
        private readonly ILog _log = Logger.Api;

        /// <summary>
        /// 取得 toke
        /// </summary>
        /// <returns></returns>
        public JObject Verification()
        {
            JObject result = null;

            try
            {
                var formContent = new FormUrlEncodedContent([
                    new KeyValuePair<string, string>("appId", "WEA_APP"),
                    new KeyValuePair<string, string>("appKey", "wea1234"),
                ]);

                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "app", "verification").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostAsync(formContent).
                    ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);

                    this._log.Info("IMVPApiRespository Verification result：" + request);
                }
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.StatusCode;
                var resp = ex.GetResponseStringAsync().Result;
                this._log.Error($"Error returned from {ex.Call.Request.Url} {Environment.NewLine}[Message] {ex.Message} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 取得理顧已佔用時段
        /// </summary>
        /// <param name="token"></param>
        /// <param name="empId"></param>
        /// <param name="startDate">YYYYMMDDHHMM</param>
        /// <param name="endDate">YYYYMMDDHHMM</param>
        /// <returns></returns>
        public JObject GetReserved(string token, string empId, string startDate, string endDate)
        {
            JObject result = null;

            try
            {
                var formContent = new FormUrlEncodedContent([
                    new KeyValuePair<string, string>("token", token),
                    new KeyValuePair<string, string>("empId", empId),
                    new KeyValuePair<string, string>("startDate", startDate),
                    new KeyValuePair<string, string>("endDate", endDate),
                ]);

                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "fm", "getReserved").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostAsync(formContent).
                    ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);

                    this._log.Info("IMVPApiRespository GetReserved result：" + request);
                }
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.StatusCode;
                var resp = ex.GetResponseStringAsync().Result;
                this._log.Error($"Error returned from {ex.Call.Request.Url} {Environment.NewLine}[Message] {ex.Message} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 設定預約
        /// </summary>
        /// <param name="imvpRequestData"></param>
        /// <returns></returns>
        public JObject Reserved(IMVPRequestData imvpRequestData)
        {
            JObject result = null;

            try
            {
                var formContent = new FormUrlEncodedContent([
                    new KeyValuePair<string, string>("token", imvpRequestData.token),
                    new KeyValuePair<string, string>("scheduleId", imvpRequestData.scheduleId),
                    new KeyValuePair<string, string>("action", imvpRequestData.action),
                    new KeyValuePair<string, string>("empId", imvpRequestData.empId),
                    new KeyValuePair<string, string>("type", imvpRequestData.type),
                    new KeyValuePair<string, string>("date", imvpRequestData.date),
                    new KeyValuePair<string, string>("startTime", imvpRequestData.startTime),
                    new KeyValuePair<string, string>("endTime", imvpRequestData.endTime),
                    new KeyValuePair<string, string>("custId", imvpRequestData.custId),
                    new KeyValuePair<string, string>("subject", imvpRequestData.subject),
                    new KeyValuePair<string, string>("description", imvpRequestData.description),
                ]);

                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "fm", "getReserved").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostAsync(formContent).
                    ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);

                    this._log.Info("IMVPApiRespository Reserved result：" + request);
                }
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.StatusCode;
                var resp = ex.GetResponseStringAsync().Result;
                this._log.Error($"Error returned from {ex.Call.Request.Url} {Environment.NewLine}[Message] {ex.Message} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
            }

            return result;
        }
    }
}
