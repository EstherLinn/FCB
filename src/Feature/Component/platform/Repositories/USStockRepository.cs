using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Component.Repositories
{
    public class USStockRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public IList<USStock> GetUSStockList()
        {
            string sql = @"SELECT 
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
                           FROM [Sysjust_USStockList] A WITH (NOLOCK)
                           LEFT JOIN [WMS_DOC_RECM] B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]";

            var uSStocks = this._dbConnection.Query<USStock>(sql)?.ToList() ?? new List<USStock>();

            return uSStocks;
        }

        public USStock GetUSStock(string firstBankCode)
        {
            string sql = @"SELECT 
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
                           ,C.[ViewCount]
                           FROM [Sysjust_USStockList] A WITH (NOLOCK)
                           LEFT JOIN [WMS_DOC_RECM] B WITH (NOLOCK) ON A.[FirstBankCode] = B.[ProductCode]
                           LEFT JOIN [USStockViewCount] C WITH (NOLOCK) ON A.[FirstBankCode] = C.[FirstBankCode]
                           WHERE A.[FirstBankCode] = @FirstBankCode";

            var uSStock = this._dbConnection.Query<USStock>(sql, new { FirstBankCode = firstBankCode })?.FirstOrDefault() ?? new USStock();

            return uSStock;
        }

        /// <summary>
        /// 暫定 觸發紀錄指數瀏覽sp
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        public bool TriggerViewCountRecord(string firstBankCode)
        {
            int updateCount = 0;
            var para = new { FirstBankCode = firstBankCode };
            updateCount = DbManager.Custom.Execute<int>("sp_USStockViewCountRecord", para, commandType: System.Data.CommandType.StoredProcedure);
            return updateCount == 1;
        }

        internal USStock GetButtonHtml(USStock item, bool isListButton = true)
        {
            item.FocusButton = PublicHelpers.FocusButton(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks, isListButton);
            item.FocusButtonHtml = item.FocusButton.ToString();
            item.SubscribeButton = PublicHelpers.SubscriptionButton(null, null, item.FirstBankCode, InvestTypeEnum.ForeignStocks, isListButton);
            item.SubscribeButtonHtml = item.SubscribeButton.ToString();
            item.AutoFocusButtonHtml = PublicHelpers.FocusTag(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks).ToString();
            item.AutoSubscribeButtonHtml = PublicHelpers.SubscriptionTag(null, null, item.FirstBankCode, item.FullName, InvestTypeEnum.ForeignStocks).ToString();

            return item;
        }
    }
}