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
            string detailLink = USStockRelatedLinkSetting.GetUSStockDetailUrl();

            var hotKeywordTags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.HotKeyword);
            var keywords = hotKeywordTags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName)).ToList();
            var hotProductTags = ItemUtils.GetMultiListValueItems(item, Template.USStock.Fields.HotProduct);
            var products = hotProductTags.Select(f => ItemUtils.GetFieldValue(f, Template.USStockTag.Fields.TagName)).ToList();

            for (int i = 0; i < uSStockList.Count; i++)
            {
                var uSStock = uSStockList[i];
                uSStock.DetailLink = detailLink + "?id=" + uSStock.FirstBankCode;
                uSStock = this._uSStockRepository.GetButtonHtml(uSStock, true);
                uSStock = this._uSStockRepository.SetTags(uSStock);
            }

            var model = new USStockModel
            {
                Item = item,
                USStockList = uSStockList,
                USStockDictionary = uSStockList.ToDictionary(x => x.FirstBankCode, x => x),
                USStockHtmlString = new HtmlString(JsonConvert.SerializeObject(uSStockList)),
                HotKeywordTags = keywords,
                HotProductTags = products
            };

            return model;
        }

    }
}