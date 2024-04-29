using System.Data;
using System.Text;
using System.Collections.Generic;
using Foundation.Wealth.Manager;
using static Feature.Wealth.Component.Models.CostRank.CostRankModel;

namespace Feature.Wealth.Component.Repositories
{
    public class CostRankRepository
    {
        public IList<ETFs> GetFundData()
        {
            List<ETFs> ETFItems = new List<ETFs>();
            string sql = """
                   SELECT Top 10 *
                   FROM [vw_BasicETF] 
                   ORDER BY TotalManagementFee,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);
            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                ETFItems.Add(item);
            }
            return ETFItems;

        }
        private void ProcessFundFilterDatas(ETFs item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.TotalManagementFee = item.TotalManagementFee * 100;
            item.MarketPrice = decimal.Round(item.MarketPrice, 4);
        }
    }
}
