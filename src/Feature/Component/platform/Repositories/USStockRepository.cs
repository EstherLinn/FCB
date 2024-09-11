using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.USStock.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class USStockRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public IList<USStock> GetUSStockList()
        {
            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string sql = $@"SELECT 
                           A.[FirstBankCode]
                           ,A.[FundCode]
                           ,A.[EnglishName]
                           ,A.[ChineseName]
                           ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [FullName]
                           ,A.[FirstBankCode] + ' ' + A.[ChineseName] + ' ' + A.[EnglishName] [value]
                           ,REPLACE(CONVERT(char(10), A.[DataDate],126),'-','/') [DataDate]
                           ,A.[ClosingPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, A.[ClosingPrice]), 1) [ClosingPriceText]
                           ,CONVERT(decimal(16,2), A.[DailyReturn]) [DailyReturn]
                           ,CONVERT(decimal(16,2), A.[WeeklyReturn]) [WeeklyReturn]
                           ,CONVERT(decimal(16,2), A.[MonthlyReturn]) [MonthlyReturn]
                           ,CONVERT(decimal(16,2), A.[ThreeMonthReturn]) [ThreeMonthReturn]
                           ,CONVERT(decimal(16,2), A.[OneYearReturn]) [OneYearReturn]
                           ,CONVERT(decimal(16,2), A.[YeartoDateReturn]) [YeartoDateReturn]
                           ,CONVERT(decimal(16,2), A.[ChangePercentage]) [ChangePercentage]
                           ,CONVERT(decimal(16,2), A.[SixMonthReturn]) [SixMonthReturn]
                           ,B.[OnlineSubscriptionAvailability]
                           ,B.[AvailabilityStatus]
                           FROM {Sysjust_USStockList} A WITH (NOLOCK)
                           LEFT JOIN {WMS_DOC_RECM} B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]
                           ORDER BY A.[MonthlyReturn] DESC, A.[FirstBankCode] ASC";

            var uSStocks = this._dbConnection.Query<USStock>(sql)?.ToList() ?? new List<USStock>();

            return uSStocks;
        }

        public USStock GetUSStock(string firstBankCode)
        {
            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string sql = $@"SELECT 
                           A.[FirstBankCode]
                           ,[FundCode]
                           ,[EnglishName]
                           ,[ChineseName]
                           ,A.[FirstBankCode] + ' ' + [ChineseName] + ' ' + [EnglishName] [FullName]
                           ,REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                           ,[ClosingPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [ClosingPrice]), 1) [ClosingPriceText]
                           ,CONVERT(decimal(16,2), [DailyReturn]) [DailyReturn]
                           ,CONVERT(decimal(16,2), [WeeklyReturn]) [WeeklyReturn]
                           ,CONVERT(decimal(16,2), [MonthlyReturn]) [MonthlyReturn]
                           ,CONVERT(decimal(16,2), [ThreeMonthReturn]) [ThreeMonthReturn]
                           ,CONVERT(decimal(16,2), [OneYearReturn]) [OneYearReturn]
                           ,CONVERT(decimal(16,2), [YeartoDateReturn]) [YeartoDateReturn]
                           ,CONVERT(decimal(16,2), [ChangePercentage]) [ChangePercentage]
                           ,CONVERT(decimal(16,2), [SixMonthReturn]) [SixMonthReturn]
                           ,B.[OnlineSubscriptionAvailability]
                           ,B.[AvailabilityStatus]
                           FROM {Sysjust_USStockList} A WITH (NOLOCK)
                           LEFT JOIN {WMS_DOC_RECM} B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]
                           WHERE A.[FirstBankCode] = @FirstBankCode";

            var uSStock = this._dbConnection.Query<USStock>(sql, new { FirstBankCode = firstBankCode })?.FirstOrDefault() ?? new USStock();

            return uSStock;
        }

        internal USStock GetButtonHtml(USStock item, bool isListButton)
        {
            item.FocusButton = PublicHelpers.FocusButton(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks, isListButton);
            item.FocusButtonHtml = item.FocusButton.ToString();
            item.SubscribeButton = PublicHelpers.SubscriptionButton(null, null, item.FirstBankCode, InvestTypeEnum.ForeignStocks, isListButton);
            item.SubscribeButtonHtml = item.SubscribeButton.ToString();
            item.AutoFocusButtonHtml = PublicHelpers.FocusTag(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks).ToString();
            item.AutoSubscribeButtonHtml = PublicHelpers.SubscriptionTag(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks).ToString();

            return item;
        }

        private List<Item> _hotKeywordTags = new List<Item>();
        private List<Item> _hotProductTags = new List<Item>();
        private List<Item> _discounts = new List<Item>();

        internal USStock SetTags(USStock uSStock)
        {
            if (!this._hotKeywordTags.Any() || !this._hotProductTags.Any() || !this._discounts.Any())
            {
                var tagFolder = ItemUtils.GetContentItem(Templates.USStockTagFolder.Id);
                var children = ItemUtils.GetChildren(tagFolder, Templates.TagFolder.Id);

                if (children != null && children.Any())
                {
                    foreach (var child in children)
                    {
                        var tagsType = ItemUtils.GetFieldValue(child, Templates.TagFolder.Fields.TagType);

                        switch (tagsType)
                        {
                            case "HotKeywordTag":
                                this._hotKeywordTags.AddRange(ItemUtils.GetChildren(child, Templates.USStockTag.Id));
                                break;
                            case "HotProductTag":
                                this._hotProductTags.AddRange(ItemUtils.GetChildren(child, Templates.USStockTag.Id));
                                break;
                            case "Discount":
                                this._discounts.AddRange(ItemUtils.GetChildren(child, Templates.USStockTag.Id));
                                break;
                        }
                    }
                }
            }

            foreach (var f in this._hotKeywordTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode) && !uSStock.HotKeywordTags.Contains(tagName))
                {
                    uSStock.HotKeywordTags.Add(tagName);
                }
            }

            foreach (var f in this._hotProductTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode) && !uSStock.HotProductTags.Contains(tagName))
                {
                    uSStock.HotProductTags.Add(tagName);
                }
            }

            foreach (var f in this._discounts)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.USStockTag.Fields.ProductCodeList);

                if (productCodeList.Contains(uSStock.FirstBankCode) && !uSStock.Discount.Contains(tagName))
                {
                    uSStock.Discount.Add(tagName);
                }
            }

            return uSStock;
        }
    }
}