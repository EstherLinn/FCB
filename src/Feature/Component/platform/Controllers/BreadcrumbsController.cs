using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Feature.Navigation.Repositories;

namespace Feature.Wealth.Component.Controllers
{
    public class BreadcrumbsController : Controller
    {
        private readonly BreadcrumbNavigationRepository _repository = new();

        public ActionResult Index()
        {
            var model = this._repository.GetBreadcrumb(RenderingContext.CurrentOrNull?.ContextItem);

            return View("/Views/Feature/Wealth/Component/Navigation/Breadcrumbs.cshtml", model);
        }
    }
}