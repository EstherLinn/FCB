using Feature.Wealth.Component.ModelBuilders;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Feature.Navigation.Repositories;
using Xcms.Sitecore.Foundation.Caching;

namespace Feature.Wealth.Component.Controllers
{
    public class PrimaryNavigationController : Controller
    {
        private readonly ICacheManager _cache = CachingContext.Current;
        private readonly DeclaredNavigationRepository _repository = new();

        public ActionResult Index()
        {
            var builder = new PrimaryNavigationModelBuilder(this._repository, this._cache);
            var model = builder.GetModel(RenderingContext.CurrentOrNull?.Rendering.Item, RenderingContext.CurrentOrNull?.ContextItem);

            return View("/Views/Feature/Wealth/Component/Navigation/PrimaryNavigation.cshtml", model);
        }
    }
}