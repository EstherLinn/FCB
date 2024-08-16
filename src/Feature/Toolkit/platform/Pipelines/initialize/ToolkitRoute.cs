using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Pipelines;

namespace Feature.Wealth.Toolkit.Pipelines.initialize
{
    public class ToolkitRoute : AreaRegistrationBase
    {
        protected string RouteName => nameof(ToolkitRoute);

        public override string AreaName => "Tools";

        public static string GenerateUrl(WebViewPage webViewPage, string controller, string action)
        {
            var route = new ToolkitRoute();

            return webViewPage.GenerateUrl(route.RouteName, route.AreaName, controller, action);
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Namespaces.Add("Feature.Wealth.Toolkit.Areas.Tools.Controllers");
            context.MapRoute(this.RouteName, "cms/{area}/{controller}/{action}", new { action = "index" });
        }
    }
}