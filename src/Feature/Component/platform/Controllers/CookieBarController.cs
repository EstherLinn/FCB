using Feature.Wealth.Component.Models.CookieBar;
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

            if (string.IsNullOrEmpty(WebUtil.GetCookieValue("accept_Cookie")))
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
        public ActionResult UpdateCookie()
        {
            try
            {
                if (string.IsNullOrEmpty(WebUtil.GetCookieValue("accept_Cookie")))
                {
                    WebUtil.SetCookieValue("accept_Cookie", "1", DateTime.MinValue, true);

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