using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class ArticleCardListController : Controller
    {
        private readonly ArticleCardListRepository _repository = new ArticleCardListRepository();

        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/ArticleCardList/ArticleCardList.cshtml", _repository.GetArticleCardList());
        }
    }
}