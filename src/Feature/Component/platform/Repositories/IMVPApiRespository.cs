using Feature.Wealth.Component.Models.Consult;
using Flurl;
using Flurl.Http;
using log4net;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using Sitecore.Marketing.Definitions.AutomationPlans.Model;
using Sitecore.Web.UI.HtmlControls;
using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class IMVPApiRespository
    {
        private readonly string _route = Settings.GetSetting("IMVP");

        /// <summary>
        /// 取得 toke
        /// </summary>
        /// <returns></returns>
        public JObject Verification()
        {
            JObject result = null;
            var request = _route.
            AppendPathSegments("api", "rest", "app", "verification").
            SetQueryParams(new
            {
                appId = "WEA_APP",
                appKey = "wea1234",
            }).
            WithHeader("ContentType", "application/json").
            AllowAnyHttpStatus().
            PostAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
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
            var request = _route.
            AppendPathSegments("api", "rest", "fm", "getReserved").
            SetQueryParams(new
            {
                token = token,
                empId = empId,
                startDate = startDate,
                endDate = endDate
            }).
            WithHeader("ContentType", "application/json").
            AllowAnyHttpStatus().
            PostAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
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
            var request = _route.
            AppendPathSegments("api", "rest", "fm", "getReserved").
            SetQueryParams(new
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
                description = imvpRequestData.description
            }).
            WithHeader("ContentType", "application/json").
            AllowAnyHttpStatus().
            PostAsync().
            ReceiveString().Result;

            if (!string.IsNullOrEmpty(request))
            {
                result = JObject.Parse(request);
            }
            return result;
        }
    }
}
