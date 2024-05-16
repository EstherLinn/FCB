using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Feature.Wealth.Component.Models.NewArrivalETF.NewArrivalEtfModel;

namespace Feature.Wealth.Component.Repositories
{
    public class NewArrivalEtfRepository
    {
        public List<ETFs> GetFundData()
        {
            List<ETFs> fundItems = new List<ETFs>();

            var sql = """
             SELECT *
             FROM [vw_BasicETF]
             WHERE [ListingDate] >= DATEADD(year, -1, GETDATE())
             ORDER BY SixMonthReturnMarketPriceOriginalCurrency
             DESC
             """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                if (item != null) 
                {
                    ProcessFundFilterDatas(item);
                    fundItems.Add(item);
                }
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(ETFs item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.DiscountPremiumFormat = RoundingPrice(item.DiscountPremium);
            item.SixMonthReturnMarketPriceOriginalCurrencyFormat = RoundingPrice2(item.SixMonthReturnMarketPriceOriginalCurrency);
            item.MarketPriceFormat = RoundingPrice(item.MarketPrice);
        }

        private decimal? RoundingPrice(decimal? number)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, 4, MidpointRounding.AwayFromZero);
        }

        private decimal? RoundingPrice2(decimal? number)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, 2, MidpointRounding.AwayFromZero);
        }


    }
}
