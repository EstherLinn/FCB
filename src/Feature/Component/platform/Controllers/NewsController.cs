using Feature.Wealth.Component.Models.News;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult NewsDetails()
        {
            var model = new NewsModel(RenderingContext.CurrentOrNull?.ContextItem);
            return View("/Views/Feature/Wealth/Component/News/NewsDetails.cshtml", model);
        }
    }
}
