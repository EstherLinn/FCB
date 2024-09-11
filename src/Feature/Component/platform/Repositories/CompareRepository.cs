using Feature.Wealth.Component.Models.Compare;
using Feature.Wealth.Component.Models.GlobalIndex;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
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
            string Sysjust_GlobalIndex = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_GlobalIndex);
            string sql = $@"
                SELECT
                    [IndexCode],
                    [IndexName]
                FROM 
                    {Sysjust_GlobalIndex} WITH (NOLOCK)
                WHERE 
                    [IndexCategoryID] IN (1, 2, 3, 4)
                ORDER BY
                    CASE 
                        WHEN [IndexCode] IN ('EB09999', 'EB18888') THEN 0
                        ELSE 1
                    END,
                    LEN([IndexCode]) DESC, 
                    [IndexCode]
                ";
            return DbManager.Custom.ExecuteIList<GlobalIndex>(sql, null, CommandType.Text);
        }

        /// <summary>
        /// 取得三個月內最高與最低淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public CompareBase GetCompareFundNetAssetValueThreeMonths(string fundId)
        {
            string Sysjust_Nav_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Nav_Fund);
            string sql = $@"
                SELECT top 1 [FirstBankCode], MAX(NetAssetValue) AS MaxNetAssetValueThreeMonths, MIN(NetAssetValue) AS MinNetAssetValueThreeMonths FROM {Sysjust_Nav_Fund} WITH(NOLOCK)
                WHERE[FirstBankCode] = @FundId AND[NetAssetValueDate] >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY[FirstBankCode]
                ";
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
            string Sysjust_Nav_ETF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Nav_ETF);
            string sql = $@"
                SELECT top 1 [FirstBankCode], MAX(NetAssetValue) AS MaxNetAssetValueThreeMonths, MIN(NetAssetValue) AS MinNetAssetValueThreeMonths FROM {Sysjust_Nav_ETF} WITH (NOLOCK)
                WHERE [FirstBankCode] = @ETFId AND [NetAssetValueDate] >= DATEADD(MONTH, -3, GETDATE())
                GROUP BY [FirstBankCode]
                ";
            var param = new { ETFId = etfId };
            return DbManager.Custom.Execute<CompareBase>(sql, param, CommandType.Text);
        }
    }
}