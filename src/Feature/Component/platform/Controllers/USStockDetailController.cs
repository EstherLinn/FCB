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

            this._uSStockRepository.TriggerViewCountRecord(firstBankCode);

            var uSStock = this._uSStockRepository.GetUSStock(firstBankCode);

            return View("/Views/Feature/Wealth/Component/USStock/USStockDetail.cshtml", CreateModel(item, uSStock));
        }

        protected USStockDetailModel CreateModel(Item item, USStock uSStock)
        {
            var hotKeywordTagFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Fields.HotKeywordTag);
            var hotKeywordTags = ItemUtils.GetChildren(hotKeywordTagFolder, Template.USStockTag.Id);
            var hotProductTagFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Fields.HotProductTag);
            var hotProductTags = ItemUtils.GetChildren(hotProductTagFolder, Template.USStockTag.Id);
            var discountFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Fields.Discount);
            var discounts = ItemUtils.GetChildren(discountFolder, Template.USStockTag.Id);

            foreach (var f in hotKeywordTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode))
                {
                    uSStock.HotKeywordTags.Add(tagName);
                }
            }

            foreach (var f in hotProductTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode))
                {
                    uSStock.HotProductTags.Add(tagName);
                }
            }

            foreach (var f in discounts)
            {
                string tagName = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode))
                {
                    uSStock.Discount.Add(tagName);
                }
            }

            var model = new USStockDetailModel
            {
                USStock = this._uSStockRepository.GetButtonHtml(uSStock, false),
                b2brwdDomain = ItemUtils.GetFieldValue(item, Template.USStockDetail.Fields.b2brwdDomain)
            };

            return model;
        }
    }
}