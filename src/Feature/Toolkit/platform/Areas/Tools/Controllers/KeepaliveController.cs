using System.Web.Mvc;

namespace Feature.Wealth.Toolkit.Areas.Tools.Controllers
{
    public class KeepaliveController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Areas/Tools/Views/Keepalive/Index.cshtml");
        }
    }
}