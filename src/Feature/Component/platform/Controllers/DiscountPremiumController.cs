using Feature.Wealth.Component.Models.DiscountPremium;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class DiscountPremiumController : Controller
    {
        private DiscountPremiumRepository _repository = new DiscountPremiumRepository();

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull.Rendering.Item;

            var viewModel = new DiscountPremiumModel
            {
                Item = dataSourceItem
            };

            return View("/Views/Feature/Wealth/Component/DiscountPremium/DiscountPremium.cshtml", viewModel);
        }

        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllEtfs()
        {
            var items = _repository.GetETFData();
            var etfs = _repository.GetFundRenderData(items);
            return new JsonNetResult(etfs);
        }

        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllEtfs2()
        {
            var items = _repository.GetETFData2();
            var etfs = _repository.GetFundRenderData(items);
            return new JsonNetResult(etfs);
        }

    }
}
