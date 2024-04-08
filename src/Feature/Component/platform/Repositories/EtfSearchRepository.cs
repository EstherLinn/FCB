using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Search;
using Foundation.Wealth.Manager;
using Mapster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Repositories
{
    public class EtfSearchRepository
    {
        private IEnumerable<EtfSearchResult> SearchResults { get; set; }

        public EtfSearchModel GetETFSearchModel()
        {
            EtfSearchModel model = new EtfSearchModel();
            var result = MapperResult();
            this.SearchResults = result;

            model.SearchResultModel = new EtfSearchResultModel()
            {
                ResultProducts = result?.ToList()
            };

            model.FilterModel = SetFilterOptions();

            return model;
        }

        public IEnumerable<EtfSearchResult> GetResultList()
        {
            var result = MapperResult();
            return result;
        }

        public IEnumerable<EtfSearchResult> MapperResult()
        {
            string sqlQuery = """
                SELECT *
                    FROM [vw_BasicETF]
                """;
            var collection = DbManager.Custom.ExecuteIList<BasicEtfDto>(sqlQuery, null, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfSearchResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.RegionType = IdentifyRegion(src.SysjustCode);
                    dest.CurrencyPair = new KeyValuePair<string, string>(src.CurrencyCode, src.CurrencyName);
                    dest.NetAssetValueDate = DateTimeFormat(src.NetAssetValueDate);
                    dest.MarketPrice = RoundingPrice(src.MarketPrice);
                    dest.NetAssetValue = RoundingPrice(src.NetAssetValue);

                    #region 報酬率

                    #region 報酬率 (市價原幣)

                    dest.InceptionDateMarketPriceOriginalCurrency    = ParsePercentageChangeToKeyValue(src.InceptionDateMarketPriceOriginalCurrency);
                    dest.YeartoDateReturnMarketPriceOriginalCurrency = ParsePercentageChangeToKeyValue(src.YeartoDateReturnMarketPriceOriginalCurrency);
                    dest.MonthlyReturnMarketPriceOriginalCurrency    = ParsePercentageChangeToKeyValue(src.MonthlyReturnMarketPriceOriginalCurrency);
                    dest.ThreeMonthReturnMarketPriceOriginalCurrency = ParsePercentageChangeToKeyValue(src.ThreeMonthReturnMarketPriceOriginalCurrency);
                    dest.SixMonthReturnMarketPriceOriginalCurrency   = ParsePercentageChangeToKeyValue(src.SixMonthReturnMarketPriceOriginalCurrency);
                    dest.OneYearReturnMarketPriceOriginalCurrency    = ParsePercentageChangeToKeyValue(src.OneYearReturnMarketPriceOriginalCurrency);
                    dest.TwoYearReturnMarketPriceOriginalCurrency    = ParsePercentageChangeToKeyValue(src.TwoYearReturnMarketPriceOriginalCurrency);
                    dest.ThreeYearReturnMarketPriceOriginalCurrency  = ParsePercentageChangeToKeyValue(src.ThreeYearReturnMarketPriceOriginalCurrency);

                    #endregion

                    #region 報酬率 (市價台幣)

                    dest.InceptionDateMarketPriceTWD    = ParsePercentageChangeToKeyValue(src.InceptionDateMarketPriceTWD);
                    dest.YeartoDateReturnMarketPriceTWD = ParsePercentageChangeToKeyValue(src.YeartoDateReturnMarketPriceTWD);
                    dest.MonthlyReturnMarketPriceTWD    = ParsePercentageChangeToKeyValue(src.MonthlyReturnMarketPriceTWD);
                    dest.ThreeMonthReturnMarketPriceTWD = ParsePercentageChangeToKeyValue(src.ThreeMonthReturnMarketPriceTWD);
                    dest.SixMonthReturnMarketPriceTWD   = ParsePercentageChangeToKeyValue(src.SixMonthReturnMarketPriceTWD);
                    dest.OneYearReturnMarketPriceTWD    = ParsePercentageChangeToKeyValue(src.OneYearReturnMarketPriceTWD);
                    dest.TwoYearReturnMarketPriceTWD    = ParsePercentageChangeToKeyValue(src.TwoYearReturnMarketPriceTWD);
                    dest.ThreeYearReturnMarketPriceTWD  = ParsePercentageChangeToKeyValue(src.ThreeYearReturnMarketPriceTWD);

                    #endregion

                    #region 報酬率 (淨值原幣)

                    dest.InceptionDateNetValueOriginalCurrency    = ParsePercentageChangeToKeyValue(src.InceptionDateNetValueOriginalCurrency);
                    dest.YeartoDateReturnNetValueOriginalCurrency = ParsePercentageChangeToKeyValue(src.YeartoDateReturnNetValueOriginalCurrency);
                    dest.MonthlyReturnNetValueOriginalCurrency    = ParsePercentageChangeToKeyValue(src.MonthlyReturnNetValueOriginalCurrency);
                    dest.ThreeMonthReturnNetValueOriginalCurrency = ParsePercentageChangeToKeyValue(src.ThreeMonthReturnNetValueOriginalCurrency);
                    dest.SixMonthReturnNetValueOriginalCurrency   = ParsePercentageChangeToKeyValue(src.SixMonthReturnNetValueOriginalCurrency);
                    dest.OneYearReturnNetValueOriginalCurrency    = ParsePercentageChangeToKeyValue(src.OneYearReturnNetValueOriginalCurrency);
                    dest.TwoYearReturnNetValueOriginalCurrency    = ParsePercentageChangeToKeyValue(src.TwoYearReturnNetValueOriginalCurrency);
                    dest.ThreeYearReturnNetValueOriginalCurrency  = ParsePercentageChangeToKeyValue(src.ThreeYearReturnNetValueOriginalCurrency);

                    #endregion

                    #region 報酬率 (淨值台幣)

                    dest.InceptionDateNetValueTWD    = ParsePercentageChangeToKeyValue(src.InceptionDateNetValueTWD);
                    dest.YeartoDateReturnNetValueTWD = ParsePercentageChangeToKeyValue(src.YeartoDateReturnNetValueTWD);
                    dest.MonthlyReturnNetValueTWD    = ParsePercentageChangeToKeyValue(src.MonthlyReturnNetValueTWD);
                    dest.ThreeMonthReturnNetValueTWD = ParsePercentageChangeToKeyValue(src.ThreeMonthReturnNetValueTWD);
                    dest.SixMonthReturnNetValueTWD   = ParsePercentageChangeToKeyValue(src.SixMonthReturnNetValueTWD);
                    dest.OneYearReturnNetValueTWD    = ParsePercentageChangeToKeyValue(src.OneYearReturnNetValueTWD);
                    dest.TwoYearReturnNetValueTWD    = ParsePercentageChangeToKeyValue(src.TwoYearReturnNetValueTWD);
                    dest.ThreeYearReturnNetValueTWD  = ParsePercentageChangeToKeyValue(src.ThreeYearReturnNetValueTWD);

                    #endregion 

                    #endregion 報酬率

                    dest.AnnualizedStandardDeviationMarketPriceRisk = RoundingPercentage(src.AnnualizedStandardDeviationMarketPriceRisk);
                    dest.AnnualizedStandardDeviationNetValueRisk = RoundingPercentage(src.AnnualizedStandardDeviationNetValueRisk);
                    dest.DiscountPremium = ParsePercentageChangeToKeyValue(src.DiscountPremium);
                    dest.LatestVolumeTradingVolume = ParseTradingVolumeToKeyValue(src.LatestVolumeTradingVolume);
                    dest.LatestVolumeTradingVolumeTenDayAverageVolume = ParseTradingVolumeToKeyValue(src.LatestVolumeTradingVolumeTenDayAverageVolume);

                    dest.PublicLimitedCompany = new KeyValuePair<int, string>(src.PublicLimitedCompanyID, src.PublicLimitedCompanyName);
                    dest.InvestmentTarget = new KeyValuePair<int, string>(src.InvestmentTargetID, src.InvestmentTargetName);
                    dest.InvestmentRegion = new KeyValuePair<int, string>(src.InvestmentRegionID, src.InvestmentRegionName);
                    dest.InvestmentStyle = new KeyValuePair<int, string>(src.InvestmentStyleID, src.InvestmentStyleName);
                    dest.TotalManagementFee = RoundingPercentage(src.TotalManagementFee);
                    dest.ScaleMillions = RoundingPrice(src.ScaleMillions);
                    dest.CanOnlineSubscription = src.OnlineSubscriptionAvailability?.ToUpper() == "Y";
                });

            var result = collection.Adapt<IEnumerable<EtfSearchResult>>(config);

            return result;
        }

        private EtfSearchFilterModel SetFilterOptions()
        {
            if (this.SearchResults == null || !this.SearchResults.Any())
            {
                return null;
            }

            var model = new EtfSearchFilterModel
            {
                PricingCurrencyList = this.SearchResults.OrderBy(i => i.CurrencyPair.Key).Select(i => i.CurrencyPair.Value).Distinct(),
                InvestmentTargetList = this.SearchResults.OrderBy(i => i.InvestmentTarget.Key).Select(i => i.InvestmentTarget.Value).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct(),
                InvestmentRegionList = this.SearchResults.OrderBy(i => i.InvestmentRegion.Key).Select(i => i.InvestmentRegion.Value).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct(),
                InvestmentStyleList = this.SearchResults.OrderBy(i => i.InvestmentStyle.Key).Select(i => i.InvestmentStyle.Value).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct(),
                PublicLimitedCompanyList = this.SearchResults.OrderBy(i => i.PublicLimitedCompany.Key).Select(i => i.PublicLimitedCompany.Value).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct(),
                DividendDistributionFrequencyList = this.SearchResults.Select(i => { if (i.DividendDistributionFrequency == "無") { i.DividendDistributionFrequency = "不配息"; } return i.DividendDistributionFrequency; }).Where(i => !string.IsNullOrWhiteSpace(i)).Distinct(),
                ExchangeList = this.SearchResults.Select(i => i.ExchangeID).OrderBy(i => i).Distinct()
            };

            return model;
        }

        #region Method

        private decimal RoundingPrice(decimal value)
        {
            return Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        private decimal RoundingPercentage(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private bool IsUpPercentage(decimal value)
        {
            bool isIncreased = true;
            if (value < 0)
            {
                isIncreased = false;
            }
            return isIncreased;
        }

        private string DateTimeFormat(DateTime? dateTime)
        {
            string format = "yyyy/MM/dd";
            if (dateTime == null)
            {
                return string.Empty;
            }
            return dateTime?.ToString(format);
        }

        private KeyValuePair<bool, decimal> ParsePercentageChangeToKeyValue(decimal value)
        {
            var keyValuePair = new KeyValuePair<bool, decimal> ( IsUpPercentage(value), RoundingPercentage(value) );
            return keyValuePair;
        }

        private KeyValuePair<decimal, string> ParseTradingVolumeToKeyValue(decimal value)
        {
            var keyValuePair = new KeyValuePair<decimal, string>(value, NumberExtensions.FormatNumber(value));
            return keyValuePair;
        }

        /// <summary>
        /// 以嘉實代碼識別國內/境外
        /// </summary>
        /// <param name="code">嘉實代碼</param>
        private string IdentifyRegion(string code)
        {
            RegionType region;

            if (string.IsNullOrWhiteSpace(code))
            {
                region = RegionType.None;
            }
            else if (!string.IsNullOrWhiteSpace(code) && code.EndsWith(".TW"))
            {
                region = RegionType.Domestic;
            }
            else
            {
                region = RegionType.Overseas;
            }

            return EnumUtil.GetEnumDescription(region);
        }

        #endregion
    }
}