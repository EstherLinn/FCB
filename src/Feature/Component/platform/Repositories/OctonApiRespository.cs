using Feature.Wealth.Component.Models.Consult;
using Flurl;
using Flurl.Http;
using log4net;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;
using System;
using System.Web;
using System.Xml;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class OctonApiRespository
    {
        private readonly string _route = Settings.GetSetting("OctonApiRoute");
        private readonly string _routeVideo = Settings.GetSetting("OctonVideoApiRoute");
        private readonly string _tenant = Settings.GetSetting("OctonApiTenant");
        private readonly string _sysCode = Settings.GetSetting("OctonApiSysCode");
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
                SetQueryParams($"Tenant={this._tenant}&SysCode={this._sysCode}&dnis={octonRequestData.dnis}&Date={octonRequestData.Date}&Start={HttpUtility.UrlEncode(octonRequestData.Start)}&End={HttpUtility.UrlEncode(octonRequestData.End)}&EmployeeCode={octonRequestData.EmployeeCode}&EmployeeName={octonRequestData.EmployeeName}&BranchCode={octonRequestData.BranchCode}&BranchName={octonRequestData.BranchName}&BranchPhone={octonRequestData.BranchPhone}").
                WithHeader("Authorization", octonRequestData.id).
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

        public string GetVideoURL(string id)
        {
            var result = string.Empty;

            try
            {
                var xmlUrl1 = $@"{this._routeVideo}/RecordingQueryWeb/RecordingInquireAPI?UUID={id}&DOMAIN={this._tenant}&SEQNO=0";
                var xmlUrl2 = $@"{this._routeVideo}/RecordingQueryWeb/RecordingInquireAPI?UUID={id}&DOMAIN={this._tenant}&SEQNO=1";

                XmlDocument doc1 = new XmlDocument();
                XmlDocument doc2 = new XmlDocument();

                doc1.Load(xmlUrl1);
                doc2.Load(xmlUrl2);

                var row1 = doc1.SelectSingleNode("RaiseEvent").SelectSingleNode("Body").SelectSingleNode("Data").SelectSingleNode("Row");
                var row2 = doc2.SelectSingleNode("RaiseEvent").SelectSingleNode("Body").SelectSingleNode("Data").SelectSingleNode("Row");

                if(row1 != null && row2 != null)
                {
                    var url1 = row1.SelectSingleNode("FILE_NAME").Value;
                    var url2 = row2.SelectSingleNode("FILE_NAME").Value;

                    result = $@"{this._routeVideo}/RecordingQueryWeb/Record/Recording_Inquire_vtm.jsp?file1={url1}&file2={url2}";
                }
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
