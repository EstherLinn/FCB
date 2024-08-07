using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class RelatedArticlesLinkController : Controller
    {
        private readonly RelatedArticlesLinkRepository _repository = new RelatedArticlesLinkRepository();

        public ActionResult RelatedArticlesLink()
        {
            return View("/Views/Feature/Wealth/Component/RelatedArticlesLink/RelatedArticlesLink.cshtml", _repository.RelatedArticleLink());
        }
    }
}
