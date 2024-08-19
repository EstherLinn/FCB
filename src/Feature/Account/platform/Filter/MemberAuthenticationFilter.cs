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
                filterContext.Result = new HttpUnauthorizedResult();
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var domain = string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName) ? filterContext.HttpContext.Request.Url.Host : Sitecore.Context.Site.TargetHostName;
                    var url = filterContext.HttpContext.Request.RawUrl;
                    filterContext.HttpContext.Response.SetSameSiteCookie("BlockUrl", $"https://{domain}{url}");
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
