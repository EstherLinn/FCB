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
                    [IndexCode],
                    [IndexName]
                FROM 
                    [Sysjust_GlobalIndex] WITH (NOLOCK)
                WHERE 
                    [IndexCategoryID] IN (1, 2, 3, 4)
                ORDER BY
                    CASE 
                        WHEN [IndexCode] IN ('EB09999', 'EB18888') THEN 0
                        ELSE 1
                    END,
                    LEN([IndexCode]) DESC, 
                    [IndexCode]
                """;
            return DbManager.Custom.ExecuteIList<GlobalIndex>(sql, null, CommandType.Text);
        }

        /// <summary>
        /// 取得三個月內最高與最低淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public CompareBase GetCompareFundNetAssetValueThreeMonths(string fundId)
        {
            string sql = """
                SELECT top 1 [FirstBankCode], MAX(NetAssetValue) AS MaxNetAssetValueThreeMonths, MIN(NetAssetValue) AS MinNetAssetValueThreeMonths FROM [dbo].[Sysjust_Nav_Fund]
                WHERE [FirstBankCode] = @FundId AND [NetAssetValueDate] >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY [FirstBankCode]
                """;
            var param = new { FundId = fundId };
            return DbManager.Custom.Execute<CompareBase>(sql, param, CommandType.Text);
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
                WHERE [FirstBankCode] = @ETFId AND [NetAssetValueDate] >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY [FirstBankCode]
                """;
            var param = new { ETFId = etfId };
            return DbManager.Custom.Execute<CompareBase>(sql, param, CommandType.Text);
        }
    }
}