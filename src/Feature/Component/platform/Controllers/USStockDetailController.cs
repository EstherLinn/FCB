using System;
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
        private readonly ViewCountRepository _viewCountrepository = new ViewCountRepository();

        private string _currentUrl = string.Empty;

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            string firstBankCode = Sitecore.Web.WebUtil.GetSafeQueryString("id");

            this._currentUrl = this.ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

            if (item != null)
            {
                this._viewCountrepository.UpdateViewCountInfo(item.ID.ToString(), this._currentUrl);
            }
            var uSStock = this._uSStockRepository.GetUSStock(firstBankCode);

            return View("/Views/Feature/Wealth/Component/USStock/USStockDetail.cshtml", CreateModel(item, uSStock));
        }

        protected USStockDetailModel CreateModel(Item item, USStock uSStock)
        {
            var hotKeywordTagFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Children.HotKeywordTag);
            var hotKeywordTags = ItemUtils.GetChildren(hotKeywordTagFolder, Template.USStockTag.Id);
            var hotProductTagFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Children.HotProductTag);
            var hotProductTags = ItemUtils.GetChildren(hotProductTagFolder, Template.USStockTag.Id);
            var discountFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Children.Discount);
            var discounts = ItemUtils.GetChildren(discountFolder, Template.USStockTag.Id);

            uSStock = this._uSStockRepository.GetButtonHtml(uSStock, false);
            uSStock = this._uSStockRepository.SetTags(uSStock, hotKeywordTags, hotProductTags, discounts);

            if (item != null)
            {
                uSStock.ViewCount = this._viewCountrepository.GetViewCountInfo(item.ID.ToString(), this._currentUrl);
            }

            var model = new USStockDetailModel
            {
                USStock = uSStock,
                b2brwdDomain = ItemUtils.GetFieldValue(item, Template.USStockDetail.Fields.b2brwdDomain)
            };

            return model;
        }
    }
}