using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Models.ETF.Tag;
using Feature.Wealth.Component.Models.Invest;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
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

        public IEnumerable<EtfSearchResult> GetResultList()
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
            var dicTag = this.TagRepository.GetTagCollection();

            Type[] ignoreTypes =
            [
                 typeof(Percentage),
                 typeof(VolumePair),
                 typeof(IdValuePair),
                 typeof(StringPair),
                 typeof(IdPair)
            ];

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfSearchResult>()
                .IgnoreMember((member, side) => ignoreTypes.Contains(member.Type))
                .AfterMapping((src, dest) =>
                {
                    dest.ETFName = src.ETFName?.Normalize(NormalizationForm.FormKC) ?? string.Empty;
                    dest.RegionType = IdentifyRegion(src.SysjustCode);
                    dest.ExchangeCode = new StringPair()
                    {
                        Value = src.ExchangeCode,
                        Text = string.IsNullOrEmpty(src.ExchangeCode) ? "-" : src.ExchangeCode
                    };
                    dest.CurrencyPair = new StringPair()
                    {
                        Key = src.CurrencyCode,
                        Value = src.CurrencyName,
                        Text = string.IsNullOrEmpty(src.CurrencyName) ? "-" : src.CurrencyName
                    };
                    dest.RiskLevel = new StringPair()
                    {
                        Value = src.RiskLevel,
                        Text = string.IsNullOrEmpty(src.RiskLevel) ? "-" : src.RiskLevel
                    };
                    dest.NetAssetValueDate = DateTimeFormat(src.NetAssetValueDate);
                    dest.MarketPriceDate = DateTimeFormat(src.MarketPriceDate);
                    dest.MarketPrice = ParseVolume(src.MarketPrice.RoundingValue());
                    dest.NetAssetValue = ParseVolume(src.NetAssetValue.RoundingValue());

                    #region 報酬率

                    #region 報酬率 (市價原幣)

                    dest.InceptionDateMarketPriceOriginalCurrency    = ParsePercentageChange(src.InceptionDateMarketPriceOriginalCurrency);
                    dest.YeartoDateReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.YeartoDateReturnMarketPriceOriginalCurrency);
                    dest.MonthlyReturnMarketPriceOriginalCurrency    = ParsePercentageChange(src.MonthlyReturnMarketPriceOriginalCurrency);
                    dest.ThreeMonthReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.ThreeMonthReturnMarketPriceOriginalCurrency);
                    dest.SixMonthReturnMarketPriceOriginalCurrency   = ParsePercentageChange(src.SixMonthReturnMarketPriceOriginalCurrency);
                    dest.OneYearReturnMarketPriceOriginalCurrency    = ParsePercentageChange(src.OneYearReturnMarketPriceOriginalCurrency);
                    dest.TwoYearReturnMarketPriceOriginalCurrency    = ParsePercentageChange(src.TwoYearReturnMarketPriceOriginalCurrency);
                    dest.ThreeYearReturnMarketPriceOriginalCurrency  = ParsePercentageChange(src.ThreeYearReturnMarketPriceOriginalCurrency);

                    #endregion

                    #region 報酬率 (市價台幣)

                    dest.InceptionDateMarketPriceTWD    = ParsePercentageChange(src.InceptionDateMarketPriceTWD);
                    dest.YeartoDateReturnMarketPriceTWD = ParsePercentageChange(src.YeartoDateReturnMarketPriceTWD);
                    dest.MonthlyReturnMarketPriceTWD    = ParsePercentageChange(src.MonthlyReturnMarketPriceTWD);
                    dest.ThreeMonthReturnMarketPriceTWD = ParsePercentageChange(src.ThreeMonthReturnMarketPriceTWD);
                    dest.SixMonthReturnMarketPriceTWD   = ParsePercentageChange(src.SixMonthReturnMarketPriceTWD);
                    dest.OneYearReturnMarketPriceTWD    = ParsePercentageChange(src.OneYearReturnMarketPriceTWD);
                    dest.TwoYearReturnMarketPriceTWD    = ParsePercentageChange(src.TwoYearReturnMarketPriceTWD);
                    dest.ThreeYearReturnMarketPriceTWD  = ParsePercentageChange(src.ThreeYearReturnMarketPriceTWD);

                    #endregion

                    #region 報酬率 (淨值原幣)

                    dest.InceptionDateNetValueOriginalCurrency    = ParsePercentageChange(src.InceptionDateNetValueOriginalCurrency);
                    dest.YeartoDateReturnNetValueOriginalCurrency = ParsePercentageChange(src.YeartoDateReturnNetValueOriginalCurrency);
                    dest.MonthlyReturnNetValueOriginalCurrency    = ParsePercentageChange(src.MonthlyReturnNetValueOriginalCurrency);
                    dest.ThreeMonthReturnNetValueOriginalCurrency = ParsePercentageChange(src.ThreeMonthReturnNetValueOriginalCurrency);
                    dest.SixMonthReturnNetValueOriginalCurrency   = ParsePercentageChange(src.SixMonthReturnNetValueOriginalCurrency);
                    dest.OneYearReturnNetValueOriginalCurrency    = ParsePercentageChange(src.OneYearReturnNetValueOriginalCurrency);
                    dest.TwoYearReturnNetValueOriginalCurrency    = ParsePercentageChange(src.TwoYearReturnNetValueOriginalCurrency);
                    dest.ThreeYearReturnNetValueOriginalCurrency  = ParsePercentageChange(src.ThreeYearReturnNetValueOriginalCurrency);

                    #endregion

                    #region 報酬率 (淨值台幣)

                    dest.InceptionDateNetValueTWD    = ParsePercentageChange(src.InceptionDateNetValueTWD);
                    dest.YeartoDateReturnNetValueTWD = ParsePercentageChange(src.YeartoDateReturnNetValueTWD);
                    dest.MonthlyReturnNetValueTWD    = ParsePercentageChange(src.MonthlyReturnNetValueTWD);
                    dest.ThreeMonthReturnNetValueTWD = ParsePercentageChange(src.ThreeMonthReturnNetValueTWD);
                    dest.SixMonthReturnNetValueTWD   = ParsePercentageChange(src.SixMonthReturnNetValueTWD);
                    dest.OneYearReturnNetValueTWD    = ParsePercentageChange(src.OneYearReturnNetValueTWD);
                    dest.TwoYearReturnNetValueTWD    = ParsePercentageChange(src.TwoYearReturnNetValueTWD);
                    dest.ThreeYearReturnNetValueTWD  = ParsePercentageChange(src.ThreeYearReturnNetValueTWD);

                    #endregion 

                    #endregion 報酬率

                    dest.DiscountPremium = ParsePercentageChange(src.DiscountPremium);

                    dest.SharpeNetValueRisk = ParseVolume(src.SharpeNetValueRisk);
                    dest.SharpeRatioMarketPriceRisk = ParseVolume(src.SharpeRatioMarketPriceRisk);
                    dest.BetaNetValueRisk = ParseVolume(src.BetaNetValueRisk);
                    dest.BetaMarketPriceRisk = ParseVolume(src.BetaMarketPriceRisk);
                    dest.AnnualizedStandardDeviationMarketPriceRisk = ParseVolume(src.AnnualizedStandardDeviationMarketPriceRisk);
                    dest.AnnualizedStandardDeviationNetValueRisk = ParseVolume(src.AnnualizedStandardDeviationNetValueRisk);

                    dest.LatestVolumeTradingVolume = ParseTradingVolume(src.LatestVolumeTradingVolume);
                    dest.LatestVolumeTradingVolumeTenDayAverageVolume = ParseTradingVolume(src.LatestVolumeTradingVolumeTenDayAverageVolume);

                    dest.PublicLimitedCompany = ParseIdValuePair(src.PublicLimitedCompanyID, src.PublicLimitedCompanyName);
                    dest.InvestmentTarget = ParseIdValuePair(src.InvestmentTargetID, src.InvestmentTargetName);
                    dest.InvestmentRegion = ParseIdValuePair(src.InvestmentRegionID, src.InvestmentRegionName);
                    dest.InvestmentStyle = ParseIdValuePair(src.InvestmentStyleID, src.InvestmentStyleName);
                    dest.EstablishmentSeniority = new IdPair()
                    {
                        Value = src.EstablishmentSeniority,
                        Text = src.EstablishmentSeniority > 0 ? Convert.ToString(src.EstablishmentSeniority) : "-"
                    };
                    dest.TotalManagementFee = ParseVolume(src.TotalManagementFee.RoundingPercentage(), "%");
                    dest.ScaleMillions = ParseVolume(src.ScaleMillions.RoundingValue());

                    bool availability = Xcms.Sitecore.Foundation.Basic.Extensions.Extender.ToBoolean(src.AvailabilityStatus);
                    bool onlinePurchaseAvailability = Xcms.Sitecore.Foundation.Basic.Extensions.Extender.ToBoolean(src.OnlineSubscriptionAvailability) || string.IsNullOrEmpty(src.OnlineSubscriptionAvailability);
                    //「是否上架」= Y 且「是否可於網路申購」= Y或空白, 顯示申購鈕
                    dest.CanOnlineSubscription = availability && onlinePurchaseAvailability;

                    if (dicTag.ContainsKey(TagType.Discount))
                    {
                        dest.DiscountTags = dicTag[TagType.Discount].Where(i => i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode))
                                                                    .Select(i => i.TagKey).ToArray();
                    }
                    else
                    {
                        dest.DiscountTags = [];
                    }

                    if (dicTag.ContainsKey(TagType.Category))
                    {
                        dest.CategoryTags = dicTag[TagType.Category].Where(i => i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode))
                                                                    .Select(i => i.TagKey).ToArray();
                    }
                    else
                    {
                        dest.CategoryTags = [];
                    }

                    if (dicTag.ContainsKey(TagType.Keywords))
                    {
                        dest.KeywordsTags = dicTag[TagType.Keywords].Where(i => i.ProductCodes.Any() && i.ProductCodes.Contains(src.ProductCode))
                                                                    .Select(i => i.TagKey).ToArray();
                    }
                    else
                    {
                        dest.KeywordsTags = [];
                    }

                    dest.CurrencyHtml = PublicHelpers.CurrencyLink(null, null, src.CurrencyName).ToString();
                    dest.FocusButtonHtml = PublicHelpers.FocusButton(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF, true).ToString();
                    dest.CompareButtonHtml = PublicHelpers.CompareButton(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF, true).ToString();
                    dest.SubscribeButtonHtml = PublicHelpers.SubscriptionButton(null, null, src.FirstBankCode, InvestTypeEnum.ETF, true).ToString();
                    dest.FocusButtonAutoHtml = PublicHelpers.FocusTag(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF).ToString();
                    dest.SubscribeButtonAutoHtml = PublicHelpers.SubscriptionTag(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF).ToString();
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

            model.MarketPricePerformanceIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.MarketPricePerformanceIntro);
            model.NetWorthPerformanceIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.NetWorthPerformanceIntro);
            model.MarketPriceRiskIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.MarketPriceRiskIntro);
            model.NetWorthRiskIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.NetWorthRiskIntro);
            model.TransactionStatusIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.TransactionStatusIntro);
            model.InformationIntro = dataSource.Field(Templates.EtfSearchDatasource.Fields.InformationIntro);
            model.DetailPageLink = EtfRelatedLinkSetting.GetETFDetailUrl();
            GetTagsForFilterOption(dataSource);

            return model;
        }

        private void GetTagsForFilterOption(Item dataSource)
        {
            if (dataSource == null)
            {
                return;
            }

            var hotKeywords = dataSource.GetMultiListValueItems(Templates.EtfSearchDatasource.Fields.HotKeywords);
            var hotProducts = dataSource.GetMultiListValueItems(Templates.EtfSearchDatasource.Fields.HotProduct);
            this.HotKeywordList = TagRepository.ParseTagContent(hotKeywords)?.ToList();
            this.HotProductList = TagRepository.ParseTagContent(hotProducts)?.ToList();
        }

        #region Method

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

        private Percentage ParsePercentageChange(decimal? number)
        {
            var pair = new Percentage()
            {
                IsUp = IsUpPercentage(number),
                Value = number.RoundingPercentage(),
            };
            pair.Text = number.HasValue ? Math.Abs(pair.Value.Value) + "%" : "-";

            if (number.HasValue)
            {
                pair.Style = pair.IsUp ? "o-rise" : "o-fall";
            }
            return pair;
        }

        private VolumePair ParseTradingVolume(decimal? number)
        {
            var pair = new VolumePair
            {
                Value = number.HasValue ? number.Value : null,
                Text = number.HasValue ? NumberExtensions.FormatNumber(number) : "-"
            };
            return pair;
        }

        private VolumePair ParseVolume(decimal? number, string suffix = "")
        {
            var pair = new VolumePair()
            {
                Value = number.HasValue ? number.Value : null,
                Text = number.HasValue ? number.Value.ToString() + suffix : "-"
            };
            return pair;
        }

        private IdValuePair ParseIdValuePair(int id, string name)
        {
            var pair = new IdValuePair()
            {
                Key = id,
                Value = name,
                Text = string.IsNullOrEmpty(name) ? "-" : name
            };
            return pair;
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

        private IEnumerable<EtfSearchResult> SearchResults { get; set; }

        /// <summary>
        /// 熱門關鍵字
        /// </summary>
        public List<ProductTag> HotKeywordList { get; set; }

        /// <summary>
        /// 熱門主題
        /// </summary>
        public List<ProductTag> HotProductList { get; set; }

        private EtfTagRepository TagRepository { get; set; } = new EtfTagRepository();


        #endregion Property
    }
}