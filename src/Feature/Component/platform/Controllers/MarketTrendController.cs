using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Component.Models.MarketTrend;
using Feature.Wealth.Component.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Controllers
{
    public class MarketTrendController : Controller
    {
        private readonly GlobalIndexRepository _globalIndexRepository = new GlobalIndexRepository();
        private readonly MarketTrendRepository _marketTrendRepository = new MarketTrendRepository();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();
        private readonly ViewCountRepository _viewCountrepository = new ViewCountRepository();
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

        private string _rootPath;

        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;

            _rootPath = ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

            var model = CreateModel(item);

            return View("/Views/Feature/Wealth/Component/MarketTrend/MarketTrend.cshtml", model);
        }

        protected MarketTrendModel CreateModel(Item item)
        {
            var model = new MarketTrendModel
            {
                Item = item,
                IndexLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.IndexLink)?.Url,
                FundLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.FundLink)?.Url,
                ETFLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.ETFLink)?.Url,
                NewsLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.NewsLink)?.Url,
                MoreNewsButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreNewsButtonText),
                MoreNewsButtonLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.MoreNewsButtonLink)?.Url,
                MoreETFButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreETFButtonText),
                MoreETFButtonLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.MoreETFButtonLink)?.Url,
                MoreFundButtonText = ItemUtils.GetFieldValue(item, Template.MarketTrend.Fields.MoreFundButtonText),
                MoreFundButtonLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.MoreFundButtonLink)?.Url,
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

            model.GlobalIndexHighchartsDataHtmlString = new HtmlString(JsonSerializer.Serialize(this._datas));

            model.StockRelevantFundHtmlString = new HtmlString(JsonSerializer.Serialize(this._stockRelevantFund));
            model.StockRelevantETFHtmlString = new HtmlString(JsonSerializer.Serialize(this._stockRelevantETF));
            model.BondRelevantFundHtmlString = new HtmlString(JsonSerializer.Serialize(this._bondRelevantFund));
            model.BondRelevantETFHtmlString = new HtmlString(JsonSerializer.Serialize(this._bondRelevantETF));
            model.IndustryRelevantFundHtmlString = new HtmlString(JsonSerializer.Serialize(this._industryRelevantFund));
            model.IndustryRelevantETFHtmlString = new HtmlString(JsonSerializer.Serialize(this._industryRelevantETF));

            return model;
        }

        private List<MarketTrend> CreateMarketTrendList(Item item, IEnumerable<Item> children, RelevantInformationType relevantInformationType)
        {
            var result = new List<MarketTrend>();

            string indexLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.IndexLink)?.Url;
            string fundLink = Models.FundDetail.FundRelatedSettingModel.GetFundDetailsUrl();
            string etfLink = ItemUtils.GeneralLink(item, Template.MarketTrend.Fields.ETFLink)?.Url;
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
                    var index = this._globalIndexList.First(c => c.IndexCode == indexCode);

                    if (index != null)
                    {
                        if (string.IsNullOrEmpty(index.DetailLink))
                        {
                            index.DetailLink = indexLink + "?id=" + index.IndexCode;
                        }

                        if (index.GlobalIndexHistory == null)
                        {
                            index.GlobalIndexHistory = this._globalIndexRepository.GetGlobalIndexHistoryList(index.IndexCode).OrderBy(c => c.DataDate).ToList();
                        }

                        temp.Add(index);

                        if (!this._datas.Any(c => c.IndexCode == index.IndexCode))
                        {
                            this._datas.Add(new Models.GlobalIndex.GlobalIndexHighchartsData
                            {
                                IndexCode = index.IndexCode,
                                Data = index.GlobalIndexHistory.Select(c => float.Parse(c.MarketPrice)).ToList()
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

                    int count = 0;

                    foreach (string fundCode in fundCodes)
                    {
                        // 最多5筆
                        if (count == 5)
                        {
                            break;
                        }

                        var fund = JsonSerializer.Deserialize<RelevantInformation>(JsonSerializer.Serialize(this._fundList.FirstOrDefault(c => c.ProductCode == fundCode)));
                        fund.DetailLink = fundLink + "?id=" + fund.ProductCode;
                        fund.Title = marketTrend.Title;

                        // TODO 取得關注

                        // TODO 取得比較

                        if (relevantInformationType == RelevantInformationType.Stock)
                        {
                            this._stockRelevantFund.Add(fund);
                        }
                        else if (relevantInformationType == RelevantInformationType.Bond)
                        {
                            this._bondRelevantFund.Add(fund);
                        }
                        else if (relevantInformationType == RelevantInformationType.Industry)
                        {
                            this._industryRelevantFund.Add(fund);
                        }

                        count++;
                    }
                }

                // 處理相關ETF
                string etfCodeString = ItemUtils.GetFieldValue(child, Template.MarketTrendItem.Fields.ETFCode);

                if (!string.IsNullOrEmpty(etfCodeString))
                {
                    string[] etfCodes = etfCodeString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    int count = 0;

                    foreach (string etfCode in etfCodes)
                    {
                        // 最多5筆
                        if (count == 5)
                        {
                            break;
                        }

                        var etf = JsonSerializer.Deserialize<RelevantInformation>(JsonSerializer.Serialize(this._etfList.FirstOrDefault(c => c.ProductCode == etfCode)));
                        etf.DetailLink = etfLink + "?id=" + etf.ProductCode;
                        etf.Title = marketTrend.Title;

                        // TODO 取得關注

                        // TODO 取得比較

                        if (relevantInformationType == RelevantInformationType.Stock)
                        {
                            this._stockRelevantETF.Add(etf);
                        }
                        else if (relevantInformationType == RelevantInformationType.Bond)
                        {
                            this._bondRelevantETF.Add(etf);
                        }
                        else if (relevantInformationType == RelevantInformationType.Industry)
                        {
                            this._industryRelevantETF.Add(etf);
                        }

                        count++;
                    }
                }

                // 處理相關新聞
                var newsItems = ItemUtils.GetMultiListValueItems(child, Template.MarketTrendItem.Fields.NewsList);

                if (newsItems != null && newsItems.Any())
                {
                    string newsType = ItemUtils.GetFieldValue(newsItems.FirstOrDefault(), Template.DropdownOption.Fields.OptionValue);

                    var resp = _djMoneyApiRespository.GetNewsForMarketTrend(newsType);

                    if (resp != null
                        && resp.ContainsKey("resultSet")
                        && resp["resultSet"] != null
                        && resp["resultSet"]["result"] != null
                        && resp["resultSet"]["result"].Any())
                    {
                        var datas = resp["resultSet"]["result"];

                        foreach (var data in datas)
                        {
                            var news = new News()
                            {
                                Date = data["v1"].ToString(),
                                Time = data["v2"].ToString(),
                                Title = data["v3"].ToString(),
                                ID = data["v4"].ToString(),
                                DetailLink = newsLink + "?id=" + data["v4"].ToString(),
                            };

                            // 取得觀看數
                            news.ViewCount = _viewCountrepository.GetViewCountInfo(newsLinkItem.ID.ToString(), _rootPath + news.DetailLink);

                            // TODO 取得收藏

                            marketTrend.News.Add(news);
                        }
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