using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;

namespace Feature.Wealth.Account.Controllers
{
    [MemberAuthenticationFilter]
    public class MemberSettingsController : Controller
    {
        public ActionResult Index()
        {

            return View("~/Views/Feature/Wealth/Account/MemberSettings/Index.cshtml");
        }

    }
}