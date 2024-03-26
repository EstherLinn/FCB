using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class FooterController : Controller
    {
        private readonly FooterRepository _bankRepository = new();

        public ActionResult Index()
        {
            return View("~/Views/Feature/Wealth/Component/Footer/Footer.cshtml", this._bankRepository.GetFooter());
        } 
    }
}