using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Repositories;

namespace Feature.Wealth.Component.Controllers
{
    public class RecommendedProductController:Controller
    {
        private readonly RecommendedProductRepository _repository = new();

        public ActionResult Index()
        {
            var viewModel = this._repository.recommendProduct(RenderingContext.CurrentOrNull?.Rendering.Item);

            return View("/Views/Feature/Wealth/Component/RecommendedProduct/RecommendedProduct.cshtml", viewModel);
        }
    }

}
