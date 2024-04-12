using Feature.Wealth.Component.Models.CommonQuestions;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class CommonQuestionsController : Controller
    {
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;
            return View("~/Views/Feature/Wealth/Component/CommonQuestions/CommonQuestions.cshtml", new CommonQuestionsModel(model));
        }
    }
}
