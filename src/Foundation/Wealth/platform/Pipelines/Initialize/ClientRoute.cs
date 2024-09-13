using System.Web.Mvc;
using System.Web.Routing;
using Xcms.Sitecore.Foundation.Basic.Pipelines;

namespace Foundation.Wealth.Pipelines.Initialize
{
    public class ClientRoute : RegisterMvcRouteBase
    {
        public override string RouteName => nameof(ClientRoute);

        protected override void Register(RouteCollection routes)
        {
            routes.MapRoute(this.RouteName, "api/client/{controller}/{action}", new { param = UrlParameter.Optional });
            routes.MapRoute("Services", "wealthapi/services/{controller}/{action}", new { param = UrlParameter.Optional });
        }
    }
}