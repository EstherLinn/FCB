using Dapper;
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

            var globalIndexs = this._dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

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

            var globalIndexs = this._dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

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

            var globalIndexs = this._dbConnection.Query<GlobalIndex>(sql, new { IndexCode = indexCode })?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }

        public GlobalIndexDetail GetGlobalIndexDetail(string indexCode)
        {
            string sql = @"SELECT
                           A.[IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           ,B.[ViewCount]
                           FROM [Sysjust_GlobalIndex] A WITH (NOLOCK)
                           LEFT JOIN [GlobalIndexViewCount] B WITH (NOLOCK) ON A.[IndexCode] = B.[IndexCode]
                           WHERE A.[IndexCode] = @IndexCode
                           ORDER BY [IndexCode]";

            var globalIndexDetail = this._dbConnection.Query<GlobalIndexDetail>(sql, new { IndexCode = indexCode })?.FirstOrDefault() ?? new GlobalIndexDetail();

            return globalIndexDetail;
        }

        /// <summary>
        /// 暫定 觸發紀錄指數瀏覽sp
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        public bool TriggerViewCountRecord(string indexCode)
        {
            int updateCount = 0;
            var para = new { IndexCode = indexCode };
            updateCount = DbManager.Custom.Execute<int>("sp_GlobalIndexViewCountRecord", para, commandType: System.Data.CommandType.StoredProcedure);
            return updateCount == 1;
        }

        public decimal? Round2(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2);
            }
            return value;
        }

        public double GetDoubleOrZero(string value)
        {
            if (double.TryParse(value, out double reslut))
            {
                return reslut;
            }
            else
            {
                return 0;
            }
        }
    }
}