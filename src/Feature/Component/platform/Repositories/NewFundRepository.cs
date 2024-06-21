using System.Text;
using System.Data;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Foundation.Wealth.Extensions;
using static Feature.Wealth.Component.Models.NewFund.NewFundModel;
using Sitecore.IO;
using System.Globalization;
using System.Security.Cryptography;
using System;
using System.Linq;
using Feature.Wealth.Component.Models.FundDetail;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Repositories
{
    public class NewFundRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string NewFundCacheKey = $"Fcb_NewFundCache";

        public List<Funds> GetFundData()
        {
            List<Funds> fundItems = new List<Funds>();

            var sql = """
             SELECT *
             FROM [vw_BasicFund]
             ORDER BY SixMonthReturnOriginalCurrency
             DESC,ProductCode
             """;

            var results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);

            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();

            foreach (var item in results)
            {
                if (item != null)
                {
                    ProcessFundFilterDatas(item);
                    item.Tags = [];
                    item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                       where tagModel.ProductCodes.Contains(item.ProductCode)
                                       select tagModel.TagName);
                }
            }

            fundItems.AddRange(results);

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.FundName = item.FundName?.Normalize(NormalizationForm.FormKC);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
            item.SixMonthReturnOriginalCurrency = NumberExtensions.RoundingPercentage(item.SixMonthReturnOriginalCurrency);
            item.PercentageChangeInFundPrice = NumberExtensions.RoundingPercentage((item.PercentageChangeInFundPrice) * 100);

            var cultureInfo = new CultureInfo("zh-TW");
            string dateFormat = "yyyy/MM/dd";
            if (DateTime.TryParseExact(item.ListingDate, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out DateTime listingDate))
            {
                item.ListingDate = listingDate.ToString(dateFormat);
                item.ListingDateFormat = listingDate;
            }
        }

        public List<Funds> GetOrSetNewFundCache()
        {
            var newFundData = (List<Funds>)_cache.Get(NewFundCacheKey) ?? new List<Funds>();

            if (!newFundData.Any())
            {
                newFundData = GetFundData();
                _cache.Set(NewFundCacheKey, newFundData, DateTimeOffset.Now.AddMinutes(60));
            }

            return newFundData;
        }

    }
}
