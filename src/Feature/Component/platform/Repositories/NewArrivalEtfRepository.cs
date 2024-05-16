using System;
using System.Data;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.NewArrivalETF.NewArrivalEtfModel;
using System.Globalization;

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
             ORDER BY SixMonthReturnMarketPriceOriginalCurrency
             DESC
             """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                if (item != null) 
                {
                    ProcessFundFilterDatas(item);
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

            var cultureInfo = new CultureInfo("zh-TW");
            string dateFormat = "yyyy/MM/dd";
            if (DateTime.TryParseExact(item.ListingDate, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out DateTime listingDate))
            {
                item.ListingDate = listingDate.ToString(dateFormat);
                item.ListingDateFormat = listingDate;
            }
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
