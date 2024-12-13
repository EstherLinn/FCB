using Feature.Wealth.Service.Models.WhiteListIp;
using Foundation.Wealth.Helper;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Service.Filter
{
    internal class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 取得當前請求的 IP 位址
            var request = filterContext.HttpContext.Request;
            var ip = IPHelper.GetIPAddress();

            Logger.Api.Info($"[AuthorizationFilter] Incoming request from IP: {ip}");

            // 檢查 IP 是否在白名單中
            if (!ConfirmIP(ip))
            {
                Logger.Api.Warn($"[AuthorizationFilter] 已開啟白名單功能，未認證 IP: {ip}");

                filterContext.Result = new JsonNetResult(new { statusCode = 403, message = "IP not allowed" });
                return;
            }

            Logger.Api.Info($"[AuthorizationFilter] 已通行 IP: {ip}");

            base.OnActionExecuting(filterContext);
        }

        private bool ConfirmIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                Logger.Api.Warn("[AuthorizationFilter] IP is null or empty");
                return false;
            }

            var ipAllow = ApiWhiteListSetting.CkeckApiAllow();

            if (!ipAllow)
            {
                Logger.Api.Warn("[AuthorizationFilter] IP 白名單功能未開啟");
                return true;
            }

            var ipList = ApiWhiteListSetting.ApiWhiteList();

            //// 未上節點，預設通過
            //if (ipList == null)
            //{
            //    Logger.Api.Info("無白名單節點，允許所有IP");
            //    return true;
            //}

            bool confirm = ipList.Any(ipTemp => ipTemp.Trim().Equals(ip));

            return confirm;
        }
    }
}