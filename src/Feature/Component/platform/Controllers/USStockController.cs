using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core.ComponentModel;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class USStockController : Controller
    {
        private readonly USStockRepository _uSStockRepository = new USStockRepository();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var uSStockList = this._uSStockRepository.GetUSStockList();

            return View("/Views/Feature/Wealth/Component/USStock/USStock.cshtml", CreateModel(item, uSStockList));
        }

        protected USStockModel CreateModel(Item item, IList<USStock> uSStockList)
        {
            string detailLink = ItemUtils.GeneralLink(item, Template.USStock.Fields.DetailLink)?.Url;

            var hotkKeywordTags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.HotKeyword);
            var keyword = hotkKeywordTags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName)).ToList();
            var hotProductTags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.HotProduct);
            var product = hotProductTags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName)).ToList();
            var discountFolder = ItemUtils.GetContentItem(Template.USStockTagFolder.Fields.Discount);
            var discounts = ItemUtils.GetChildren(discountFolder, Template.USStockTag.Id);

            for (int i = 0; i < uSStockList.Count; i++)
            {
                var uSStock = uSStockList[i];
                uSStock.DetailLink = detailLink + "?id=" + uSStock.FirstBankCode;
                uSStock = this._uSStockRepository.GetButtonHtml(uSStock);

                foreach (var f in hotkKeywordTags)
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

                foreach(var f in discounts)
                {
                    string tagName = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName);
                    string productCodeList = ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.ProductCodeList);

                    if (productCodeList.Contains(uSStock.FirstBankCode))
                    {
                        uSStock.Discount.Add(tagName);
                    }
                }
            }

            var model = new USStockModel
            {
                Item = item,
                USStockList = uSStockList,
                USStockDictionary = uSStockList.ToDictionary(x => x.FirstBankCode, x => x),
                USStockHtmlString = new HtmlString(JsonConvert.SerializeObject(uSStockList)),
                HotKeywordTags = keyword,
                HotProductTags = product
            };

            return model;
        }

    }
}