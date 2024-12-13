using Feature.Wealth.Service.Models.WhiteListIp;
using Foundation.Wealth.Helper;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Filter
{
    internal class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 取得當前請求的 IP 位址
            var ip = IPHelper.GetIPAddress();

            ApiWhiteListSetting.Log(LogLevel.Info, "Incoming request from IP:", ip);

            // 檢查 IP 是否在白名單中
            if (!ConfirmIP(ip))
            {
                ApiWhiteListSetting.Log(LogLevel.Warn, "已開啟白名單功能，未認證", ip);

                filterContext.Result = new JsonNetResult(new { statusCode = 403, message = "IP not allowed" });
                return;
            }

            ApiWhiteListSetting.Log(LogLevel.Info, " 已通行", ip);

            base.OnActionExecuting(filterContext);
        }

        private bool ConfirmIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                ApiWhiteListSetting.Log(LogLevel.Warn, "IP is null or empty", ip);
                return false;
            }

            var ipAllow = ApiWhiteListSetting.CkeckApiAllow();

            if (!ipAllow)
            {
                ApiWhiteListSetting.Log(LogLevel.Warn, "IP 白名單功能未開啟", ip);
                return true;
            }

            var ipList = ApiWhiteListSetting.ApiWhiteList();

            bool confirm = ipList.Any(ipTemp => ipTemp.Trim().Equals(ip));

            return confirm;
        }
    }
}