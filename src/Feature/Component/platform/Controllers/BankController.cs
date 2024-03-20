using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class BankController : SitecoreController
    {
        private readonly BankRepository _bankRepository = new();

        public ActionResult Footer() => View("~/Views/Feature/Wealth/Component/Footer/Footer.cshtml", this._bankRepository.GetFooter());
    }
}
