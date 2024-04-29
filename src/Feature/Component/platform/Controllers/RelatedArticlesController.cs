using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class RelatedArticlesController : Controller
    {
        private readonly RelatedArticleRepository _repository = new RelatedArticleRepository();

        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/RelatedArticles/RelatedArticles.cshtml", _repository.GetRelatedArticleInfo());
        }
    }
}
