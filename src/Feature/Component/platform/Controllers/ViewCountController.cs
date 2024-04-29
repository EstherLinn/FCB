using System;
using System.Web.Mvc;
using Feature.Wealth.Component.Repositories;

namespace Feature.Wealth.Component.Controllers
{
    public class ViewCountController : Controller
    {
        private readonly ViewCountRepository _repository = new ViewCountRepository();

        [HttpPost]
        public ActionResult GetViewCount(string pageItemId, string currentUrl)
        {
            try
            {
                int viewCount = _repository.GetViewCountInfo(pageItemId, currentUrl);
                return Json(new { success = true, viewCount = viewCount.ToString("N0") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateViewCount(string pageItemId, string currentUrl)
        {
            try
            {
                int viewCount = _repository.UpdateViewCountInfo(pageItemId, currentUrl);
                return Json(new { success = true, viewCount = viewCount.ToString("N0") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
