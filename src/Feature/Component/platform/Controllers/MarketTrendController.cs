using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.MarketTrend;
using Feature.Wealth.Component.Models.News;
using Feature.Wealth.Component.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class MarketTrendController : Controller
    {
        private readonly GlobalIndexRepository _globalIndexRepository = new GlobalIndexRepository();
        private readonly MarketTrendRepository _marketTrendRepository = new MarketTrendRepository();
        private IList<Models.GlobalIndex.GlobalIndex> _globalIndexList;
        private List<Models.GlobalIndex.GlobalIndexHighchartsData> _datas = new List<Models.GlobalIndex.GlobalIndexHighchartsData>();
        private List<RelevantInformation> _stockRelevantFund = new List<RelevantInformation>();
        private List<RelevantInformation> _stockRelevantETF = new List<RelevantInformation>();
        private List<RelevantInformation> _bondRelevantFund = new List<RelevantInformation>();
        private List<RelevantInformation> _bondRelevantETF = new List<RelevantInformation>();
        private List<RelevantInformation> _industryRelevantFund = new List<RelevantInformation>();
        private List<RelevantInformation> _industryRelevantETF = new List<RelevantInformation>();

        private IEnumerable<RelevantInformation> _fundList = new List<RelevantInformation>();
        private IEnumerable<RelevantInformation> _etfList = new List<RelevantInformation>();

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            var model = CreateModel(item);

            return View("/Views/Feature/Wealth/Component/MarketTrend/MarketTrend.cshtml", model);
        }

        protected MarketTrendModel CreateModel(Item item)
        {
            var model = new MarketTrendModel
            {
                Item = item,
                IndexLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.IndexLink)?.Url,
                FundLink = Models.FundDetail.FundRelatedSettingModel.GetFundDetailsUrl(),
                ETFLink = Models.ETF.EtfRelatedLinkSetting.GetETFDetailUrl(),
                NewsLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.NewsLink)?.Url,
                MoreNewsButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreNewsButtonText),
                MoreNewsButtonLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.MoreNewsButtonLink)?.Url,
                MoreETFButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreETFButtonText),
                MoreETFButtonLink = Models.FundDetail.FundRelatedSettingModel.GetFundSearchUrl(),
                MoreFundButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreFundButtonText),
                MoreFundButtonLink = Models.ETF.EtfRelatedLinkSetting.GetETFSearchUrl(),
                ReportTitle = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.ReportTitle),
                ReportText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.ReportText),
                ReportButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.ReportButtonText),
                ReportButtonLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.ReportButtonLink)?.Url
            };

            var stockFolder = ItemUtils.GetItem(Template.Stock.Id);
            var bondFolder = ItemUtils.GetItem(Template.Bond.Id);
            var industryFolder = ItemUtils.GetItem(Template.Industry.Id);

            var stockChildren = ItemUtils.GetChildren(stockFolder, Template.MarketTrendItem.Id);
            var bondChildren = ItemUtils.GetChildren(bondFolder, Template.MarketTrendItem.Id);
            var industryChildren = ItemUtils.GetChildren(industryFolder, Template.MarketTrendItem.Id);

            this._globalIndexList = this._globalIndexRepository.GetGlobalIndexList();
            this._fundList = this._marketTrendRepository.GetFundList();
            this._etfList = this._marketTrendRepository.GetETFList();

            model.Stock = CreateMarketTrendList(item, stockChildren, RelevantInformationType.Stock);
            model.Bond = CreateMarketTrendList(item, bondChildren, RelevantInformationType.Bond);
            model.Industry = CreateMarketTrendList(item, industryChildren, RelevantInformationType.Industry);

            model.GlobalIndexHighchartsDataHtmlString = new HtmlString(JsonConvert.SerializeObject(this._datas));

            model.StockRelevantFundHtmlString = new HtmlString(JsonConvert.SerializeObject(this._stockRelevantFund));
            model.StockRelevantETFHtmlString = new HtmlString(JsonConvert.SerializeObject(this._stockRelevantETF));
            model.BondRelevantFundHtmlString = new HtmlString(JsonConvert.SerializeObject(this._bondRelevantFund));
            model.BondRelevantETFHtmlString = new HtmlString(JsonConvert.SerializeObject(this._bondRelevantETF));
            model.IndustryRelevantFundHtmlString = new HtmlString(JsonConvert.SerializeObject(this._industryRelevantFund));
            model.IndustryRelevantETFHtmlString = new HtmlString(JsonConvert.SerializeObject(this._industryRelevantETF));

            return model;
        }

        private List<MarketTrend> CreateMarketTrendList(Item item, IEnumerable<Item> children, RelevantInformationType relevantInformationType)
        {
            var result = new List<MarketTrend>();

            string indexLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.IndexLink)?.Url;
            string fundLink = Models.FundDetail.FundRelatedSettingModel.GetFundDetailsUrl();
            string etfLink = Models.ETF.EtfRelatedLinkSetting.GetETFDetailUrl();
            var newsLinkItem = ItemUtils.GetReferenceFieldItem(item, Template.MarketTrend.Fields.NewsLink);
            string newsLink = newsLinkItem.Url();

            foreach (var child in children)
            {
                var marketTrend = new MarketTrend
                {
                    Title = ItemUtils.GetFieldValue(child, Template.MarketTrendItem.Fields.Title)
                };

                var date = ItemUtils.GetDateTimeFieldValue(child, Template.MarketTrendItem.Fields.Date);
                if (date != null && date.HasValue)
                {
                    marketTrend.Date = date.Value.Year + "年" + date.Value.Month + "月";
                }

                var degree = ItemUtils.GetReferenceFieldItem(child, Template.MarketTrendItem.Fields.Degree);

                marketTrend.Degree = ItemUtils.GetFieldValue(degree, Template.DropdownOption.Fields.OptionValue);
                marketTrend.DegreeText = ItemUtils.GetFieldValue(degree, Template.DropdownOption.Fields.OptionText);

                // 處理相關指數
                var indexItems = ItemUtils.GetMultiListValueItems(child, Template.MarketTrendItem.Fields.IndexCode);
                var indexCodes = new List<string>();

                foreach (var indexItem in indexItems)
                {
                    indexCodes.Add(ItemUtils.GetFieldValue(indexItem, Template.IndexItem.Fields.IndexCode));
                }

                var temp = new List<Models.GlobalIndex.GlobalIndex>();

                foreach (string indexCode in indexCodes)
                {
                    var index = this._globalIndexList.First(c => c.IndexCode.ToLower().Trim() == indexCode.ToLower().Trim());

                    if (index != null)
                    {
                        if (string.IsNullOrEmpty(index.DetailLink))
                        {
                            index.DetailLink = indexLink + "?id=" + index.IndexCode;
                        }

                        if (index.GlobalIndexHistory == null)
                        {
                            index.GlobalIndexHistory = this._globalIndexRepository.GetGlobalIndexHistoryList(index.IndexCode);
                        }

                        temp.Add(index);

                        if (!this._datas.Any(c => c.IndexCode == index.IndexCode))
                        {
                            this._datas.Add(new Models.GlobalIndex.GlobalIndexHighchartsData
                            {
                                IndexCode = index.IndexCode,
                                Data = index.GlobalIndexHistory
                            });
                        }
                    }
                }

                if (temp.Count > 0)
                {
                    // 為了前端格式要三個一組
                    var temp3 = new List<Models.GlobalIndex.GlobalIndex>();

                    foreach (var index in temp)
                    {
                        temp3.Add(index);

                        if (temp3.Count == 3)
                        {
                            marketTrend.RelevantGlobalIndex.Add(temp3);

                            temp3 = new List<Models.GlobalIndex.GlobalIndex>();
                        }
                    }

                    if (temp3.Count > 0)
                    {
                        marketTrend.RelevantGlobalIndex.Add(temp3);
                    }
                }

                // 處理相關基金
                string fundCodeString = ItemUtils.GetFieldValue(child, Template.MarketTrendItem.Fields.FundCode);

                if (!string.IsNullOrEmpty(fundCodeString))
                {
                    string[] fundCodes = fundCodeString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    var tempFunds = new List<RelevantInformation>();

                    foreach (string fundCode in fundCodes)
                    {
                        // 最多5筆
                        if (tempFunds.Count == 5)
                        {
                            break;
                        }

                        if (this._fundList.Any(c => c.ProductCode.ToLower().Trim() == fundCode.ToLower().Trim()))
                        {
                            var fund = this._marketTrendRepository.CloneRelevantInformation(this._fundList.FirstOrDefault(c => c.ProductCode.ToLower().Trim() == fundCode.ToLower().Trim()));
                            fund.DetailLink = fundLink + "?id=" + fund.ProductCode;
                            fund.Title = marketTrend.Title;

                            fund = this._marketTrendRepository.GetButtonHtml(fund, InvestTypeEnum.Fund);

                            if (!tempFunds.Any(c => c.ProductCode == fund.ProductCode))
                            {
                                tempFunds.Add(fund);
                            }
                        }
                    }

                    // 照六個月績效做預設排序
                    tempFunds = tempFunds.OrderByDescending(c => c.M6Change).ToList();

                    if (relevantInformationType == RelevantInformationType.Stock)
                    {
                        this._stockRelevantFund.AddRange(tempFunds);
                    }
                    else if (relevantInformationType == RelevantInformationType.Bond)
                    {
                        this._bondRelevantFund.AddRange(tempFunds);
                    }
                    else if (relevantInformationType == RelevantInformationType.Industry)
                    {
                        this._industryRelevantFund.AddRange(tempFunds);
                    }
                }

                // 處理相關ETF
                string etfCodeString = ItemUtils.GetFieldValue(child, Template.MarketTrendItem.Fields.ETFCode);

                if (!string.IsNullOrEmpty(etfCodeString))
                {
                    string[] etfCodes = etfCodeString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    var tempETFs = new List<RelevantInformation>();

                    foreach (string etfCode in etfCodes)
                    {
                        // 最多5筆
                        if (tempETFs.Count == 5)
                        {
                            break;
                        }

                        if (this._etfList.Any(c => c.ProductCode.ToLower().Trim() == etfCode.ToLower().Trim()))
                        {
                            var etf = this._marketTrendRepository.CloneRelevantInformation(this._etfList.FirstOrDefault(c => c.ProductCode.ToLower().Trim() == etfCode.ToLower().Trim()));
                            etf.DetailLink = etfLink + "?id=" + etf.ProductCode;
                            etf.Title = marketTrend.Title;

                            etf = this._marketTrendRepository.GetButtonHtml(etf, InvestTypeEnum.ETF);

                            if (!tempETFs.Any(c => c.ProductCode == etf.ProductCode))
                            {
                                tempETFs.Add(etf);
                            }
                        }
                    }

                    // 照六個月績效做預設排序
                    tempETFs = tempETFs.OrderByDescending(c => c.M6Change).ToList();

                    if (relevantInformationType == RelevantInformationType.Stock)
                    {
                        this._stockRelevantETF.AddRange(tempETFs);
                    }
                    else if (relevantInformationType == RelevantInformationType.Bond)
                    {
                        this._bondRelevantETF.AddRange(tempETFs);
                    }
                    else if (relevantInformationType == RelevantInformationType.Industry)
                    {
                        this._industryRelevantETF.AddRange(tempETFs);
                    }
                }

                // 處理相關新聞
                var newsItems = ItemUtils.GetMultiListValueItems(child, Template.MarketTrendItem.Fields.NewsList);

                if (newsItems != null && newsItems.Any())
                {
                    string newsType = ItemUtils.GetFieldValue(newsItems.FirstOrDefault(), Template.DropdownOption.Fields.OptionText);
                    var pageId = MarketNewsRelatedLinkSetting.GetMarketNewsDetailPageItem().ID.ToGuid();

                    marketTrend.News = this._marketTrendRepository.GetNews(pageId, newsType).ToList();

                    for (int i = 0; i < marketTrend.News.Count; i++)
                    {
                        marketTrend.News[i].DetailLink = MarketNewsRelatedLinkSetting.GetMarketNewsDetailUrl() + "?id=" + marketTrend.News[i].ID;
                    }
                }

                // 讓前端顯示第一筆的相對應區塊
                if (result.Count == 0)
                {
                    marketTrend.Checked = "checked";
                    marketTrend.IsActive = "is-active";
                    marketTrend.Display = string.Empty;
                }

                result.Add(marketTrend);
            }

            return result;
        }
    }
}