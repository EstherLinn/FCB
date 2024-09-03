using Feature.Wealth.Account.Helpers;
using Foundation.Wealth.Helper;
using Sitecore.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Xcms.Sitecore.Foundation.Basic.Extensions;
namespace Feature.Wealth.Account.Filter
{
    public class IMVPAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // 驗證來源 IP
            var safeIP = Settings.GetSetting("IMVPSafeIP");

            var ip = GetIPAddress();

            if (safeIP.Contains(ip) == false)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //回到首頁
                filterContext.Result = new JsonNetResult(new { statusCode = -9999, statusMsg = "不合法的IP" }); ;
            }
        }
    }
}
