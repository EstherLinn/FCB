using Feature.Wealth.Component.Models.Consult;
using Flurl;
using Flurl.Http;
using log4net;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class OctonApiRespository
    {
        private readonly string _route = Settings.GetSetting("Octon");
        private readonly ILog _log = Logger.Api;

        /// <summary>
        /// 取得視訊連結
        /// </summary>
        /// <param name="octonRequestData"></param>
        /// <returns></returns>
        public JObject GetWebURL(OctonRequestData octonRequestData)
        {
            JObject result = null;

            try
            {
                var request = _route.
                AppendPathSegments("mmccmedia", "GetWebURL").
                SetQueryParams(new
                {
                    Tenant = "YJI304",
                    SysCode = "Octon",
                    dnis = octonRequestData.dnis,
                    Date = octonRequestData.Date,
                    Start = octonRequestData.Start,
                    End = octonRequestData.End,
                    EmployeeCode = octonRequestData.EmployeeCode,
                    EmployeeName = octonRequestData.EmployeeName,
                    BranchCode = octonRequestData.BranchCode,
                    BranchName = octonRequestData.BranchName,
                    BranchPhone = octonRequestData.BranchPhone
                }).
                WithHeader("Authorization", octonRequestData.Authorization).
                WithHeader("ContentType", "application/x-www-form-urlencoded;charset=utf-8").
                AllowAnyHttpStatus().
                PostAsync().
                ReceiveString().Result;

                if (!string.IsNullOrEmpty(request))
                {
                    result = JObject.Parse(request);
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
