using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Wealth.Platform.Models.Example;

namespace Project.Wealth.Platform.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Wealth/Example/Index.cshtml", CreateModel());
        }

        protected ExampleModel CreateModel()
        {
            var model = new ExampleModel();
            return model;
        }
    }
}