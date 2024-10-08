using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.USStock;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core.ComponentModel;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;

namespace Feature.Wealth.Component.Controllers
{
    public class USStockController : Controller
    {
        private readonly USStockRepository _uSStockRepository = new USStockRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string USStockSearchCacheKey = $"Fcb_USStockSearchCache";
        private readonly string cacheTime = Settings.GetSetting("USStockSearchCacheTime");

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;
            IList<USStock> uSStockList;
            uSStockList = (IList<USStock>)_cache.Get(USStockSearchCacheKey);

            if (uSStockList == null) {
                uSStockList = this._uSStockRepository.GetUSStockList();
                if (uSStockList != null && uSStockList.Any())
                {
                    _cache.Set(USStockSearchCacheKey, uSStockList, _commonRespository.GetCacheExpireTime(cacheTime));
                }
            }

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