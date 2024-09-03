using System.Web.Mvc;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;


namespace Feature.Wealth.Component.Controllers
{
    public class USStockDetailController : Controller
    {
        private readonly USStockRepository _uSStockRepository = new USStockRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string firstBankCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            var uSStock = this._uSStockRepository.GetUSStock(firstBankCode);

            return View("/Views/Feature/Wealth/Component/USStock/USStockDetail.cshtml", CreateModel(item, uSStock));
        }

        protected USStockDetailModel CreateModel(Item item, USStock uSStock)
        {
            uSStock = this._uSStockRepository.GetButtonHtml(uSStock, false);
            uSStock = this._uSStockRepository.SetTags(uSStock);

            var model = new USStockDetailModel
            {
                Item = item,
                PageID = USStockRelatedLinkSetting.GetUSStockDetailPageItem()?.ID.ToString(),
                USStock = uSStock,
                b2brwdDomain = ItemUtils.GetFieldValue(item, Template.USStockDetail.Fields.b2brwdDomain)
            };

            return model;
        }
    }
}