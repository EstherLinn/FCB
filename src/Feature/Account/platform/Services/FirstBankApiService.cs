using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.FundTrackList;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Services
{
       public  class FirstBankApiService
    {
        private readonly string _route = Settings.GetSetting("FocusApiRoute");

        public async Task<JObject> GetTrackList(string promotionCode)
        {
            JObject result = null;
            if (string.IsNullOrEmpty(_route))
            {
                return result;
            }
            try
            {
                var reqObj = new { promotionCode = promotionCode, channel = "wms" };
                var request = await _route.
                    AllowAnyHttpStatus().
                    PostJsonAsync(reqObj);
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JObject.Parse(resp);
                    }
                    Logger.Api.Info($"ileo關注清單response: {resp}");
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
    }
}
