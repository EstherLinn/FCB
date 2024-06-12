using Flurl;
using Flurl.Http;
using Foundation.Wealth.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Services
{
    public class WebBankService
    {
        private readonly string _route = Settings.GetSetting("AppPay.VerifyUrl");
        private readonly string _id = Settings.GetSetting("AppPay.Id");
        private readonly string _key = Settings.GetSetting("AppPay.Key");
        private readonly MemoryCache _cache = MemoryCache.Default;
        public async Task<object> UserVerifyRequest(string callBackUrl,string getQueueId)
        {
            object objReturn = null;
            Uri url = new Uri(callBackUrl);
            url.Query.AppendQueryParam("queueId", getQueueId);
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["queueId"] = getQueueId;
            uriBuilder.Query = query.ToString();
            callBackUrl = uriBuilder.Uri.ToString();
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var computeStr = string.Format("callbackTarget={0}&callbackUri={1}&fnct=2&" +
                "key={4}&merchantId={2}&timestamp={3}&version=1&key={4}",
                "_self", HttpUtility.UrlDecode(callBackUrl), _id, timestamp, _key);
            string routeWithParms = string.Empty;
            try
            {
                 routeWithParms = _route + string.Format("?callbackTarget={0}&callbackUri={1}&fnct=2&" +
                "merchantId={2}&timestamp={3}&version=1&key={4}&sign={5}",
                "_self", CheckUrlParmas(callBackUrl), _id, timestamp, _key, SHA1Helper.Encrypt(computeStr));

                var formData = new
                {
                    callbackTarget = "_self",
                    callbackUri = CheckUrlParmas(callBackUrl),
                    fnct = 2,
                    merchantId = _id,
                    timestamp = timestamp,
                    version = 1,
                    sign = SHA1Helper.Encrypt(computeStr)
                };

                //form post
                var resp = await routeWithParms.PostMultipartAsync(m =>
                m.AddStringParts(formData));

                if (resp.StatusCode < 300)
                {
                    var msg = await resp.GetStringAsync();
                    Logger.Account.Info($"StatusCode:{resp.StatusCode},Success Get Data:{msg}");
                    dynamic data = JsonConvert.DeserializeObject(msg);
                    var computeStr2 = string.Format("merchantId={0}&txReqId={1}&key={2}",
                    _id, data.txReqId, _key);
                    _cache.Set(getQueueId, data.txReqId, DateTimeOffset.Now.AddMinutes(5));
                    objReturn = new
                    {
                        merchantId = _id,
                        txReqId = data.txReqId,
                        sign = SHA1Helper.Encrypt(computeStr2),
                    };
                }

            }
            catch (FlurlHttpException ex)
            {
                Logger.Account.Info($"Error returned request url:${_route}");
                Logger.Account.Info($"Error returned post data: callbacktarget  = _self,callbackuri={CheckUrlParmas(callBackUrl)},fnct=2,merchantid={_id},timestamp={timestamp},version=1,sign={SHA1Helper.Encrypt(computeStr)}");
                Logger.Account.Info($"Error returned from {ex.Call.Request.Url}, StatusCode :{ex.StatusCode} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Account.Info(ex);
            }
            finally
            {
                Logger.Account.Info($"網銀0201 post data = {routeWithParms}");
            }
            return objReturn;
        }

        /// <summary>
        /// 按照AppPay規格　參數兩個以上才需encode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string CheckUrlParmas(string url)
        {
            Uri checkUrl = new Uri(url);
            if (string.IsNullOrEmpty(checkUrl.Query))
            {
                return HttpUtility.UrlDecode(url);
            }
            var urlQuery = HttpUtility.ParseQueryString(checkUrl.Query);
            if (urlQuery.Count > 1)
            {
                return HttpUtility.UrlEncode(url);
            }
            return HttpUtility.UrlDecode(url);
        }
    }
}
