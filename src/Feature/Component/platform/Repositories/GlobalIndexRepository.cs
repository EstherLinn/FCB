using Dapper;
using Feature.Wealth.Component.Models.FundReturn;
using Feature.Wealth.Component.Models.GlobalIndex;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Component.Repositories
{
    public class GlobalIndexRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();

        public IList<GlobalIndex> GetGlobalIndexList()
        {
            string sql = @"SELECT
                           [IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           FROM [Sysjust_GlobalIndex] WITH (NOLOCK)
                           ORDER BY [IndexCode]";

            var globalIndexs = _dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }

        public IList<GlobalIndex> GetCommonGlobalIndexList()
        {
            string sql = @"SELECT
                           [IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           FROM [Sysjust_GlobalIndex] WITH (NOLOCK)
                           WHERE [IndexCode] IN
                           (
                           'EB09999','AI000040','AI000041','AI000220',
                           'AI000545','AI000030','AI000410','AI000070',
                           'AI000800','AI000010','AI000020','AI000140',
                           'AI000280','AI000360','AI000170','AI000515'
                           )
                           ORDER BY [IndexCode]";

            var globalIndexs = _dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }

        public IList<GlobalIndex> GetGlobalIndexHistoryList(string indexCode)
        {
            // 取近30筆
            string sql = @"SELECT TOP 30
                           [IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           FROM [Sysjust_GlobalIndex_History] WITH (NOLOCK)
                           WHERE IndexCode = @IndexCode
                           ORDER BY [DataDate] DESC";

            var globalIndexs = _dbConnection.Query<GlobalIndex>(sql, new { IndexCode = indexCode })?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }
    }
}