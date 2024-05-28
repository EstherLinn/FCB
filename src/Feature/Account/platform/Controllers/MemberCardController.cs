using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Account.Models.MemberCard;

namespace Feature.Wealth.Account.Controllers
{
    [MemberAuthenticationFilter]
    public class MemberCardController : Controller
    {
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("~/Views/Feature/Wealth/Account/MemberCard/MemberCard.cshtml",new MemberCardModel(model));
        }

    }
}