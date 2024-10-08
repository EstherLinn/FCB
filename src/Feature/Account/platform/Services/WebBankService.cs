using Feature.Wealth.Account.Models.OAuth;
using Feature.Wealth.Account.Repositories;
using Flurl;
using Flurl.Http;
using Foundation.Wealth.Helper;
using Newtonsoft.Json;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
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
        public async Task<object> UserVerifyRequest(string callBackUrl, string getQueueId)
        {
            //default return
            object objReturn = new
            {
                success = false
            };
            var step = string.Empty;
            step = $"Step1 網址參數加入queueId";
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
                step = $"Step2  call 0201";
                //form post
                var resp = await routeWithParms.PostMultipartAsync(m =>
                m.AddStringParts(formData));

                if (resp.StatusCode < 300)
                {
                    step = $"Step3 回覆200，解析response參數";
                    var msg = await resp.GetStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
                    if (!data.TryGetValue("txReqId", out string txReqIdString))
                    {
                        return objReturn;
                    }
                    var computeStr2 = string.Format("merchantId={0}&txReqId={1}&key={2}",
                    _id, txReqIdString, _key);
                    LoginSharedRepository loginSharedRepository = new LoginSharedRepository();
                    loginSharedRepository.RecordTansaction(getQueueId, txReqIdString);
                    //success return
                    objReturn = new
                    {
                        success = true,
                        merchantId = _id,
                        txReqId = txReqIdString,
                        sign = SHA1Helper.Encrypt(computeStr2),
                    };
                    Logger.Account.Info($"網銀0201 {step}  routeWithParms = {routeWithParms} ,merchantId={_id},txReqId={txReqIdString},sign={SHA1Helper.Encrypt(computeStr2)}");
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Account.Info($"網銀0201 {step} routeWithParms = {routeWithParms} ,StatusCode :{ex.StatusCode} , Error Message : {ex.ToString()}");
            }
            catch (Exception ex)
            {
                Logger.Account.Info($"網銀0201 {step} routeWithParms = {routeWithParms}, Exception Message {ex.ToString()}");
            }
            return objReturn;
        }

        /// <summary>
        /// 按照AppPay規格　參數一個以上才需encode
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
