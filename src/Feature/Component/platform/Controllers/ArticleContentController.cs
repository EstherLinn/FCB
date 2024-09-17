using Feature.Wealth.Component.Models.ArticleContent;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class ArticleContentController : Controller
    {
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = new ArticleContentModel(item);

            return View("/Views/Feature/Wealth/Component/ArticleContent/ArticleContent.cshtml", model);
        }
    }
}