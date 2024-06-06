using Feature.Wealth.Component.Models.Paginator;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class PaginatorController : Controller
    {
        private readonly PaginatorRepository _paginatorRepository = new PaginatorRepository();

        public ActionResult Index()
        {
            Item currentItem = RenderingContext.CurrentOrNull?.Rendering.Item;

            var prevPageItem = _paginatorRepository.GetPreviousPageItem(currentItem);
            var nextPageItem = _paginatorRepository.GetNextPageItem(currentItem);

            var model = new PaginatorModel
            {
                PreviousPage = new PaginatorItem
                {
                    Item = prevPageItem,
                    Title = prevPageItem != null ? ItemUtils.GetFieldValue(prevPageItem, Templates.PageNavigationTitle.Fields.NavigationTitle) : string.Empty,
                    Link = prevPageItem != null ? prevPageItem.Url() : string.Empty
                },
                NextPage = new PaginatorItem
                {
                    Item = nextPageItem,
                    Title = nextPageItem != null ? ItemUtils.GetFieldValue(nextPageItem, Templates.PageNavigationTitle.Fields.NavigationTitle) : string.Empty,
                    Link = nextPageItem != null ? nextPageItem.Url() : string.Empty
                }
            };

            return View("/Views/Feature/Wealth/Component/Paginator/Paginator.cshtml", model);
        }
    }
}