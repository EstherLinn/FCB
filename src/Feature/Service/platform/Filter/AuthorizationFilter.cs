using Foundation.Wealth.Models;
using Sitecore.Configuration;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Filter
{
    internal class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var validatedIps = Settings.GetSetting("ApiAllowedIps"); //TODO: 待確認 config

            // 取得當前請求的 IP 位址
            var request = filterContext.HttpContext.Request;
            //var userIpAddress = GetIPAddress(request);
            var ip = GetIPAddress();

            // 檢查 IP 是否在白名單中
            if (!validatedIps.Contains(ip) && Config.IsEnableCheck)
            {
                //&& Config.IsEnableCheck 加這段會通過本機與開發環境會通過
                // 如果不在白名單中，返回 403 Forbidden
                //filterContext.Result = new HttpStatusCodeResult(403, "IP not allowed");
                filterContext.Result = new JsonNetResult(new { statusCode = 403, message = "IP not allowed" });
            }

            base.OnActionExecuting(filterContext);
        }

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
    }
}