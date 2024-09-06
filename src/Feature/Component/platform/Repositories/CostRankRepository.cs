using System.Data;
using System.Text;
using System.Collections.Generic;
using Foundation.Wealth.Manager;
using static Feature.Wealth.Component.Models.CostRank.CostRankModel;
using Feature.Wealth.Component.Models.ETF.Tag;
using System.Linq;
using System;

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
                   WHERE LEFT(ProductCode, 2) NOT IN ('EA', 'EB')
                   ORDER BY TotalManagementFee,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            EtfTagRepository tagRepository = new EtfTagRepository();
            var dicTag = tagRepository.GetTagCollection();

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                if (dicTag.ContainsKey(TagType.Discount))
                {
                    var discountTags = dicTag[TagType.Discount]
                       .Where(tag => tag.ProductCodes.Any() && tag.ProductCodes.Contains(item.ProductCode))
                       .Select(tag => tag.TagKey)
                       .ToArray();

                    item.ETFDiscountTags = discountTags;
                }
                ETFItems.Add(item);

            }

            return ETFItems;

        }
        private void ProcessFundFilterDatas(ETFs item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.TotalManagementFee = decimal.Round(item.TotalManagementFee, 2, MidpointRounding.AwayFromZero);
            item.MarketPrice = decimal.Round(item.MarketPrice, 4, MidpointRounding.AwayFromZero);
        }
    }
}
