using Sitecore;
using System.Linq;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Foundation.Wealth.Helper
{
    public static class ClientRoute
    {
        /// <summary>
        /// 產生 route 路徑
        /// </summary>
        /// <param name="webViewPage"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        /// <remarks>
        /// 用法: ClientRoute(this, "controller", "action",params (key, value))
        /// </remarks>
        public static string GenerateUrl(WebViewPage webViewPage, string controller, string action, params (string Key, object Value)[] @params)
        {
            var route = new Pipelines.Initialize.ClientRoute();

            var result = @params.Concat(
                new (string Key, object Value)[]
                {
                    ("sc_lang", Context.Language),
                    ("sc_site", Context.Site.Name)
                }).ToArray();
            return webViewPage.GenerateUrl(route.RouteName, null, controller, action, result);
        }
    }
}