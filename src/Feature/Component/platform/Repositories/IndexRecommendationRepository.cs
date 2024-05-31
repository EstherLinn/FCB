using System.Text;
using System.Linq;
using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.IndexRecommendation;
using Sitecore.IO;
using System.Security.Cryptography;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Repositories
{
    public class IndexRecommendationRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();

        public IList<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            string sql = """
                   SELECT *
                   FROM [vw_BasicFund] 
                   ORDER BY ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();
            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                item.Tags = [];
                item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                   where tagModel.ProductCodes.Contains(item.ProductCode)
                                   select tagModel.TagName);
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

        public IList<Funds> GetFundsDatas()
        {
            var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
            var query = queryitem.ID.ToGuid();
            var etfData = GetFundData();

            var funds = _repository.GetVisitRecords(query, "id");

            if (funds == null || !funds.Any())
            {
                return new List<Funds>();
            }

            var fundIds = funds
                        .OrderByDescending(x => x.VisitCount)
                        .Take(5)
                        .SelectMany(x => x.QueryStrings)
                        .Where(x => x.Key.Equals("id"))
                        .Select(x => x.Value)
                        .ToList();


            var results = etfData
            .Where(e => fundIds.Contains(e.ProductCode))
            .OrderBy(e => fundIds.IndexOf(e.ProductCode.ToString()))
            .ToList();

            return results;
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
                        .Where(tag => tag.ProductCodes.Any() && tag.ProductCodes.Contains(item.ProductCode))
                        .Select(tag => tag.TagKey)
                        .ToArray();

                    item.ETFDiscountTags = discountTags;
                }
            }

            return results;
        }

    }
}
