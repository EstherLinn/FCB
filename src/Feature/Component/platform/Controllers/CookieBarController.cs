using Feature.Wealth.Component.Models.CookieBar;
using Foundation.Wealth.Helper;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class CookieBarController : Controller
    {
        public ActionResult Index()
        {
            var model = new CookieBarModel();

            if (string.IsNullOrEmpty(WebUtil.GetCookieValue("CookieConsent")))
            {
                var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;
                var content = ItemUtils.GetFieldValue(dataSource, Templates.CookieBarDatasource.Fields.Content);
                var buttonText = ItemUtils.GetFieldValue(dataSource, Templates.CookieBarDatasource.Fields.ButtonText);

                model.DataSource = dataSource;
                model.Content = content;
                model.ButtonText = buttonText;
            }
            else
            {
                model = null;
            }

            return View("/Views/Feature/Wealth/Component/CookieBar/CookieBar.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCookie()
        {
            try
            {
                if (string.IsNullOrEmpty(WebUtil.GetCookieValue("CookieConsent")))
                {
                    this.Response.SetSameSiteCookie("CookieConsent","1");
                    var objReturn = new
                    {
                        success = true
                    };

                    return new JsonNetResult(objReturn);
                }
                else
                {
                    var objReturn = new
                    {
                        success = false
                    };

                    return new JsonNetResult(objReturn);
                }
            }
            catch (Exception ex)
            {
                var objReturn = new
                {
                    success = false,
                    message = ex.Message
                };

                return new JsonNetResult(objReturn);
            }
        }
    }
}