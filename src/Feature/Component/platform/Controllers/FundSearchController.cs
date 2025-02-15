﻿using Feature.Wealth.Component.Models.FundSearch;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;
using Newtonsoft.Json;
using System.IO.Compression;
using System.IO;
using System.Text;
using System;

namespace Feature.Wealth.Component.Controllers
{
    public class FundSearchController : Controller
    {
        private readonly FundSearchRepository _fundsearchrepository = new FundSearchRepository();
        private readonly FundTagRepository _tagrepository = new FundTagRepository();
        private readonly CommonRepository _commonRespository = new CommonRepository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string FundSearchCacheKey = $"Fcb_FundSearchCache";
        private readonly string cacheTime = Settings.GetSetting("FundSearchCacheTime");

        public ActionResult Index()
        {
            var dataSourceItem = RenderingContext.CurrentOrNull?.Rendering.Item;
            var hotkeywordtags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.FundSearch.Fields.HotKeywordtags);
            var keywords = new List<string>();
            foreach (var item in hotkeywordtags)
            {
                string key = ItemUtils.GetFieldValue(item, Template.FundSearch.Fields.TagName);
                keywords.Add(key);
            }

            var hotproducttags = ItemUtils.GetMultiListValueItems(dataSourceItem, Template.FundSearch.Fields.HotProductags);
            var products = new List<string>();
            foreach (var item in hotproducttags)
            {
                string product = ItemUtils.GetFieldValue(item, Template.FundSearch.Fields.TagName);
                products.Add(product);
            }
            string content = ItemUtils.GetFieldValue(dataSourceItem, Template.FundSearch.Fields.Content);
            HtmlString riskcontent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.RiskIndicatorContent);
            HtmlString sharpecontent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.SharpeContent);
            HtmlString betacontent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.BetaContent);
            HtmlString alphacontent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.AlphaContent);
            HtmlString standardDeviationContent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.StandardDeviationContent);
            HtmlString annualizedStandardDeviationContent = ItemUtils.Field(dataSourceItem, Template.FundSearch.Fields.AnnualizedStandardDeviationContent);

            var topicnames = _tagrepository.GetFundTenTagNameData();

            var items = _fundsearchrepository.GetFundSearchData();

            var regions = items
                        .OrderBy(f => f.InvestmentRegionID)
                        .SelectMany(f =>
                        {
                            if (!string.IsNullOrEmpty(f.InvestmentRegionName))
                            {
                                return f.InvestmentRegionName.Split(',');
                            }
                            else
                            {
                                return new string[] { };
                            }
                        })
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                        .ToList();



            var searchbar = new SearchBarData
            {
                Currencies = items.OrderBy(f => f.CurrencyCode).Select(f => f.CurrencyName).Distinct().ToList(),
                FundCompanies = items.Where(f => f.FundCompanyName != null).OrderBy(f => f.FundCompanyName).Select(f => f.FundCompanyName).Distinct().ToList(),
                InvestmentRegions = regions,
                InvestmentTargets = items.OrderBy(t => t.InvestmentTargetID).Select(f => f.InvestmentTargetName).Distinct().ToList(),
                FundTypeNames = items.OrderBy(f => f.FormatFundType).Select(f => f.FormatFundType).Distinct().ToList(),
                DividendDistributionFrequencies = _fundsearchrepository.GetDividend()
            };

            var viewModel = new FundSearchViewModel
            {
                Item = dataSourceItem,
                FundSearchData = items,
                SearchBarData = searchbar,
                HotKeywordTags = keywords,
                HotProductTags = products,
                TopicNameTags = topicnames,
                Content = content,
                RiskIndicatorContent = riskcontent,
                SharpeContent = sharpecontent,
                BetaContent = betacontent,
                AlphaContent = alphacontent,
                StandardDeviationContent = standardDeviationContent,
                AnnualizedStandardDeviationContent = annualizedStandardDeviationContent
            };

            return View("/Views/Feature/Wealth/Component/FundSearch/FundSearch.cshtml", viewModel);
        }


        /// <summary>
        /// 搜尋回傳值
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllFunds()
        {
            List<Funds> funds;
            funds = (List<Funds>)_cache.Get(FundSearchCacheKey);

            if (funds == null)
            {
                var items = _fundsearchrepository.GetFundSearchData();
                if (items != null && items.Any())
                {
                    funds = _fundsearchrepository.GetFundRenderData(items);
                    if (funds != null && funds.Any())
                    {
                        _cache.Set(FundSearchCacheKey, funds, _commonRespository.GetCacheExpireTime(cacheTime));
                    }
                }
            }
            var jsonString = JsonConvert.SerializeObject(funds);
            // GZIP 壓縮
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }              
                return Content(Convert.ToBase64String(mso.ToArray()));
            }
        }
    }
}