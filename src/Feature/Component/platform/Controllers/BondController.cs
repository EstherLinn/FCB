﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;

namespace Feature.Wealth.Component.Controllers
{
    public class BondController : Controller
    {
        private readonly BondRepository _bondRepository = new BondRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string BondSearchCacheKey = $"Fcb_BondSearchCache";
        private readonly string cacheTime = Settings.GetSetting("BondSearchCacheTime");

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/Bond/Bond.cshtml", CreateModel(item));
        }

        public ActionResult UpDownRank()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/Bond/UpDownRank.cshtml", CreateModel(item));
        }

        public ActionResult BondPrice()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            return View("/Views/Feature/Wealth/Component/Bond/BondPrice.cshtml", CreateModel(item));
        }

        protected BondModel CreateModel(Item item)
        {
            IList<Bond> bondList;
            bondList = (IList<Bond>)_cache.Get(BondSearchCacheKey);

            if (bondList == null) {
                bondList = this._bondRepository.GetBondList();
                if (bondList != null && bondList.Any()) {
                    _cache.Set(BondSearchCacheKey, bondList, _commonRespository.GetCacheExpireTime(cacheTime));
                }
            }

            var hotKeywordTags = ItemUtils.GetMultiListValueItems(item, Template.Bond.Fields.HotKeyword);
            var keywords = hotKeywordTags.Select(f => ItemUtils.GetFieldValue(f, Template.BondTag.Fields.TagName)).ToList();
            var hotProductTags = ItemUtils.GetMultiListValueItems(item, Template.Bond.Fields.HotProduct);
            var products = hotProductTags.Select(f => ItemUtils.GetFieldValue(f, Template.BondTag.Fields.TagName)).ToList();

            var docs = ItemUtils.GetChildren(ItemUtils.GetItem(Template.DocFolder.Id));
            var dic_docs = new Dictionary<string, List<string>>();

            foreach (var doc in docs)
            {
                var key = doc.Name.Length > 4 ? doc.Name.Substring(0, 4) : doc.Name;

                if (dic_docs.ContainsKey(key) == false)
                {
                    var paths = new List<string>
                    {
                        doc.Url()
                    };

                    dic_docs.Add(key, paths);
                }
                else
                {
                    var paths = dic_docs[key];
                    paths.Add(doc.Url());
                    dic_docs[key] = paths;
                }
            }

            if (bondList != null && bondList.Any())
            {
                for (int i = 0; i < bondList.Count; i++)
                {
                    var bondCode = bondList[i].BondCode;
                    if (dic_docs.ContainsKey(bondCode))
                    {
                        bondList[i].DocPaths = dic_docs[bondList[i].BondCode];
                    }

                    if (bondList[i].DocPaths.Count > 0)
                    {
                        var docString = string.Empty;

                        foreach (var path in bondList[i].DocPaths)
                        {
                            docString += $@"<a href=""{path}"" target=""_blank"" class=""o-prefixLink o-prefixLink--pdf t-bold""></a>";
                        }

                        bondList[i].DocString = docString;
                    }
                }
            }

            var model = new BondModel
            {
                Item = item,
                BondList = bondList,
                BondHtmlString = new HtmlString(JsonConvert.SerializeObject(bondList.OrderByDescending(i => i.UpsAndDownsMonth))),
                HotKeywordTags = keywords,
                HotProductTags = products,
                CurrencyList = bondList.OrderBy(i => i.CurrencyCode).Select(i => i.CurrencyName).Distinct(),
                PaymentFrequencyList = bondList.OrderBy(i => i.PaymentFrequency).Select(i => i.PaymentFrequencyName).Distinct(),
                BondClassList = bondList.OrderBy(i => i.BondClass).Select(i => i.BondClass).Where(b => string.IsNullOrEmpty(b) == false).Distinct(),
                IssuerList = bondList.OrderBy(i => i.Issuer).Select(i => i.Issuer).Distinct(),
            };

            return model;
        }
    }
}
