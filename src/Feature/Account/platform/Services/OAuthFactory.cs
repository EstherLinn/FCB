﻿using Feature.Wealth.Account.Models.OAuth;
using Sitecore.Configuration;
using System;
using System.Web;

namespace Feature.Wealth.Account.Services
{
    public static class OAuthFactory
    {
        public static string GetAuthUrl(PlatFormEunm oAuthEunm)
        {
            Guid state = Guid.NewGuid();
            switch (oAuthEunm)
            {
                case PlatFormEunm.FaceBook:
                    string fbAppId = Settings.GetSetting("Facebook.AppId");
                    string fbAuthUrl = Settings.GetSetting("Facebook.AuthUrl");
                    string fbRedirectUrl = $"https://{HttpContext.Current.Request.Url.Host}/api/client/Accounts/SignInFacebook";
                    return fbAuthUrl.Replace("{AppId}", fbAppId).Replace("{RedirectUrl}", fbRedirectUrl).Replace("{state}", state.ToString());
                case PlatFormEunm.Line:
                    string lineAppId = Settings.GetSetting("Line.ClientId");
                    string lineAuthUrl = Settings.GetSetting("Line.AuthUrl");
                    //string lineRedirectUrl = $"https://{HttpContext.Current.Request.Url.Host}/Demo/Jason/Oauth";
                    string lineRedirectUrl = $"https://{HttpContext.Current.Request.Url.Host}/api/client/Accounts/SignInLine";

                    return lineAuthUrl.Replace("{scope}", HttpUtility.UrlEncode("profile email openid")).Replace("{state}", state.ToString()).Replace("{RedirectUrl}", lineRedirectUrl).Replace("{clientid}", lineAppId);
                default:
                    return string.Empty;
            }
        }
    }
}
