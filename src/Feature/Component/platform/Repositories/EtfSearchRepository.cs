using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Models.ETF.Tag;
using Foundation.Wealth.Manager;
using Mapster;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class EtfSearchRepository
    {
        public EtfSearchModel GetETFSearchModel(Item dataSource)
        {
            EtfSearchModel model = new EtfSearchModel();

            if (dataSource != null)
            {
                model.DatasourceId = dataSource.ID.ToSearchId();
            }

            var result = MapperResult();
            this.SearchResults = result;
            model.SearchResultModel = GetDatasourceData(dataSource);
            model.FilterModel = SetFilterOptions();

            return model;
        }

        public IEnumerable<EtfSearchResult> GetResultList(ReqSearch req)
        {
            var result = MapperResult();
            return result;
        }

        public IList<BasicEtfDto> QueryBasicData()
        {
            string sqlQuery = """
                SELECT *
                FROM [vw_BasicETF]
                ORDER BY [SixMonthReturnMarketPriceOriginalCurrency] DESC, [FirstBankCode] ASC
                """;
            var collection = DbManager.Custom.ExecuteIList<BasicEtfDto>(sqlQuery, null, CommandType.Text);
            return collection;
        }

        public IEnumerable<EtfSearchResult> MapperResult()
        {
            var collection = QueryBasicData();
            var tagList = GetTagSource();

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfSearchResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.ETFName = src.ETFName.Normalize(NormalizationForm.FormKC);
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

                    dest.Tags = tagList?.Where(i => i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode)).Select(i => i.TagKey).ToArray();
                    dest.DiscountTags = tagList?.Where(i => i.TagType == TagType.Discount && i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode))
                                                .Select(i => i.TagKey).ToArray();
                    dest.CategoryTags = tagList?.Where(i => i.TagType == TagType.Category && i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode))
                                                .Select(i => i.TagKey).ToArray();
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
                HotKeywordList = this.HotKeywordList.Select(i => i.TagKey),
                HotProductList = this.HotProductList.Select(i => i.TagKey),
                PricingCurrencyList = this.SearchResults.OrderBy(i => i.CurrencyPair.Key).Select(i => i.CurrencyPair.Value).Distinct(),
                InvestmentTargetList = this.SearchResults.OrderBy(i => i.InvestmentTarget.Key).Select(i => i.InvestmentTarget.Value).Distinct(),
                InvestmentRegionList = this.SearchResults.OrderBy(i => i.InvestmentRegion.Key).Select(i => i.InvestmentRegion.Value).Distinct(),
                InvestmentStyleList = this.SearchResults.OrderBy(i => i.InvestmentStyle.Key).Select(i => i.InvestmentStyle.Value).Distinct(),
                PublicLimitedCompanyList = this.SearchResults.OrderBy(i => i.PublicLimitedCompany.Key).Select(i => i.PublicLimitedCompany.Value).Distinct(),
                DividendDistributionFrequencyList = this.SearchResults.OrderBy(i => i.DividendDistributionFrequency).Select(i => i.DividendDistributionFrequency).Distinct(),
                ExchangeList = this.SearchResults.Select(i => i.ExchangeID).OrderBy(i => i).Distinct()
            };

            return model;
        }

        private EtfSearchResultModel GetDatasourceData(Item dataSource)
        {
            EtfSearchResultModel model = new EtfSearchResultModel();

            if (dataSource == null)
            {
                return null;
            }

            string detailPageLink = string.Empty;
            Item linkItem = ItemUtils.TargetItem(dataSource, Templates.EtfSearchDatasource.Fields.DetailPageLink);

            if (linkItem != null)
            {
                detailPageLink = ItemUtils.Url(linkItem);
            }

            model.MarketPricePerformanceIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.MarketPricePerformanceIntro);
            model.NetWorthPerformanceIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.NetWorthPerformanceIntro);
            model.MarketPriceRiskIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.MarketPriceRiskIntro);
            model.NetWorthRiskIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.NetWorthRiskIntro);
            model.TransactionStatusIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.TransactionStatusIntro);
            model.InformationIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.InformationIntro);
            model.DetailPageLink = detailPageLink;
            GetTagsForFilterOption(dataSource);

            return model;
        }

        private void GetTagsForFilterOption(Item dataSource)
        {
            if (dataSource == null)
            {
                return;
            }

            var hotKeywords = dataSource.GetMultiListValueItems(Templates.EtfSearchDatasource.Fields.HotKeyword);
            var hotProducts = dataSource.GetMultiListValueItems(Templates.EtfSearchDatasource.Fields.HotProduct);
            this.HotKeywordList = SetTagContent(hotKeywords)?.ToList();
            this.HotProductList = SetTagContent(hotProducts)?.ToList();
        }

        private List<ProductTag> GetTagSource()
        {
            Item tagSource = ItemUtils.GetItem(TagSourceFolder);
            var tags = tagSource.GetDescendants(Templates.TagContent.Id);
            
            if (tags == null || !tags.Any())
            {
                return null;
            }

            var result = SetTagContent(tags);
            return result?.ToList();
        }

        private IEnumerable<ProductTag> SetTagContent(IEnumerable<Item> items)
        {
            foreach (Item tagItem in items)
            {
                string key = tagItem.GetFieldValue(Templates.TagContent.Fields.TagKey);

                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                var typeItem = tagItem.TargetItem(Templates.TagContent.Fields.Type);
                string typeValue = typeItem.GetFieldValue(Templates.DropdownOption.Fields.OptionValue);

                if (!Enum.TryParse(typeValue, out TagType type))
                {
                    continue;
                }

                ProductTag productTag = new ProductTag()
                {
                    TagKey = key,
                    ProductCodes = tagItem.GetMultiLineText(Templates.TagContent.Fields.ProductCode)?.ToList(),
                    TagType = type
                };

                yield return productTag;
            }
        }

        #region Method

        private decimal? RoundingPrice(decimal? number)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, 4, MidpointRounding.AwayFromZero);
        }

        private decimal? RoundingPercentage(decimal? number)
        {
            if (!number.HasValue)
            {
                return null;
            }

            return Math.Round(number.Value, 2, MidpointRounding.AwayFromZero);
        }

        private bool IsUpPercentage(decimal? number)
        {
            bool isIncreased = true;
            if (number.HasValue && number < 0)
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

        private KeyValuePair<bool, decimal?> ParsePercentageChangeToKeyValue(decimal? value)
        {
            var keyValuePair = new KeyValuePair<bool, decimal?> ( IsUpPercentage(value), RoundingPercentage(value) );
            return keyValuePair;
        }

        private KeyValuePair<decimal?, string> ParseTradingVolumeToKeyValue(decimal? value)
        {
            var keyValuePair = new KeyValuePair<decimal?, string>(value, NumberExtensions.FormatNumber(value));
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

        #region Property

        public static readonly ID TagSourceFolder = new ID("{A83FD682-0D9C-43D0-BE60-261C8E557690}");

        private IEnumerable<EtfSearchResult> SearchResults { get; set; }

        /// <summary>
        /// 熱門關鍵字
        /// </summary>
        public List<ProductTag> HotKeywordList { get; set; }

        /// <summary>
        /// 熱門主題
        /// </summary>
        public List<ProductTag> HotProductList { get; set; }

        #endregion Property
    }
}