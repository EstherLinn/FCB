using System.Data;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.DiscountPremium.DiscountPremiumModel;

namespace Feature.Wealth.Component.Repositories
{
    public class DiscountPremiumRepository
    {
        public IList<ETFs> GetFundData()
        {
            List<ETFs> ETFItems = new List<ETFs>();
            string sql = """
                   SELECT Top 10 *
                   FROM [vw_BasicETF] 
                   ORDER BY DiscountPremium
                   DESC,ProductCode
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
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC).TrimEnd(' ');
            item.DiscountPremium = decimal.Round(item.DiscountPremium, 4);
            item.MarketPrice = decimal.Round(item.MarketPrice, 4);
        }
    }
}
