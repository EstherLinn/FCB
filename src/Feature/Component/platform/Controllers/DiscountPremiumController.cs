using System;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using Feature.Wealth.Component.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.DiscountPremium;
using static Feature.Wealth.Component.Models.DiscountPremium.DiscountPremiumModel;
using Feature.Wealth.Component.Models.ETF;

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
