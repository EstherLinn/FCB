using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Models.Example;

namespace Feature.Wealth.Account.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Feature/Account/Example/Index.cshtml", CreateModel());
        }

        protected ExampleModel CreateModel()
        {
            var model = new ExampleModel();
            return model;
        }
    }
}