using Feature.Wealth.Component.Models.Bond;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Search;
using Feature.Wealth.Component.Models.FundSearch;
using Feature.Wealth.Component.Models.SearchBar;
using Feature.Wealth.Component.Models.SiteProductSearch.Product;
using Feature.Wealth.Component.Models.StructuredProduct;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using Mapster;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

        public RespSearch GetResultList()
        {
            var product = _cache.Get(SearchBarCache) as RespSearch;

            if (product == null)
            {
                product = new RespSearch
                {
                    FundProducts = MapperFundResult()?.ToList(),
                    ETFProducts = MapperETFResult()?.ToList(),
                    ForeignStocks = MapperForeignStockResult().ToList(),
                    ForeignBonds = MapperForeignBondsResult().ToList(),
                    StructuredProducts = MapperStructuredProductResult().ToList(),
                };
                _cache.Set(SearchBarCache, product, new CommonRepository().GetCacheExpireTime(Settings.GetSetting(CacheTime)));
            }

            return product;
        }

        #region 基金

        public IEnumerable<Dictionary<string, object>> MapperFundResult()
        {
            var collection = new FundSearchRepository().GetFundSearchData().OrderByDescending(i => i.SixMonthReturnOriginalCurrency);

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
                });

            var result = collection.Adapt<IEnumerable<FundProductResult>>(config);

            return result.Select(x => new Dictionary<string, object> {
                { "ProductCode", x.ProductCode },
                { "ProductName", x.ProductName },
                { "CanOnlineSubscription", x.CanOnlineSubscription },
            });
        }

        #endregion 基金

        #region ETF

        public IEnumerable<Dictionary<string, object>> MapperETFResult()
        {
            var collection = new EtfSearchRepository().QueryBasicData().OrderByDescending(i => i.SixMonthReturnMarketPriceOriginalCurrency);

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
                    dest.MarketPriceDate = DateTimeExtensions.FormatDate(src.BasicMarketPriceDate);
                    dest.MarketPrice = ParseVolume(src.BasicMarketPrice.RoundingValue());

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
                });

            var result = collection.Adapt<IEnumerable<EtfProductResult>>(config);

            return result.Select(x => new Dictionary<string, object> {
                { "FirstBankCode", x.FirstBankCode },
                { "ETFName", x.ETFName },
                { "CanOnlineSubscription", x.CanOnlineSubscription },
            });
        }

        #endregion ETF

        #region 國外股票

        public IList<USStockListDto> QueryForeignStockData()
        {
            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string sqlQuery = $@"
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
                FROM {Sysjust_USStockList} AS [StockTable] WITH (NOLOCK)
                LEFT JOIN {WMS_DOC_RECM} AS [MainTable] WITH (NOLOCK)
                    ON [StockTable].[FirstBankCode] = [MainTable].[ProductCode]
                ORDER BY [StockTable].[MonthlyReturn] DESC, [StockTable].[FirstBankCode] ASC
                ";
            var collection = DbManager.Custom.ExecuteIList<USStockListDto>(sqlQuery, null, CommandType.Text);
            return collection;
        }

        public IEnumerable<Dictionary<string, object>> MapperForeignStockResult()
        {
            var collection = QueryForeignStockData();

            var config = new TypeAdapterConfig();
            config.ForType<USStockListDto, ForeignStockResult>()
                .AfterMapping((src, dest) =>
                {
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
                });

            var result = collection.Adapt<IEnumerable<ForeignStockResult>>(config);

            return result.Select(x => new Dictionary<string, object> {
                { "FirstBankCode", x.FirstBankCode },
                { "ChineseName", x.ChineseName },
                { "EnglishName", x.EnglishName },
                { "CanOnlineSubscription", x.CanOnlineSubscription },
            });
        }

        #endregion 國外股票

        #region 國外債券

        public IEnumerable<SafeBondListDto> GetBondsList()
        {
            string BondList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondList);
            string BondNav = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondNav);

            string sql = @$"SELECT
                            A.[BondCode]
                            ,A.[BondName]
                            ,A.[InterestRate]
                            ,A.[MinIncrementAmount]
                            ,A.[OpenToPublic]
                            ,A.[Listed]
                            ,EF.BankBuyPrice AS [SubscriptionFee]
                            ,EF.BankSellPrice AS [RedemptionFee]
                            ,CASE 
                            WHEN EF.PriceBaseDate IS NOT NULL 
                            THEN FORMAT(TRY_CAST(CONCAT((TRY_CONVERT(INT, LEFT(EF.PriceBaseDate, 3)) + 1911), RIGHT(EF.PriceBaseDate, 4)) AS DATE), 'yyyy/MM/dd')
                            END AS [Date]
                            ,B.[PreviousInterest]
                            ,B.[YieldRateYTM]
                            FROM {BondList} AS A WITH (NOLOCK)
                            LEFT JOIN {BondNav} AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                            LEFT JOIN
                            (
                            SELECT 
                            [ProductIdentifier]
                            ,[DataDate]
                            ,[BankProductCode]
                            ,[BankBuyPrice]
                            ,[BankSellPrice]
                            ,[PriceBaseDate]
                            ,ROW_NUMBER() OVER(PARTITION BY [BankProductCode] ORDER BY [DataDate] DESC) AS [RowNumber]
                            FROM [FUND_ETF] WITH (NOLOCK)
                            WHERE [ProductIdentifier] = 'B'
                            ) AS EF ON A.BondCode = EF.BankProductCode AND EF.[RowNumber] = 1";

            var bonds = DbManager.Custom.ExecuteIList<BondListDto>(sql, null, CommandType.Text);

            if (!bonds.Any())
            {
                return new List<SafeBondListDto>();
            }

            var minDate = bonds
           .Where(b => !string.IsNullOrEmpty(b.Date))
           .OrderBy(b => b.Date)
           .Select(b => b.Date).FirstOrDefault();

            IList<BondHistoryPrice> _bondHistoryPrices = new List<BondHistoryPrice>();

            if (DateTime.TryParseExact(minDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fourMonthAgo))
            {
                fourMonthAgo = fourMonthAgo.AddMonths(-1).AddDays(-10);
                _bondHistoryPrices = new BondRepository().GetBondHistoryPriceByDate(fourMonthAgo.ToString("yyyyMMdd"));
            }

            for (int i = 0; i < bonds.Count; i++)
            {
                DataFormat(_bondHistoryPrices, bonds[i]);
            }

            var collection = bonds.OrderByDescending(i => i.UpsAndDownsMonth);

            var config = new TypeAdapterConfig();
            config.ForType<BondListDto, SafeBondListDto>()
                .AfterMapping((src, dest) =>
                {
                    src.WithoutSensitiveData();
                });

            var result = collection.Adapt<IEnumerable<SafeBondListDto>>(config);
            return result;
        }

        private void DataFormat(IList<BondHistoryPrice> _bondHistoryPrices, BondListDto bond)
        {
            if (DateTime.TryParseExact(bond.Date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var oneMonthAgo))
            {
                oneMonthAgo = oneMonthAgo.AddMonths(-1);
                bond.UpsAndDownsMonth = GetUpsAndDowns(_bondHistoryPrices, bond, oneMonthAgo.ToString("yyyyMMdd"));
            }

            bond.UpsAndDownsMonth = bond.UpsAndDownsMonth.DecimalNumber(2);
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

        public IEnumerable<Dictionary<string, string>> MapperForeignBondsResult() => GetBondsList()
            .Select(
                item => new Dictionary<string, string>
                {
                    { "BondCode", item.BondCode },
                    { "BondName", item.BondName },
                    { "OpenToPublic", item.OpenToPublic },
                    { "Listed", item.Listed },
                });

        #endregion 國外債券

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

        public IEnumerable<Dictionary<string, string>> MapperStructuredProductResult()
        {
            var collection = QueryStructuredProductBasicData().OrderBy(i => i.ProductCode);
            var result = collection.Select(
                item => new Dictionary<string, string>
                {
                    { "ProductCode", item.ProductCode },
                    { "ProductName", item.ProductName },
                }
            );

            return result;
        }

        #endregion 結構型商品

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