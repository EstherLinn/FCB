using Feature.Wealth.Account.Helpers;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Feature.Wealth.Account.Filter
{
    public class MemberAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                filterContext.Result = new HttpUnauthorizedResult();
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
