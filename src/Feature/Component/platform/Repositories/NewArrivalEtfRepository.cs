using Feature.Wealth.Component.Models.ETF.Tag;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
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
             WHERE LEFT(ProductCode, 2) NOT IN ('EA', 'EB')
             ORDER BY SixMonthReturnMarketPriceOriginalCurrency
             DESC,ProductCode
             """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            EtfTagRepository tagRepository = new EtfTagRepository();
            var dicTag = tagRepository.GetTagCollection();

            foreach (var item in results)
            {
                if (item != null) 
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
