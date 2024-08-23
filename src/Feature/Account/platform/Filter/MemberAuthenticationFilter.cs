using Feature.Wealth.Account.Helpers;
using Foundation.Wealth.Helper;
using Sitecore.Web;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
namespace Feature.Wealth.Account.Filter
{
    public class MemberAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                var domain = string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName) ? filterContext.HttpContext.Request.Url.Host : Sitecore.Context.Site.TargetHostName;
                var url = filterContext.HttpContext.Request.RawUrl;
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.SetSameSiteCookie("BlockUrl", $"https://{domain}{url}");
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    var pageUrl = filterContext.HttpContext.Request.Params["pageUrl"];
                    filterContext.HttpContext.Response.SetSameSiteCookie("BlockUrl", $"https://{domain}{pageUrl}");
                }

            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //回到首頁
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}
