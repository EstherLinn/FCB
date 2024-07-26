using System.Data;
using System.Text;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.DiscountPremium.DiscountPremiumModel;
using Feature.Wealth.Component.Models.ETF.Tag;
using System.Linq;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Helper;
using Feature.Wealth.Component.Models.ETF;

namespace Feature.Wealth.Component.Repositories
{
    public class DiscountPremiumRepository
    {
        public IList<ETFs> GetETFData()
        {
            List<ETFs> ETFItems = new List<ETFs>();
            string sql = """
                   SELECT Top 10 *
                   FROM [vw_BasicETF] 
                   WHERE LEFT(ProductCode, 2) NOT IN ('EA', 'EB')
                   ORDER BY DiscountPremium
                   DESC,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);
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
                ETFItems.Add(item);

            }

            return ETFItems;
        }

        public IList<ETFs> GetETFData2()
        {
            List<ETFs> ETFItems = new List<ETFs>();
            string sql = """
                   SELECT Top 10 *
                   FROM [vw_BasicETF] 
                   WHERE LEFT(ProductCode, 2) NOT IN ('EA', 'EB')
                   ORDER BY DiscountPremium,ProductCode
                   """;

            var results = DbManager.Custom.ExecuteIList<ETFs>(sql, null, CommandType.Text);
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
                ETFItems.Add(item);
            }

            return ETFItems;
        }

        public List<RenderETFs> GetFundRenderData(IList<ETFs> etfs)
        {
            var result = new List<RenderETFs>();

            foreach (var f in etfs)
            {
                var vm = new RenderETFs();
                vm.ETFDiscountTags = f.ETFDiscountTags;

                vm.ProductCode = f.ProductCode;
                vm.ProductName = f.ProductName.Normalize(NormalizationForm.FormKC);
                vm.ExchangeCode = f.ExchangeCode;
                vm.MarketPrice = f.MarketPrice;
                vm.NetAssetValueDate = f.NetAssetValueDateFormat;
                vm.DiscountPremium = CreateDictionary(f.DiscountPremium);

                vm.LatestVolumeTradingVolume = new KeyValuePair<string, decimal?>(f.LatestVolumeTradingVolumeFormat,f.LatestVolumeTradingVolume);
                vm.TenDayAverageVolume = new KeyValuePair<string, decimal?>(f.TenDayAverageVolumeFormat, f.TenDayAverageVolume);

                vm.IsOnlineSubscriptionAvailability = f.AvailabilityStatus == "Y" &&
                                  (f.OnlineSubscriptionAvailability == "Y" ||
                                   string.IsNullOrEmpty(f.OnlineSubscriptionAvailability));

                vm.FocusTag = PublicHelpers.FocusButtonString(null, f.ProductCode, f.ProductName, InvestTypeEnum.ETF, true);
                vm.CompareTag = PublicHelpers.CompareButtonString(null, f.ProductCode, f.ProductName, InvestTypeEnum.ETF, true);
                vm.SubscriptionTag = PublicHelpers.SubscriptionButtonString(null, f.ProductCode, InvestTypeEnum.ETF, true);

                vm.DetailUrl = EtfRelatedLinkSetting.GetETFDetailUrl();
                result.Add(vm);
            }
            return result;
        }

        private KeyValuePair<bool, decimal?> CreateDictionary(decimal? value)
        {
            bool isUp = value >= 0;
            if (value == null)
            {
                isUp = true;
            }
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }
    }
}
