using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.ScheduleAgent.Models.Example;

namespace Feature.ScheduleAgent.Controllers
{
    public class ExampleController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/ScheduleAgent/ScheduleAgent/Example/Index.cshtml", CreateModel());
        }

        protected ExampleModel CreateModel()
        {
            var model = new ExampleModel();
            return model;
        }
    }
}