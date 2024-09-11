using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Models.FundSearch;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.SiteProductSearch;
using Feature.Wealth.Component.Models.SiteProductSearch.Product;
using Feature.Wealth.Component.Models.StructuredProduct;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Mapster;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Repositories
{
    public class SearchBarRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string SearchBarCache = $"Fcb_SearchBarCache";
        private readonly string CacheTime = $"SearchBarCacheTime";

        public RespProduct GetResultList()
        {
            var product = _cache.Get(SearchBarCache) as RespProduct;

            if (product == null)
            {
                product = new RespProduct
                {
                    FundProducts = MapperFundResult()?.ToList(),
                    ETFProducts = MapperETFResult()?.ToList(),
                    ForeignStocks = MapperForeignStockResult()?.ToList(),
                    ForeignBonds = MapperForeignBondsResult()?.ToList(),
                    StructuredProducts = MapperStructuredProductResult()?.ToList(),
                };
                _cache.Set(SearchBarCache, product, new CommonRepository().GetCacheExpireTime(Settings.GetSetting(CacheTime)));
            }

            return product;
        }

        #region 基金

        public IEnumerable<FundProductResult> MapperFundResult()
        {
            var collection = new FundSearchRepository().GetFundSearchData();

            var config = new TypeAdapterConfig();
            config.ForType<FundSearchModel, FundProductResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.ProductName = src.FundName?.Normalize(NormalizationForm.FormKC) ?? string.Empty;
                    dest.NetAssetValue = src.NetAssetValue.RoundingValue();
                    dest.NetAssetValueDate = DateTimeExtensions.FormatDate(src.NetAssetValueDate);
                    dest.CurrencyPair = new KeyValuePair<string, string>(src.CurrencyCode, src.CurrencyName);

                    #region == 報酬率 ==

                    #region 報酬率 (原幣)

                    dest.OneMonthReturnOriginalCurrency = ParseReturnToKeyValue(src.OneMonthReturnOriginalCurrency);
                    dest.ThreeMonthReturnOriginalCurrency = ParseReturnToKeyValue(src.ThreeMonthReturnOriginalCurrency);
                    dest.SixMonthReturnOriginalCurrency = ParseReturnToKeyValue(src.SixMonthReturnOriginalCurrency);
                    dest.OneYearReturnOriginalCurrency = ParseReturnToKeyValue(src.OneYearReturnOriginalCurrency);

                    #endregion 報酬率 (原幣)

                    #region 報酬率 (台幣)

                    dest.OneMonthReturnTWD = ParseReturnToKeyValue(src.OneMonthReturnTWD);
                    dest.ThreeMonthReturnTWD = ParseReturnToKeyValue(src.ThreeMonthReturnTWD);
                    dest.SixMonthReturnTWD = ParseReturnToKeyValue(src.SixMonthReturnTWD);
                    dest.OneYearReturnTWD = ParseReturnToKeyValue(src.OneYearReturnTWD);

                    #endregion 報酬率 (台幣)

                    #endregion == 報酬率 ==

                    bool availability = IsAvailability(src.AvailabilityStatus);
                    bool onlinePurchaseAvailability = IsAvailability(src.OnlineSubscriptionAvailability) || string.IsNullOrEmpty(src.OnlineSubscriptionAvailability);
                    dest.CanOnlineSubscription = availability && onlinePurchaseAvailability;

                    dest.CurrencyHtml = PublicHelpers.CurrencyLink(null, null, src.CurrencyName).ToString();
                    dest.FocusButtonAutoHtml = PublicHelpers.FocusTag(null, null, src.ProductCode, dest.ProductName, InvestTypeEnum.Fund).ToString();
                    dest.SubscribeButtonAutoHtml = PublicHelpers.SubscriptionTag(null, null, src.ProductCode, dest.ProductName, InvestTypeEnum.Fund).ToString();
                });

            var result = collection.Adapt<IEnumerable<FundProductResult>>(config);

            return result;
        }

        #endregion 基金

        #region ETF

        public IEnumerable<EtfProductResult> MapperETFResult()
        {
            var collection = new EtfSearchRepository().QueryBasicData();

            Type[] ignoreTypes =
            [
                 typeof(Percentage),
                 typeof(VolumePair),
                 typeof(IdValuePair),
                 typeof(StringPair),
                 typeof(IdPair)
            ];

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfProductResult>()
                .IgnoreMember((member, side) => ignoreTypes.Contains(member.Type))
                .AfterMapping((src, dest) =>
                {
                    dest.ETFName = src.ETFName?.Normalize(NormalizationForm.FormKC) ?? string.Empty;
                    dest.ExchangeCode = new StringPair()
                    {
                        Value = src.ExchangeCode,
                        Text = string.IsNullOrEmpty(src.ExchangeCode) ? "-" : src.ExchangeCode.ToString()
                    };
                    dest.CurrencyPair = new StringPair()
                    {
                        Key = src.CurrencyCode,
                        Value = src.CurrencyName,
                        Text = string.IsNullOrEmpty(src.CurrencyName) ? "-" : src.CurrencyName
                    };
                    dest.NetAssetValueDate = DateTimeExtensions.FormatDate(src.NetAssetValueDate);
                    dest.MarketPrice = ParseVolume(src.MarketPrice.RoundingValue());
                    dest.NetAssetValue = ParseVolume(src.NetAssetValue.RoundingValue());

                    #region == 報酬率 ==

                    #region 報酬率 (市價原幣)

                    dest.InceptionDateMarketPriceOriginalCurrency = ParsePercentageChange(src.InceptionDateMarketPriceOriginalCurrency);
                    dest.YeartoDateReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.YeartoDateReturnMarketPriceOriginalCurrency);
                    dest.MonthlyReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.MonthlyReturnMarketPriceOriginalCurrency);
                    dest.ThreeMonthReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.ThreeMonthReturnMarketPriceOriginalCurrency);
                    dest.SixMonthReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.SixMonthReturnMarketPriceOriginalCurrency);
                    dest.OneYearReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.OneYearReturnMarketPriceOriginalCurrency);
                    dest.TwoYearReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.TwoYearReturnMarketPriceOriginalCurrency);
                    dest.ThreeYearReturnMarketPriceOriginalCurrency = ParsePercentageChange(src.ThreeYearReturnMarketPriceOriginalCurrency);

                    #endregion 報酬率 (市價原幣)

                    #region 報酬率 (市價台幣)

                    dest.InceptionDateMarketPriceTWD = ParsePercentageChange(src.InceptionDateMarketPriceTWD);
                    dest.YeartoDateReturnMarketPriceTWD = ParsePercentageChange(src.YeartoDateReturnMarketPriceTWD);
                    dest.MonthlyReturnMarketPriceTWD = ParsePercentageChange(src.MonthlyReturnMarketPriceTWD);
                    dest.ThreeMonthReturnMarketPriceTWD = ParsePercentageChange(src.ThreeMonthReturnMarketPriceTWD);
                    dest.SixMonthReturnMarketPriceTWD = ParsePercentageChange(src.SixMonthReturnMarketPriceTWD);
                    dest.OneYearReturnMarketPriceTWD = ParsePercentageChange(src.OneYearReturnMarketPriceTWD);
                    dest.TwoYearReturnMarketPriceTWD = ParsePercentageChange(src.TwoYearReturnMarketPriceTWD);
                    dest.ThreeYearReturnMarketPriceTWD = ParsePercentageChange(src.ThreeYearReturnMarketPriceTWD);

                    #endregion 報酬率 (市價台幣)

                    #endregion == 報酬率 ==

                    bool availability = IsAvailability(src.AvailabilityStatus);
                    bool onlinePurchaseAvailability = IsAvailability(src.OnlineSubscriptionAvailability) || string.IsNullOrEmpty(src.OnlineSubscriptionAvailability);
                    dest.CanOnlineSubscription = availability && onlinePurchaseAvailability;

                    dest.CurrencyHtml = PublicHelpers.CurrencyLink(null, null, src.CurrencyName).ToString();
                    dest.FocusButtonAutoHtml = PublicHelpers.FocusTag(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF).ToString();
                    dest.SubscribeButtonAutoHtml = PublicHelpers.SubscriptionTag(null, null, src.FirstBankCode, dest.ETFName, InvestTypeEnum.ETF).ToString();
                });

            var result = collection.Adapt<IEnumerable<EtfProductResult>>(config);

            return result;
        }

        #endregion ETF

        #region 國外股票

        public IList<USStockListDto> QueryForeignStockData()
        {
            string sqlQuery = """
                SELECT [FirstBankCode]
                    ,[FundCode]
                    ,[EnglishName]
                    ,[ChineseName]
                    ,[DataDate]
                    ,CAST([ClosingPrice] AS DECIMAL(12, 2)) AS [ClosingPrice]
                    ,[DailyReturn]
                    ,[WeeklyReturn]
                    ,[MonthlyReturn]
                    ,[ThreeMonthReturn]
                    ,[OneYearReturn]
                    ,[YeartoDateReturn]
                    ,[ChangePercentage]
                    ,[SixMonthReturn]
                    ,[MainTable].[AvailabilityStatus]
                    ,[MainTable].[OnlineSubscriptionAvailability]
                FROM [dbo].[Sysjust_USStockList] AS [StockTable] WITH (NOLOCK)
                LEFT JOIN [dbo].[WMS_DOC_RECM] AS [MainTable] WITH (NOLOCK)
                    ON [StockTable].[FirstBankCode] = [MainTable].[ProductCode]
                ORDER BY [StockTable].[MonthlyReturn] DESC, [StockTable].[FirstBankCode] ASC
                """;
            var collection = DbManager.Custom.ExecuteIList<USStockListDto>(sqlQuery, null, CommandType.Text);
            return collection;
        }

        public IEnumerable<ForeignStockResult> MapperForeignStockResult()
        {
            var collection = QueryForeignStockData();

            var config = new TypeAdapterConfig();
            config.ForType<USStockListDto, ForeignStockResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.FundCodePair = new KeyValuePair<string, string>(src.FundCode, string.IsNullOrEmpty(src.FundCode) ? "-" : src.FundCode);
                    dest.ClosingPrice = new KeyValuePair<decimal?, string>(src.ClosingPrice, src.ClosingPrice.HasValue ? src.ClosingPrice.Value.ToString() : "-");
                    dest.DataDate = DateTimeExtensions.FormatDate(src.DataDate);
                    dest.DailyReturn = ParseReturnToKeyValue(src.DailyReturn);
                    dest.WeeklyReturn = ParseReturnToKeyValue(src.WeeklyReturn);
                    dest.MonthlyReturn = ParseReturnToKeyValue(src.MonthlyReturn);
                    dest.ThreeMonthReturn = ParseReturnToKeyValue(src.ThreeMonthReturn);
                    dest.OneYearReturn = ParseReturnToKeyValue(src.OneYearReturn);
                    dest.YeartoDateReturn = ParseReturnToKeyValue(src.YeartoDateReturn);
                    dest.SixMonthReturn = ParseReturnToKeyValue(src.SixMonthReturn);
                    dest.ChangePercentage = ParseReturnToKeyValue(src.ChangePercentage);

                    bool availability = IsAvailability(src.AvailabilityStatus);
                    bool onlinePurchaseAvailability = IsAvailability(src.OnlineSubscriptionAvailability) || string.IsNullOrEmpty(src.OnlineSubscriptionAvailability);
                    dest.CanOnlineSubscription = availability && onlinePurchaseAvailability;

                    string fullName = string.Concat(src.FirstBankCode, src.ChineseName, src.EnglishName);

                    dest.FocusButtonAutoHtml = PublicHelpers.FocusTag(null, null, src.FirstBankCode, fullName, InvestTypeEnum.ForeignStocks).ToString();
                    dest.SubscribeButtonAutoHtml = PublicHelpers.SubscriptionTag(null, null, src.FirstBankCode, fullName, InvestTypeEnum.ForeignStocks).ToString();
                });

            var result = collection.Adapt<IEnumerable<ForeignStockResult>>(config);

            return result;
        }

        #endregion 國外股票

        #region 結構型商品

        public IList<BasicStructuredProductDto> QueryStructuredProductBasicData()
        {
            string sqlQuery = """
                SELECT *
                FROM [vw_StructProduct]
                """;
            var collection = DbManager.Custom.ExecuteIList<BasicStructuredProductDto>(sqlQuery, null, CommandType.Text);
            return collection;
        }

        public IEnumerable<StructuredProductResult> MapperStructuredProductResult()
        {
            var collection = QueryStructuredProductBasicData();

            var config = new TypeAdapterConfig();
            config.ForType<BasicStructuredProductDto, StructuredProductResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.IssuingInstitutionPair = new KeyValuePair<string, string>(src.IssuingInstitution, string.IsNullOrEmpty(src.IssuingInstitution) ? "-" : src.IssuingInstitution);
                    dest.ProductMaturityDatePair = new KeyValuePair<string, string>(src.ProductMaturityDate, string.IsNullOrEmpty(src.ProductMaturityDate) ? "-" : src.ProductMaturityDate);
                    dest.CurrencyPair = new KeyValuePair<string, string>(src.CurrencyCode, src.CurrencyName);
                    dest.BankSellPricePair = new KeyValuePair<string, string>(src.BankSellPrice, string.IsNullOrEmpty(src.BankSellPrice) ? "-" : src.BankSellPrice);
                    dest.PriceBaseDatePair = new KeyValuePair<string, string>(src.PriceBaseDate, string.IsNullOrEmpty(src.PriceBaseDate) ? "-" : src.PriceBaseDate);

                    dest.CurrencyHtml = PublicHelpers.CurrencyLink(null, null, src.CurrencyName).ToString();
                });

            var result = collection.Adapt<IEnumerable<StructuredProductResult>>(config);

            return result;
        }

        #endregion 結構型商品

        #region 國外債券

        public IList<BondListDto> GetBondsList()
        {
            string sql = @"SELECT
                           A.[BondCode]
                           ,A.[ISINCode]
                           ,A.[BondName]
                           ,A.[Currency]
                           ,A.[CurrencyCode]
                           ,A.[InterestRate]
                           ,A.[PaymentFrequency]
                           ,A.[RiskLevel]
                           ,A.[SalesTarget]
                           ,A.[MinSubscriptionForeign]
                           ,A.[MinSubscriptionNTD]
                           ,A.[MinIncrementAmount]
                           ,A.[RedemptionDateByIssuer]
                           ,A.[Issuer]
                           ,A.[OpenToPublic]
                           ,A.[Listed]
                           ,B.[SubscriptionFee]
                           ,B.[RedemptionFee]                           
                           ,B.[ReservedColumn]
                           ,B.[Note]
                           ,B.[PreviousInterest]
                           ,B.[SPCreditRating]
                           ,B.[MoodyCreditRating]
                           ,B.[FitchCreditRating]
                           ,B.[YieldRateYTM]
                           FROM [BondList] AS A WITH (NOLOCK)
                           LEFT JOIN [BondNav] AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)";

            var bonds = DbManager.Custom.ExecuteIList<BondListDto>(sql, null, CommandType.Text);

            var minDate = bonds
           .Where(b => !string.IsNullOrEmpty(b.Date))
           .OrderBy(b => b.Date)
           .Select(b => b.Date).FirstOrDefault();

            IList<BondHistoryPrice> _bondHistoryPrices = new List<BondHistoryPrice>();
            if (DateTime.TryParse(minDate, out var fourMonthAgo))
            {
                fourMonthAgo = fourMonthAgo.AddMonths(-1).AddDays(-10);
                _bondHistoryPrices = new BondRepository().GetBondHistoryPriceByDate(fourMonthAgo.ToString("yyyyMMdd"));
            }

            for (int i = 0; i < bonds.Count; i++)
            {
                bonds[i] = MoreInfo(_bondHistoryPrices, bonds[i]);
            }

            return bonds;
        }

        private BondListDto MoreInfo(IList<BondHistoryPrice> _bondHistoryPrices, BondListDto bond)
        {
            var now = DateTime.Now;

            bond.InterestRate = bond.InterestRate.DecimalNumber(4);
            bond.SubscriptionFee = bond.SubscriptionFee.DecimalNumber(2);
            bond.RedemptionFee = bond.RedemptionFee.DecimalNumber(2);
            bond.PreviousInterest = bond.PreviousInterest.DecimalNumber(4);
            bond.YieldRateYTM = bond.YieldRateYTM.DecimalNumber(2);

            if (DateTime.TryParse(bond.MaturityDate, out var d))
            {
                var diff = d.Subtract(now).TotalDays;
                if (diff > 0)
                {
                    bond.MaturityYear = decimal.Parse((diff / 365).ToString());
                }
                else
                {
                    bond.MaturityYear = 0;
                }
            }
            else
            {
                bond.MaturityYear = 0;
            }

            bond.MaturityYear = bond.MaturityYear.DecimalNumber(2);

            if (int.TryParse(bond.MinIncrementAmount, out var min))
            {
                bond.MinIncrementAmountNumber = min;
            }
            else
            {
                bond.MinIncrementAmountNumber = 0;
            }

            if (DateTime.TryParse(bond.Date, out var oneMonthAgo))
            {
                oneMonthAgo = oneMonthAgo.AddMonths(-1);
                bond.UpsAndDownsMonth = GetUpsAndDowns(_bondHistoryPrices, bond, oneMonthAgo.ToString("yyyyMMdd"));
            }

            bond.UpsAndDownsMonth = bond.UpsAndDownsMonth.DecimalNumber(2);
            bond.UpsAndDownsSeason = bond.UpsAndDownsSeason.DecimalNumber(2);
            return bond;
        }

        private decimal? GetUpsAndDowns(IList<BondHistoryPrice> _bondHistoryPrices, BondListDto bond, string date)
        {
            BondHistoryPrice bondHistoryPrice = null;

            if (_bondHistoryPrices != null && _bondHistoryPrices.Any())
            {
                bondHistoryPrice = _bondHistoryPrices.FirstOrDefault(b => b.BondCode == bond.BondCode && int.Parse(b.Date) <= int.Parse(date));
            }

            if (bondHistoryPrice != null)
            {
                if (bond.SubscriptionFee.HasValue && bond.SubscriptionFee != 0 && bondHistoryPrice.SubscriptionFee.HasValue && bondHistoryPrice.SubscriptionFee != 0)
                {
                    return (bond.SubscriptionFee - bondHistoryPrice.SubscriptionFee) / bondHistoryPrice.SubscriptionFee * 100;
                }
                else if (bond.RedemptionFee.HasValue && bond.RedemptionFee != 0 && bondHistoryPrice.RedemptionFee.HasValue && bondHistoryPrice.RedemptionFee != 0)
                {
                    return (bond.RedemptionFee - bondHistoryPrice.RedemptionFee) / bondHistoryPrice.RedemptionFee * 100;
                }
            }

            return null;
        }

        public IEnumerable<ForeignBondsResult> MapperForeignBondsResult()
        {
            var collection = GetBondsList();

            var config = new TypeAdapterConfig();
            config.ForType<BondListDto, ForeignBondsResult>()
                .AfterMapping((src, dest) =>
                {
                    dest.FocusButtonAutoHtml = PublicHelpers.FocusTag(null, null, src.BondCode, dest.BondName, InvestTypeEnum.ForeignBonds).ToString();
                    dest.SubscribeButtonAutoHtml = PublicHelpers.SubscriptionTag(null, null, src.BondCode, dest.BondName, InvestTypeEnum.ForeignBonds).ToString();
                });

            var result = collection.Adapt<IEnumerable<ForeignBondsResult>>(config);
            return result.OrderByDescending(i => i.UpsAndDownsMonth);
        }

        #endregion 國外債券

        #region Method

        private bool IsAvailability(string value)
        {
            return Extender.ToBoolean(value);
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

        private VolumePair ParseVolume(decimal? number, string suffix = "")
        {
            var pair = new VolumePair()
            {
                Value = number.HasValue ? number.Value : null,
                Text = number.HasValue ? number.Value.ToString() + suffix : "-"
            };
            return pair;
        }

        private KeyValuePair<bool, decimal?> ParseReturnToKeyValue(decimal? number)
        {
            return new KeyValuePair<bool, decimal?>(IsUpPercentage(number), number.RoundingPercentage());
        }

        #endregion Method
    }
}