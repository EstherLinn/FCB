using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.MarketTrend;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Component.Repositories
{
    public class MarketTrendRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public RelevantInformation GetFund(string productCode)
        {
            string sql = @"SELECT
                           [ProductCode]
                           ,[ProductName]
                           ,[NetAssetValue]
                           ,REPLACE(CONVERT(char(10), [NetAssetValueDate],126),'-','/') [NetAssetValueDate]
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [PercentageChangeInFundPrice]*100) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
                           ,[AvailabilityStatus]
                           FROM [vw_BasicFund]
                           WHERE [ProductCode] = @ProductCode";

            var result = this._dbConnection.Query<RelevantInformation>(sql, new { ProductCode = productCode })?.FirstOrDefault() ?? new RelevantInformation();

            return result;
        }

        public IEnumerable<RelevantInformation> GetFundList()
        {
            string sql = @"SELECT
                           [ProductCode]
                           ,[ProductName]
                           ,[NetAssetValue]
                           ,REPLACE(CONVERT(char(10), [NetAssetValueDate],126),'-','/') [NetAssetValueDate]
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [PercentageChangeInFundPrice]*100) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
                           ,[AvailabilityStatus]
                           FROM [vw_BasicFund]";

            var result = this._dbConnection.Query<RelevantInformation>(sql) ?? new List<RelevantInformation>();

            return result;
        }

        public RelevantInformation GetETF(string productCode)
        {
            string sql = @"SELECT
                           [ProductCode]
                           ,[ProductName]
                           ,[NetAssetValue]
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [NetAssetValueChangePercentage]) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnNetValueOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
                           ,[AvailabilityStatus]
                           FROM [vw_BasicETF]
                           WHERE [ProductCode] = @ProductCode";

            var result = this._dbConnection.Query<RelevantInformation>(sql, new { ProductCode = productCode })?.FirstOrDefault() ?? new RelevantInformation();

            return result;
        }

        public IEnumerable<RelevantInformation> GetETFList()
        {
            string sql = @"SELECT
                           [ProductCode]
                           ,[ProductName]
                           ,[NetAssetValue]
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [NetAssetValueChangePercentage]) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnNetValueOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
                           ,[AvailabilityStatus]
                           FROM [vw_BasicETF]";

            var result = this._dbConnection.Query<RelevantInformation>(sql) ?? new List<RelevantInformation>();

            return result;
        }

        public IEnumerable<News> GetNews(Guid pageId, string newsType)
        {
            string sql = @"SELECT TOP 5 
                           [NewsSerialNumber] [ID]
                           ,SUBSTRING([NewsDetailDate], 0, 11) [Date]
                           ,SUBSTRING([NewsDetailDate], 11, 6) [Time]
                           ,[NewsTitle] [Title]
                           ,IIF(B.VisitCount IS NULL, 0, B.VisitCount) [ViewCount]
                           FROM [NewsDetail] A WITH (NOLOCK)
                           LEFT JOIN [VisitCount] B WITH (NOLOCK) ON A.NewsSerialNumber = REPLACE(REPLACE(REPLACE(B.QueryStrings, 'id=', ''), '%7b', '{'), '%7d', '}') AND B.PageId = @PageId
                           WHERE A.[NewsType] LIKE @NewsType
                           ORDER BY [NewsDetailDate] DESC";

            var result = this._dbConnection.Query<News>(sql, new { PageId = pageId, NewsType = "%" + newsType + "%" }) ?? new List<News>();

            return result;
        }

        public RelevantInformation CloneRelevantInformation(RelevantInformation item)
        {
            return new RelevantInformation()
            {
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Title = item.Title,
                NetAssetValue = Round4(item.NetAssetValue),
                NetAssetValueDate = item.NetAssetValueDate,
                CurrencyCode = item.CurrencyCode,
                CurrencyName = item.CurrencyName,
                CurrencyLink = item.CurrencyLink,
                Change = item.Change,
                M6Change = item.M6Change,
                DetailLink = item.DetailLink,
                AvailabilityStatus = item.AvailabilityStatus,
                OnlineSubscriptionAvailability = item.OnlineSubscriptionAvailability,
                IsLogin = item.IsLogin,
                IsLike = item.IsLike
            };
        }

        private static decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4, MidpointRounding.AwayFromZero);
            }
            return value;
        }

        internal RelevantInformation GetButtonHtml(RelevantInformation item, InvestTypeEnum investTypeEnum)
        {
            item.CurrencyLinkHtml = PublicHelpers.CurrencyLink(null, null, item.CurrencyName).ToString();
            item.FocusButtonHtml = PublicHelpers.FocusButton(null, null, item.ProductCode, item.ProductName, investTypeEnum, true).ToString();
            item.CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.ProductCode, item.ProductName, investTypeEnum, true).ToString();
            item.SubscribeButtonHtml = PublicHelpers.SubscriptionButton(null, null, item.ProductCode, investTypeEnum, true).ToString();

            return item;
        }
    }
}