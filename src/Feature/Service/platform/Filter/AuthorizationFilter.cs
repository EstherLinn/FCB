using Feature.Wealth.Service.Models.WhiteListIp;
using Foundation.Wealth.Helper;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Filter
{
    internal class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // var validatedIps = Settings.GetSetting("ApiAllowedIps"); //TODO: 待確認 config

            // 取得當前請求的 IP 位址
            var request = filterContext.HttpContext.Request;
            //var userIpAddress = GetIPAddress(request);
            var ip = IPHelper.GetIPAddress();

            // 檢查 IP 是否在白名單中
            if (!ConfirmIP(ip))
            {
                //&& Config.IsEnableCheck 加這段會通過本機與開發環境會通過
                // 如果不在白名單中，返回 403 Forbidden
                //filterContext.Result = new HttpStatusCodeResult(403, "IP not allowed");
                filterContext.Result = new JsonNetResult(new { statusCode = 403, message = "IP not allowed" });
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private bool ConfirmIP(string ip)
        {

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            var ipAllow = ApiWhiteListSetting.CkeckApiAllow();

            if (!ipAllow)
            {
                return false;
            }

            var ipList = ApiWhiteListSetting.ApiWhiteList();

            //未上節點，預設通過
            if (ipList == null)
            {
                return true;
            }

            bool confirm = false;

            foreach (string ipTemp in ipList)
            {
                if (ipTemp.Trim().CompareTo(ip) == 0)
                {
                    confirm = true;
                    break;
                }
            }

            return confirm;
        }
    }
}
