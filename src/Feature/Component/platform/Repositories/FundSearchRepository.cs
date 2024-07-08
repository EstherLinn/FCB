using System;
using System.Data;
using System.Text;
using System.Linq;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Feature.Wealth.Component.Models.Invest;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.FundSearch;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Repositories
{
    public class FundSearchRepository
    {
        public IList<FundSearchModel> GetFundSearchData()
        {
            string sql = """
                SELECT * FROM [vw_BasicFund]
                ORDER BY SixMonthReturnOriginalCurrency DESC,ProductCode
                """;
            var results = DbManager.Custom.ExecuteIList<FundSearchModel>(sql, null, CommandType.Text);
            return results;
        }

        /// <summary>
        /// 取得資料-列表渲染用
        /// </summary>
        public List<Funds> GetFundRenderData(IList<FundSearchModel> funds)
        {
            var _tagsRepository = new FundTagRepository();
            var tags = _tagsRepository.GetFundTagData();

            var result = new List<Funds>();

            foreach (var f in funds)
            {
                var vm = new Funds();
                vm.Tags = [];
                vm.HotProductsTags = [];

                // 共用欄位
                if (f.TargetName == "Y")
                {
                    vm.HotProductsTags.Add("百元基金");
                }

                vm.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                 where tagModel.ProductCodes.Contains(f.ProductCode)
                                 select tagModel.TagName);

                vm.HotKeyWordTags = [];
                vm.HotKeyWordTags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.KeywordTag)
                                           where tagModel.ProductCodes.Contains(f.ProductCode)
                                           select tagModel.TagName);

                vm.HotProductsTags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.SortTag)
                                            where tagModel.ProductCodes.Contains(f.ProductCode)
                                            select tagModel.TagName);

                vm.DomesticForeignFundIndicator = f.DomesticForeignFundIndicator;
                vm.ProductCode = f.ProductCode;
                vm.FundName = FullWidthToHalfWidth(f.FundName);
                vm.NetAssetValue = Round4(f.NetAssetValue);
                vm.NetAssetValueDate = f.NetAssetValueDateFormat;

                vm.IsOnlineSubscriptionAvailability = f.AvailabilityStatus == "Y" &&
                                  (f.OnlineSubscriptionAvailability == "Y" ||
                                   string.IsNullOrEmpty(f.OnlineSubscriptionAvailability));


                //績效表現
                if (f.CurrencyCode == null || f.FundCurrency == "TWD")
                {
                    f.FundCurrencyCode = "00";
                    f.FundCurrencyName = "新臺幣";
                }
                vm.Currency = new KeyValuePair<string, string>(f.CurrencyCode, f.CurrencyName);
                vm.SixMonthReturnOriginalCurrency = CreateReturnDictionary(f.SixMonthReturnOriginalCurrency);
                vm.OneMonthReturnOriginalCurrency = CreateReturnDictionary(f.OneMonthReturnOriginalCurrency);
                vm.ThreeMonthReturnOriginalCurrency = CreateReturnDictionary(f.ThreeMonthReturnOriginalCurrency);
                vm.OneYearReturnOriginalCurrency = CreateReturnDictionary(f.OneYearReturnOriginalCurrency);
                vm.SixMonthReturnTWD = CreateReturnDictionary(f.SixMonthReturnTWD);
                vm.OneMonthReturnTWD = CreateReturnDictionary(f.OneMonthReturnTWD);
                vm.ThreeMonthReturnTWD = CreateReturnDictionary(f.ThreeMonthReturnTWD);
                vm.OneYearReturnTWD = CreateReturnDictionary(f.OneYearReturnTWD);
                //基本資料
                vm.FundCurrency = new KeyValuePair<string, string>(f.FundCurrencyCode, f.FundCurrencyName);
                vm.PercentageChangeInFundPrice = Percentage(f.PercentageChangeInFundPrice);
                vm.FundSizeMillionOriginalCurrency = Round4(f.FundSizeMillionOriginalCurrency);
                vm.FundSizeMillionTWD = Round4(f.FundSizeMillionTWD);
                vm.FundType = f.FormatFundType;

                if (f.DividendDistributionFrequency == "無")
                {
                    vm.DividendFrequencyName = "不配息";
                }
                else
                {
                    vm.DividendFrequencyName = f.DividendDistributionFrequency;
                }

                //風險指標
                vm.RiskRewardLevel = f.RiskRewardLevel;
                vm.Sharpe = Round4(f.Sharpe);
                vm.Beta = Round4(f.Beta);
                vm.OneYearAlpha = Round4(f.OneYearAlpha);
                vm.AnnualizedStandardDeviation = Round4(f.AnnualizedStandardDeviation);
                vm.DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl();

                //篩選用
                vm.FundCompanyName = f.FundCompanyName;
                if (!string.IsNullOrEmpty(f.InvestmentRegionName))
                {
                    vm.InvestmentRegionName = f.InvestmentRegionName.Split(',')
                        .Select(region => region.Trim())
                        .ToList();
                }
                else
                {
                    vm.InvestmentRegionName = [null];
                }

                vm.value = f.ProductCode + " " + FullWidthToHalfWidth(f.FundName);

                vm.Data = new FundData
                {
                    DetailUrl = FundRelatedSettingModel.GetFundDetailsUrl() + "?id=" + f.ProductCode,
                    Purchase = vm.IsOnlineSubscriptionAvailability,
                    AutoFocusButtonHtml = PublicHelpers.FocusTag(null, null, f.ProductCode, f.FundName, InvestTypeEnum.Fund).ToString(),
                    AutoSubscribeButtonHtml = PublicHelpers.SubscriptionTag(null, null, f.ProductCode, f.FundName, InvestTypeEnum.Fund).ToString()
                };

                vm.InvestmentTargetName = f.InvestmentTargetName ?? string.Empty;
                vm.FundRating = f.FundRating;

                vm.YeartoDateReturnOriginalCurrency = RoundingPrice(f.YeartoDateReturnOriginalCurrency);
                vm.InceptionDateReturnOriginalCurrency = RoundingPrice(f.InceptionDateReturnOriginalCurrency);
                vm.TwoYearReturnOriginalCurrency = RoundingPrice(f.TwoYearReturnOriginalCurrency);
                vm.ThreeYearReturnOriginalCurrency = RoundingPrice(f.ThreeYearReturnOriginalCurrency);

                vm.YeartoDateReturnTWD = RoundingPrice(f.YeartoDateReturnTWD);
                vm.InceptionDateReturnTWD = RoundingPrice(f.InceptionDateReturnTWD);
                vm.TwoYearReturnTWD = RoundingPrice(f.TwoYearReturnTWD);
                vm.ThreeYearReturnTWD = RoundingPrice(f.ThreeYearReturnTWD);

                vm.FocusTag = PublicHelpers.FocusButtonString(null, f.ProductCode, f.FundName, InvestTypeEnum.Fund, true);
                vm.CompareTag = PublicHelpers.CompareButtonString(null, f.ProductCode, f.FundName, InvestTypeEnum.Fund, true);
                vm.SubscriptionTag = PublicHelpers.SubscriptionButtonString(null, f.ProductCode, InvestTypeEnum.Fund, true);

                result.Add(vm);
            }
            return result;
        }

        private KeyValuePair<bool, decimal?> CreateReturnDictionary(decimal? value)
        {
            bool isUp = value >= 0;
            if (value == null)
            {
                isUp = true;
            }
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }

        private KeyValuePair<bool, decimal?> Percentage(decimal? value)
        {
            bool isUp = true;
            if (value != null)
            {
                isUp = value >= 0;
                value = decimal.Round((decimal)value * 100, 2);
            }
            return new KeyValuePair<bool, decimal?>(isUp, value);
        }

        private static decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
            }
            return value;
        }

        private decimal? RoundingPrice(decimal? number)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, 4, MidpointRounding.AwayFromZero);
        }

        public static string FullWidthToHalfWidth(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (c >= 0xFF01 && c <= 0xFF5E)
                {
                    sb.Append((char)(c - 0xFEE0));
                }
                else if (c == 0x3000)
                {
                    sb.Append((char)0x20);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
