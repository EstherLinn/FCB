using Flurl;
using Flurl.Http;
using Foundation.Wealth.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
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

        public async Task<object> UserVerifyRequest(string callBackUrl)
        {
            JObject result = null;
            object objReturn = null;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var computeStr = string.Format("callbackTarget={0}&callbackUri={1}&fnct=2&" +
                "merchantId={2}&timestamp={3}&version=1&key={4}",
                "_self", HttpUtility.UrlDecode(callBackUrl), _id, timestamp, _key);
            try
            {
                //form post
                var resp = await _route.PostMultipartAsync(mp =>
                mp.AddString("callbacktarget", "_self")
                .AddString("callbackuri", CheckUrlParmas(callBackUrl))
                .AddString("fnct", "2")
                .AddString("merchantid", _id)
                .AddString("timestamp", timestamp)
                .AddString("version", "1")
                .AddString("sign", SHA1Helper.Encrypt(computeStr)));
                if (resp.StatusCode < 300)
                {
                    var msg = await resp.GetStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(msg);
                    var computeStr2 = string.Format("merchantId={0}&txReqId={1}&key={2}",
                    _id, data.txReqId, _key);
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
                Logger.Account.Info($"Error returned post data: callbacktarget  = _self,callbackuri={HttpUtility.UrlEncode(callBackUrl)},fnct=2,merchantid={_id},timestamp={timestamp},version=1,sign={SHA1Helper.Encrypt(computeStr)}" );
                Logger.Account.Info($"Error returned from {ex.Call.Request.Url}, StatusCode :{ex.StatusCode} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Account.Info(ex);
            }
            return objReturn;
        }

        public async Task<JObject> LoginResultInfo()
        {
            JObject result = null;
            var computeStr = string.Format("ack=ok&autoRedirectWaitSec=0&key={0}",string.Empty);
            var resp = await _route.PostMultipartAsync(mp =>
            mp.AddString("ack", "ok")
            .AddString("autoRedirectWaitSec", "0") 
            .AddString("sign", SHA1Helper.Encrypt(computeStr)));
            return result;
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
            if (urlQuery.Count > 2)
            {
                return HttpUtility.UrlEncode(url);
            }
            return HttpUtility.UrlDecode(url);
        }
    }
}
