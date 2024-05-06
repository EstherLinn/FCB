using Feature.Wealth.Component.Models.CookieBar;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class CookieBarController : Controller
    {
        public ActionResult Index()
        {
            var dataSource = RenderingContext.CurrentOrNull?.Rendering.Item;
            var content = ItemUtils.GetFieldValue(dataSource, Templates.CookieBarDatasource.Fields.Content);
            var buttonText = ItemUtils.GetFieldValue(dataSource, Templates.CookieBarDatasource.Fields.ButtonText);
            var style = System.Web.HttpContext.Current.Request.Cookies["accept_Cookie"] != null ? "display:none;" : string.Empty;

            var model = new CookieBarModel()
            {
                DataSource = dataSource,
                Content = content,
                ButtonText = buttonText,
                Style = style
            };

            return View("/Views/Feature/Wealth/Component/CookieBar/CookieBar.cshtml", model);
        }

        [HttpPost]
        public ActionResult UpdateCookie()
        {
            try
            {
                if (Request.Cookies["accept_Cookie"] == null)
                {
                    var cookie = new HttpCookie("accept_Cookie")
                    {
                        Value = "1",
                        Expires = DateTime.Now.AddDays(1),
                        Path = "/",
                        HttpOnly = true
                    };

                    Response.Cookies.Add(cookie);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}