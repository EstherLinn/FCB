using System.Text;
using System.Linq;
using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.IndexRecommendation;

namespace Feature.Wealth.Component.Repositories
{
    public class IndexRecommendationRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();

        public IList<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT Top 5 *
                   FROM [vw_BasicFund] 
                   ORDER BY ViewCount
                   DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }
            return fundItems;

        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice * 100));
            if (item.SixMonthReturnOriginalCurrency < 0)
            {
                item.SixMonthReturnOriginalCurrencyFormat = item.SixMonthReturnOriginalCurrency.ToString().Substring(1);
            }
            if (item.PercentageChangeInFundPrice < 0)
            {
                item.PercentageChangeInFundPriceFormat = item.PercentageChangeInFundPrice.ToString().Substring(1);
            }
        }

        public IList<ETFs> GetETFData()
        {
            List<ETFs> etfsItems = new List<ETFs>();
            string sql = """
                   SELECT *
                   FROM [vw_BasicETF]
                   ORDER BY SixMonthReturnMarketPriceOriginalCurrency
                   DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);

            foreach (var item in results)
            {
                item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
                etfsItems.Add(item);
            }
            return etfsItems;
        }

        public IList<ETFs> GetETFDatas()
        {
            var queryitem = EtfRelatedLinkSetting.GetETFDetailPageItem();
            var query = queryitem.ID.ToGuid();
            var etfData = GetETFData();

            var etfs = _repository.GetVisitRecords(query, "id");

            if (etfs == null || !etfs.Any())
            {
                return new List<ETFs>();
            }

            var etfIds = etfs
                        .OrderByDescending(x => x.VisitCount)
                        .Take(5)
                        .SelectMany(x => x.QueryStrings)
                        .Where(x => x.Key.Equals("id"))
                        .Select(x => x.Value)
                        .ToList();


            var results = etfData
            .Where(e => etfIds.Contains(e.ProductCode))
            .OrderBy(e => etfIds.IndexOf(e.ProductCode.ToString()))
            .ToList();

            EtfTagRepository tagRepository = new EtfTagRepository();
            var dicTag = tagRepository.GetTagCollection();

            foreach (var item in results)
            {
                if (dicTag.ContainsKey(TagType.Discount))
                {
                    var discountTags = dicTag[TagType.Discount]
                        .Where(tag => tag.ProductCodes.Contains(item.ProductCode.ToString()))
                        .Select(tag => tag.TagKey)
                        .ToArray();

                    item.ETFDiscountTags = discountTags;
                }
            }

            return results;
        }

    }
}
