using Feature.Wealth.Account.Models.OAuth;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace Feature.Wealth.Account.Services
{
    public class LineService : IOAuthTokenService<LineTokenResponse>, IOAuthProfileService<LineUser>
    {
        private readonly string _AppId = Settings.GetSetting("Line.ClientId");
        private readonly string _AppSecret = Settings.GetSetting("Line.ClientSecret");
        private readonly string _TokenUrl = Settings.GetSetting("Line.TokenUrl");
        private readonly string _ProfileUrl = Settings.GetSetting("Line.ProfileUrl");
        private readonly string _VerifyUrl = Settings.GetSetting("Line.VerifyUrl");
        private readonly string _RedirectUrl = $"https://{HttpContext.Current.Request.Url.Host}/api/client/Accounts/SignInLine";
        public string AppId => _AppId;
        public string AppSecret => _AppSecret;
        public string TokenUrl => _TokenUrl;
        public string ProfileUrl => _ProfileUrl;
        public string VerifyUrl => _VerifyUrl;
        public string RedirectUrl => _RedirectUrl;

        public async Task<LineTokenResponse> GetTokensByCode(string code)
        {
            var formContent = new FormUrlEncodedContent(new[]
              {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri",RedirectUrl),
                new KeyValuePair<string, string>("client_id", _AppId),
                new KeyValuePair<string, string>("client_secret", _AppSecret),
                });


            var res = await TokenUrl
                .PostAsync(formContent)
                .ReceiveString();

            return JsonConvert.DeserializeObject<LineTokenResponse>(res);
        }

        public async Task<LineUser> GetProfileByToken(string access_token)
        {
            var res = await ProfileUrl
                .WithOAuthBearerToken(access_token)
                .GetAsync()
                .ReceiveString();
            return JsonConvert.DeserializeObject<LineUser>(res);
        }

        public async Task<LineVerify> GetVerifyAccessToken(string id_token, string uid)
        {
            var formContent = new FormUrlEncodedContent(new[]
           {
            new KeyValuePair<string, string>("id_token", id_token),
            new KeyValuePair<string, string>("client_id", AppId),
            new KeyValuePair<string, string>("user_id", uid),
           });
            var res = await VerifyUrl
                .PostAsync(formContent)
                .ReceiveString();

            return JsonConvert.DeserializeObject<LineVerify>(res);
        }
    }
}
