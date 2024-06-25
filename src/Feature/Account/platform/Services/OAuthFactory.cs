using Feature.Wealth.Account.Models.OAuth;
using Sitecore.Configuration;
using System;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Services
{
    public static class OAuthFactory
    {
        public static string GetAuthUrl(PlatFormEunm oAuthEunm)
        {
            var domain = string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName) ? HttpContext.Current.Request.Url.Host : Sitecore.Context.Site.TargetHostName;
            Guid state = Guid.NewGuid();
            switch (oAuthEunm)
            {
                case PlatFormEunm.FaceBook:
                    string fbAppId = Settings.GetSetting("Facebook.AppId");
                    string fbAuthUrl = Settings.GetSetting("Facebook.AuthUrl");
                    string fbRedirectUrl = $"https://{domain}/api/client/Accounts/SignInFacebook";
                    return fbAuthUrl.Replace("{AppId}", fbAppId).Replace("{RedirectUrl}", fbRedirectUrl).Replace("{state}", state.ToString());
                case PlatFormEunm.Line:
                    string lineAppId = Settings.GetSetting("Line.ClientId");
                    string lineAuthUrl = Settings.GetSetting("Line.AuthUrl");
                    string lineRedirectUrl = $"https://{domain}/api/client/Accounts/SignInLine";
                    return lineAuthUrl.Replace("{scope}", HttpUtility.UrlEncode("profile email openid")).Replace("{state}", state.ToString()).Replace("{RedirectUrl}", lineRedirectUrl).Replace("{clientid}", lineAppId);
                default:
                    return string.Empty;
            }
        }
    }
}
