using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.TftfuStg;

namespace Feature.Wealth.Component.Controllers
{
    public class TftfuSmartTypeController : Controller
    {
        private TftfuSmartTypeRepository _TftfuSmartTypeRepository = new TftfuSmartTypeRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;
            var viewModel = new TftfuStgModel { Item = dataSourceItem };

            return View("/Views/Feature/Wealth/Component/TftfuSmartType/TftfuSmartType.cshtml", viewModel);
        }

        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllFunds()
        {
            var items = _TftfuSmartTypeRepository.GetFundData();
            var funds = _TftfuSmartTypeRepository.GetFundRenderData(items);
            return new JsonNetResult(funds);
        }

        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllFunds2()
        {
            var items = _TftfuSmartTypeRepository.GetFundData2();
            var funds = _TftfuSmartTypeRepository.GetFundRenderData(items);
            return new JsonNetResult(funds);
        }


    }
}