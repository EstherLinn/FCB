using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class MultipleFileDownloadListController : Controller
    {
        private readonly MultipleFileDownloadListRepository _repository = new();

        public ActionResult Index()
        {
            return View("/Views/Feature/Wealth/Component/MultipleFileDownloadList/MultipleFileDownloadList.cshtml", this._repository.GetMultipleFileDownloadList());
        }
    }
}