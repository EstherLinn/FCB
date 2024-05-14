using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.MarketTrend;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
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
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [PercentageChangeInFundPrice]*100) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
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
                           ,[CurrencyCode]
                           ,[CurrencyName]
                           ,CONVERT(decimal(16,2), [PercentageChangeInFundPrice]*100) [Change]
                           ,CONVERT(decimal(16,2), [SixMonthReturnOriginalCurrency]) [M6Change]
                           ,[OnlineSubscriptionAvailability]
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
                           FROM [vw_BasicETF]";

            var result = this._dbConnection.Query<RelevantInformation>(sql) ?? new List<RelevantInformation>();

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
                CurrencyCode = item.CurrencyCode,
                CurrencyName = item.CurrencyName,
                CurrencyLink = item.CurrencyLink,
                Change = item.Change,
                M6Change = item.M6Change,
                DetailLink = item.DetailLink,
                OnlineSubscriptionAvailability = item.OnlineSubscriptionAvailability,
                IsLogin = item.IsLogin,
                IsLike = item.IsLike
            };
        }

        private static decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
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