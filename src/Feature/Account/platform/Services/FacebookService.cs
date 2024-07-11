using Feature.Wealth.Account.Models.OAuth;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Flurl.Http;
using Newtonsoft.Json;
using Xcms.Sitecore.Foundation.Basic.Logging;
using log4net;

namespace Feature.Wealth.Account.Services
{
    public class FacebookService : IOAuthTokenService<FBTokenResponse>, IOAuthProfileService<FacebookUser>
    {
        private readonly string domain = string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName) ? HttpContext.Current.Request.Url.Host : Sitecore.Context.Site.TargetHostName;
        private readonly string _AppId = Settings.GetSetting("Facebook.AppId");
        private readonly string _AppSecret = Settings.GetSetting("Facebook.AppSecret");
        private readonly string _TokenUrl = Settings.GetSetting("Facebook.TokenUrl");
        private readonly string _ProfileUrl = Settings.GetSetting("Facebook.ProfileUrl");
        private readonly string _RedirectUrl = $"https://{HttpContext.Current.Request.Url.Host}/api/client/Accounts/SignInFacebook";
        private ILog Log { get; } = Logger.Account;
        public string AppId => _AppId;
        public string AppSecret => _AppSecret;
        public string TokenUrl => _TokenUrl.Replace("{AppId}", _AppId).Replace("{RedirectUrl}", _RedirectUrl.Replace(HttpContext.Current.Request.Url.Host, domain)).Replace("{AppSecret}", _AppSecret);
        public string ProfileUrl => _ProfileUrl;
        public string RedirectUrl => _RedirectUrl;

        public async Task<FBTokenResponse> GetTokensByCode(string code)
        {
            var res = string.Empty;
            try
            {
                res = await TokenUrl.Replace("{code}", code)
                   .GetAsync()
                   .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                Log.Error($"Error returned from {ex.Call.Request.Url}: {ex.ToString()} by FB Login");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"Error returned from Line Login: {ex.ToString()}");
                return null;
            }
            return JsonConvert.DeserializeObject<FBTokenResponse>(res);
        }
        public async Task<FacebookUser> GetProfileByToken(string access_token)
        {
            string combindProfileUrl = $"{ProfileUrl}&access_token={access_token}";
            var res = string.Empty;
            try
            {
                res = await combindProfileUrl
                .GetAsync()
                .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                Log.Error($"Error returned from {ex.Call.Request.Url}: {ex.ToString()} by FB Login");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"Error returned from Line Login: {ex.ToString()}");
                return null;
            }
            return JsonConvert.DeserializeObject<FacebookUser>(res);

        }
    }

}

