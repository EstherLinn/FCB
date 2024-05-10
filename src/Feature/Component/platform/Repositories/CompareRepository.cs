using Feature.Wealth.Component.Models.Compare;
using Feature.Wealth.Component.Models.GlobalIndex;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;

namespace Feature.Wealth.Component.Repositories
{
    public class CompareRepository
    {
        /// <summary>
        /// 全球指數供 - 比較頁
        /// </summary>
        /// <returns></returns>
        public IList<GlobalIndex> GetCompareGlobalIndexList()
        {
            string sql = """
                SELECT
                    [IndexCode]
                    ,[IndexName]
                    FROM [Sysjust_GlobalIndex] WITH (NOLOCK)
                    WHERE [IndexCategoryID] IN (1, 2, 3, 4)
                    ORDER BY LEN([IndexCode]) DESC, [IndexCode]
                """;
            return DbManager.Custom.ExecuteIList<GlobalIndex>(sql, null, CommandType.Text);
        }

        /// <summary>
        /// 取得歷史全球指數走勢圖
        /// </summary>
        /// <param name="indexCode"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public IList<GlobalIndex> GetGlobalIndexReturnChartData(string indexCode, string startdate, string enddate)
        {
            string sql = """
                SELECT REPLACE(CONVERT(char(10), [DataDate],126),'-','/') [DataDate]
                    ,[ChangePercentage]
                    FROM [Sysjust_GlobalIndex_History] WITH (NOLOCK)                             
                    WHERE IndexCode = @IndexCode
                    AND [DataDate] BETWEEN @StartDate AND @EndDate
                    ORDER BY [DataDate]
                """;
            var param = new { IndexCode = indexCode, StartDate = startdate, EndDate = enddate };
            return DbManager.Custom.ExecuteIList<GlobalIndex>(sql, param, CommandType.Text);
        }

        /// <summary>
        /// 取得三個月內最高與最低淨值
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        public CompareBase GetCompareETFNetAssetValueThreeMonths(string etfId)
        {
            string sql = """
                SELECT top 1 [FirstBankCode], MAX(NetAssetValue) AS MaxNetAssetValueThreeMonths, MIN(NetAssetValue) AS MinNetAssetValueThreeMonths FROM [dbo].[Sysjust_Nav_ETF]
                WHERE　[FirstBankCode] = @ETFId AND [NetAssetValueDate] >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY [FirstBankCode]
                """;
            var param = new { ETFId = etfId };
            return DbManager.Custom.Execute<CompareBase>(sql, param, CommandType.Text);
        }
    }
}