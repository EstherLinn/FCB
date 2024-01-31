using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Octon.Models.Example;

namespace Feature.Wealth.Octon.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/Octon/Octon/Example/Index.cshtml", CreateModel());
        }

        protected ExampleModel CreateModel()
        {
            var model = new ExampleModel();
            return model;
        }
    }
}