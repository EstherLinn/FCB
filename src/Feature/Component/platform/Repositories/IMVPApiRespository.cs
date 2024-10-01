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
        private readonly string _appId = Settings.GetSetting("IMVPApiAppId");
        private readonly string _appKey = Settings.GetSetting("IMVPApiAppKey");
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
                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "app", "verification").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostJsonAsync(new
                    {
                        appId = this._appId,
                        appKey = this._appKey,
                    }).ReceiveString().Result;

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
                throw ex;
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
                throw ex;
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

            // IMVP 要得員工編號只要 6 碼
            if (empId.Length == 8)
            {
                empId = empId.Substring(2, 6);
            }

            try
            {
                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "fm", "getReserved").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostJsonAsync(new
                    {
                        token = token,
                        empId = empId,
                        startDate = startDate,
                        endDate = endDate,
                    }).
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
                throw ex;
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
                throw ex;
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

            // IMVP 要的員工編號只要 6 碼
            if (imvpRequestData.empId.Length == 8)
            {
                imvpRequestData.empId = imvpRequestData.empId.Substring(2, 6);
            }

            try
            {
                var flurlClient = new FlurlClientBuilder(this._route).Build();

                var request = flurlClient.
                    Request().
                    AppendPathSegments("api", "rest", "fm", "reserve").
                    WithHeader("ContentType", "application/json").
                    AllowAnyHttpStatus().
                    PostJsonAsync(new
                    {
                        token = imvpRequestData.token,
                        scheduleId = imvpRequestData.scheduleId,
                        action = imvpRequestData.action,
                        empId = imvpRequestData.empId,
                        type = imvpRequestData.type,
                        date = imvpRequestData.date,
                        startTime = imvpRequestData.startTime,
                        endTime = imvpRequestData.endTime,
                        custId = imvpRequestData.custId,
                        subject = imvpRequestData.subject,
                        description = imvpRequestData.description,
                    }).
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
                throw ex;
            }
            catch (Exception ex)
            {
                this._log.Error(ex);
                throw ex;
            }

            return result;
        }
    }
}
