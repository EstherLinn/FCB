using Sitecore;
using Sitecore.Web;
using System;
using System.Web.Mvc;

namespace Feature.Wealth.Toolkit.Attributes
{
    public class ToolkitRoleAttribute : ActionFilterAttribute
    {
        private bool IsDeveloper => Context.User.IsInRole("sitecore\\developer") || Context.User.IsInRole("sitecore\\sitecore client developing");
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Context.User.IsAdministrator && !this.IsDeveloper && !WebUtil.GetCookieValue("___sc___").Equals(DateTime.UtcNow.ToString("yyyyMMdd")))
            {
                filterContext.Result = new RedirectResult("/404");
            }
        }
    }
}