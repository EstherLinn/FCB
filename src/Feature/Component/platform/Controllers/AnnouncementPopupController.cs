using Feature.Wealth.Component.Models.AnnouncementPopup;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Foundation.Wealth.Helper;

namespace Feature.Wealth.Component.Controllers
{
    public class AnnouncementPopupController : Controller
    {
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty(WebUtil.GetCookieValue("AnnouncementCheck")))
            {
                var item = RenderingContext.CurrentOrNull?.Rendering.Item;
                var model = new AnnouncementPopupModel(item);
                return View("/Views/Feature/Wealth/Component/AnnouncementPopup/AnnouncementPopup.cshtml", model);
            }
            else
            {
                return View("/Views/Feature/Wealth/Component/AnnouncementPopup/AnnouncementPopup.cshtml", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAnnouncement()
        {
            try
            {
                if (string.IsNullOrEmpty(WebUtil.GetCookieValue("AnnouncementCheck")))
                {
                    this.Response.SetSameSiteCookie("AnnouncementCheck", "1");
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
