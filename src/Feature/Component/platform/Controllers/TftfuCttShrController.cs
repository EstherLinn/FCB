using Feature.Wealth.Component.Models.TftfuStg;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class TftfuCttShrController : Controller
    {
        private TftfuSmartTypeRepository _TftfuSmartTypeRepository = new TftfuSmartTypeRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var viewModel = new TftfuStgModel { Item = dataSourceItem };

            return View("/Views/Feature/Wealth/Component/TftfuCttShr/TftfuCttShr.cshtml", viewModel);
        }

        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllFunds()
        {
            var items = _TftfuSmartTypeRepository.GetCTTFundData1();
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
            var items = _TftfuSmartTypeRepository.GetCTTFundData2();
            var funds = _TftfuSmartTypeRepository.GetFundRenderData(items);
            return new JsonNetResult(funds);
        }


    }
}