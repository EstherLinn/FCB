using Feature.Wealth.Component.Models.Footer;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class FooterController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Feature/Wealth/Component/Footer/Footer.cshtml", FooterModel.GetFooter());
        }
    }
}
