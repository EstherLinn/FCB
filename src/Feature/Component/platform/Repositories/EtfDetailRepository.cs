using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.ETF.Detail;
using Feature.Wealth.Component.Models.ETF.Tag;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Manager;
using Mapster;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class EtfDetailRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string ETFDetailsCacheKey = $"Fcb_ETFDetailsCache";

        /// <summary>
        /// 代碼
        /// </summary>
        public string ETFId { get; set; }

        /// <summary>
        /// 產品註記
        /// </summary>
        public string Indicator { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        public Dictionary<TagType, List<ProductTag>> TagCollection { get; set; }

        public EtfDetailModel GetETFDetailModel(string etfId, Item dataSource)
        {
            this.Indicator = GetProductIdentifier(etfId);
            EtfDetailModel model;
            model = GetOrSetETFDetailsCache(etfId);
            GetDatasourceData(model, dataSource);
            return model;
        }

        /// <summary>
        /// 取得產品註記
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        private string GetProductIdentifier(string etfId)
        {
            string sql = """
                SELECT ProductIdentifier
                FROM [FUND_ETF] WITH (NOLOCK)
                WHERE [BankProductCode] = @ETFId
                """;
            var para = new { ETFId = etfId };
            string indicator = DbManager.Custom.Execute<string>(sql, para, CommandType.Text);
            return indicator;
        }

        public EtfDetailModel GetOrSetETFDetailsCache(string etfId)
        {
            this.ETFId = etfId;
            var etfDic = (Dictionary<string, EtfDetailModel>)_cache.Get(ETFDetailsCacheKey) ?? new Dictionary<string, EtfDetailModel>();
            EtfDetailModel etfFullData = null;

            if (!etfDic.Any())
            {
                etfDic.Add(etfId, CreateETFDetailsData());
                _cache.Set(ETFDetailsCacheKey, etfDic, DateTimeOffset.Now.AddMinutes(60));
            }
            else
            {
                if (etfDic.TryGetValue(etfId, out etfFullData))
                {
                    return etfFullData;
                }
                else
                {
                    etfDic.Add(etfId, CreateETFDetailsData());
                }
            }

            etfFullData = etfDic[etfId];
            return etfFullData;
        }

        private void GetDatasourceData(EtfDetailModel detailModel, Item dataSource)
        {
            if (dataSource == null)
            {
                return;
            }

            detailModel.Datasource = dataSource;
            detailModel.AccordionContent = dataSource.Field(Templates.EtfDetailDatasource.Fields.AccordionContent);
            detailModel.RiskIntro = dataSource.Field(Templates.EtfDetailDatasource.Fields.RiskIntro);
            detailModel.RiskQuadrantChartDescription = dataSource.Field(Templates.EtfDetailDatasource.Fields.RiskQuadrantChartDescription);
            detailModel.DividendRecordDescription = dataSource.Field(Templates.EtfDetailDatasource.Fields.DividendRecordDescription);
        }

        private EtfDetailModel CreateETFDetailsData()
        {
            EtfDetailModel model = new EtfDetailModel();

            var result = MapperResult();

            if (result == null)
            {
                return model;
            }

            model.BasicEtf = result;

            EtfTagRepository tagRepository = new EtfTagRepository();
            this.TagCollection = tagRepository.GetTagCollection();
            model.DiscountTags = GetTags(TagType.Discount);
            model.CategoryTags = GetTags(TagType.Category);
            model.ETFMarketPriceOverPastYear = GetMarketPriceWithOverPastYear();
            model.ETFNetWorthOverPastYear = GetNetWorthWithOverPastYear();
            model.ETFTypeRanks = GetSameTypeETFRank();
            model.ETFThiryDaysNav = GetThrityDaysNav();
            model.ETFNetWorthAnnunalReturn = GetAnnualReturn();
            model.ETFNetWorthMonthlyReturn = GetNetWortMonthlyReturn();
            model.ETFTradingPrice = GetBuyAndSellPriceData() ?? new EtfTradingPrice();
            model.ETFThiryDaysTradingPrice = GetThrityDaysBuyAndSellPrice();
            model.ETFStockHoldings = GetETFStockHolding();
            model.ETFRiskIndicator = GetETFRiskIndicator();
            model.ETFYearReturns = GetETFYearReturnCompare();
            model.ETFDividendRecords = GetDividendRecords();
            GetLastDividendRecord(model.BasicEtf, model.ETFDividendRecords);
            model.ETFScaleRecords = GetScalechange();

            return model;
        }

        /// <summary>
        /// 取得標籤
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        private string[] GetTags(TagType tagType)
        {
            var dicTag = this.TagCollection;

            if (dicTag.TryGetValue(tagType, out List<ProductTag> productTags))
            {
                string[] arrTag = productTags.Where(i => i.ProductCodes.Contains(this.ETFId)).Select(i => i.TagKey).ToArray();
                return arrTag;
            }

            return [];
        }

        private EtfDetail MapperResult()
        {
            string sqlQuery = """
                SELECT *
                FROM [vw_BasicETF]
                WHERE [ProductCode] = @ProductCode
                """;
            var param = new { ProductCode = this.ETFId };
            var product = DbManager.Custom.Execute<BasicEtfDto>(sqlQuery, param, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfDetail>()
                .AfterMapping((src, dest) =>
                {
                    dest.ETFName = src.ETFName.Normalize(NormalizationForm.FormKC);
                    //dest.MarketPrice = src.MarketPrice.FormatDecimalNumber();
                    dest.RiskLevel = src.RiskLevel;

                    bool availability = Xcms.Sitecore.Foundation.Basic.Extensions.Extender.ToBoolean(src.AvailabilityStatus);
                    bool onlinePurchaseAvailability = Xcms.Sitecore.Foundation.Basic.Extensions.Extender.ToBoolean(src.OnlineSubscriptionAvailability) || string.IsNullOrEmpty(src.OnlineSubscriptionAvailability);
                    //「是否上架」= Y 且「是否可於網路申購」= Y或空白, 顯示申購鈕
                    dest.CanOnlineSubscription = availability && onlinePurchaseAvailability;

                    dest.DividendDistributionFrequency = src.DividendDistributionFrequency.CheckNullOrEmptyString();
                    dest.InvestmentRegionName = src.InvestmentRegionName.CheckNullOrEmptyString();
                    dest.InvestmentTargetName = src.InvestmentTargetName.CheckNullOrEmptyString();
                    dest.InvestmentStyleName = src.InvestmentStyleName.CheckNullOrEmptyString();
                    dest.CurrencyName = src.CurrencyName.CheckNullOrEmptyString();
                    dest.IndicatorIndex = src.IndicatorIndex.CheckNullOrEmptyString();
                    dest.StockIndexName = src.StockIndexName.CheckNullOrEmptyString();

                    dest.ScaleMillions = NumberExtensions.Rounding(src.ScaleMillions, 2);
                    dest.ScaleDate = DateTimeExtensions.FormatDate(src.ScaleDate);
                    dest.EstablishmentDate = DateTimeExtensions.FormatDate(src.EstablishmentDate).CheckNullOrEmptyString();
                    // TODO: 規模幣別值有問題

                    // 近一年市價 / 淨值走勢
                    dest.HighestMarketPrice = src.HighestMarketPrice.FormatDecimalNumber(4);
                    dest.LowestMarketPrice = src.LowestMarketPrice.FormatDecimalNumber(4);
                    dest.HighestNetAssetValue = src.HighestNetAssetValue.FormatDecimalNumber(4);
                    dest.LowestNetAssestValue = src.LowestNetAssestValue.FormatDecimalNumber(4);

                    #region 市價

                    dest.MonthtoDateMarketPriceOriginalCurrency = src.MonthtoDateMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.MonthtoDateMarketPriceOriginalCurrencyStyle = src.MonthtoDateMarketPriceOriginalCurrency.DecimalNumberToStyle();

                    dest.DailyReturnMarketPriceOriginalCurrency = src.DailyReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.DailyReturnMarketPriceOriginalCurrencyStyle = src.DailyReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.WeeklyReturnMarketPriceOriginalCurrency = src.WeeklyReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.WeeklyReturnMarketPriceOriginalCurrencyStyle = src.WeeklyReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.MonthlyReturnMarketPriceOriginalCurrency = src.MonthlyReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.MonthlyReturnMarketPriceOriginalCurrencyStyle = src.MonthlyReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    // 近一年績效走勢
                    dest.ThreeMonthReturnMarketPriceOriginalCurrency = src.ThreeMonthReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.ThreeMonthReturnMarketPriceOriginalCurrencyStyle = src.ThreeMonthReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.SixMonthReturnMarketPriceOriginalCurrency = src.SixMonthReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.SixMonthReturnMarketPriceOriginalCurrencyStyle = src.SixMonthReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.YeartoDateReturnMarketPriceOriginalCurrency = src.YeartoDateReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.YeartoDateReturnMarketPriceOriginalCurrencyStyle = src.YeartoDateReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();

                    dest.OneYearReturnMarketPriceOriginalCurrency = src.OneYearReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.OneYearReturnMarketPriceOriginalCurrencyStyle = src.OneYearReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.TwoYearReturnMarketPriceOriginalCurrency = src.TwoYearReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.TwoYearReturnMarketPriceOriginalCurrencyStyle = src.TwoYearReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.ThreeYearReturnMarketPriceOriginalCurrency = src.ThreeYearReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.ThreeYearReturnMarketPriceOriginalCurrencyStyle = src.ThreeYearReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();
                    dest.FiveYearReturnMarketPriceOriginalCurrency = src.FiveYearReturnMarketPriceOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.FiveYearReturnMarketPriceOriginalCurrencyStyle = src.FiveYearReturnMarketPriceOriginalCurrency.DecimalNumberToStyle();

                    dest.MarketPriceChange = src.MarketPriceChange.FormatDecimalNumber();
                    dest.MarketPriceChangePercentage = src.MarketPriceChangePercentage.FormatDecimalNumber(needPercent: true);
                    dest.MarketPriceChangeStyle = src.MarketPriceChange.DecimalNumberToStyle();
                    dest.MarketPriceChangePercentageStyle = src.MarketPriceChangePercentage.DecimalNumberToStyle();
                    dest.AnnualizedStandardDeviationMarketPriceRisk = src.AnnualizedStandardDeviationMarketPriceRisk.FormatDecimalNumber(needAbs: false);
                    dest.SharpeRatioMarketPriceRisk = src.SharpeRatioMarketPriceRisk.FormatDecimalNumber(needAbs: false);
                    dest.BetaMarketPriceRisk = src.BetaMarketPriceRisk.FormatDecimalNumber(needAbs: false);

                    #endregion 市價

                    #region 淨值

                    dest.YeartoDateReturnNetValueOriginalCurrency = src.YeartoDateReturnNetValueOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.YeartoDateReturnNetValueOriginalCurrencyStyle = src.YeartoDateReturnNetValueOriginalCurrency.DecimalNumberToStyle();
                    dest.NetAssetValueDate = DateTimeExtensions.FormatDate(src.NetAssetValueDate);
                    dest.NetAssetValueChange = src.NetAssetValueChange.FormatDecimalNumber();
                    dest.NetAssetValueChangePercentage = src.NetAssetValueChangePercentage.FormatDecimalNumber(needPercent: true);
                    dest.NetAssetValueChangeStyle = src.NetAssetValueChange.DecimalNumberToStyle();
                    dest.NetAssetValueChangePercentageStyle = src.NetAssetValueChangePercentage.DecimalNumberToStyle();
                    dest.AnnualizedStandardDeviationNetValueRisk = src.AnnualizedStandardDeviationNetValueRisk.FormatDecimalNumber(needAbs: false);
                    dest.SharpeNetValueRisk = src.SharpeNetValueRisk.FormatDecimalNumber(needAbs: false);
                    dest.BetaNetValueRisk = src.BetaNetValueRisk.FormatDecimalNumber(needAbs: false);

                    #endregion 淨值

                    dest.AvailabilityStatus = src.AvailabilityStatus.CheckNullOrEmptyString();
                    dest.ShortSellingTransactions = src.ShortSellingTransactions.CheckNullOrEmptyString();
                    dest.OptionsTrading = src.OptionsTrading.CheckNullOrEmptyString();
                    dest.LeverageLongShort = src.LeverageLongShort.CheckNullOrEmptyString();
                    dest.PublicLimitedCompanyName = src.PublicLimitedCompanyName.CheckNullOrEmptyString();
                    dest.Dealer = src.Dealer.CheckNullOrEmptyString();
                    dest.Depository = src.Depository.CheckNullOrEmptyString();
                    dest.StockManager = src.StockManager.CheckNullOrEmptyString();
                    dest.InvestmentStrategy = src.InvestmentStrategy.CheckNullOrEmptyString();
                    dest.SubscriptionFee = src.SubscriptionFee.CheckNullOrEmptyString();
                    dest.RedemptionFee = src.RedemptionFee.CheckNullOrEmptyString();
                    dest.SubscriptionNAVDate = src.SubscriptionNAVDate.CheckNullOrEmptyString();
                    dest.RedemptionNAVDate = src.RedemptionNAVDate.CheckNullOrEmptyString();
                    dest.RedemptionDepositDate = src.RedemptionDepositDate.CheckNullOrEmptyString();
                    dest.MinimumSingleInvestmentAmount = src.MinimumSingleInvestmentAmount.CheckNullOrEmptyString();
                    dest.MinimumRegularInvestmentAmount = src.MinimumRegularInvestmentAmount.CheckNullOrEmptyString();
                    dest.MinimumIrregularInvestmentAmount = src.MinimumIrregularInvestmentAmount.CheckNullOrEmptyString();
                    dest.BankRelatedInstructions = src.BankRelatedInstructions.CheckNullOrEmptyString();
                    dest.DividendDistributionFrequency = src.DividendDistributionFrequency.CheckNullOrEmptyString();

                    dest.TotalManagementFee = src.TotalManagementFee.FormatDecimalNumber(needPercent: true);

                    #region 上、下架時間

                    var cultureInfo = new CultureInfo("zh-TW");
                    string dateFormat = "yyyy/MM/dd";
                    if (DateTime.TryParseExact(src.ListingDate, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out DateTime listingDate))
                    {
                        dest.ListingDate = listingDate.ToString(dateFormat);
                    }
                    else
                    {
                        dest.ListingDate = src.ListingDate.CheckNullOrEmptyString();
                    }

                    if (DateTime.TryParseExact(src.DelistingDate, "yyyyMMdd", cultureInfo, DateTimeStyles.None, out DateTime delistingDate))
                    {
                        dest.DelistingDate = delistingDate.ToString(dateFormat);
                    }
                    else
                    {
                        dest.DelistingDate = src.DelistingDate.CheckNullOrEmptyString();
                    }

                    #endregion 上、下架時間
                });

            var result = product.Adapt<EtfDetail>(config);

            return result;
        }

        /// <summary>
        /// 近一年市價走勢
        /// </summary>
        /// <returns></returns>
        private List<EtfPriceHistory> GetMarketPriceWithOverPastYear()
        {
            string sql = """
                SELECT [NetAssetValueDate], [MarketPrice]
                FROM [Sysjust_Nav_ETF] WITH (NOLOCK)
                WHERE [FirstBankCode] = @ETFId AND [NetAssetValueDate] >= DATEADD(year, -1, GETDATE())
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfNav>(sql, param, CommandType.Text)?.ToList();
            var config = new TypeAdapterConfig();
            config.ForType<EtfNav, EtfPriceHistory>()
                .AfterMapping((src, dest) =>
                {
                    dest.NetAssetValueDate = DateTimeExtensions.FormatDate(src.NetAssetValueDate);
                    dest.MarketPrice = src.MarketPrice.RoundingValue();
                });

            var result = collection.Adapt<List<EtfPriceHistory>>(config);
            return result;
        }

        /// <summary>
        /// 近一年淨值走勢
        /// </summary>
        /// <returns></returns>
        private List<EtfPriceHistory> GetNetWorthWithOverPastYear()
        {
            string sql = """
                SELECT [NetAssetValueDate], [NetAssetValue]
                FROM [Sysjust_Nav_ETF] WITH (NOLOCK)
                WHERE [FirstBankCode] = @ETFId AND [NetAssetValueDate] >= DATEADD(year, -1, GETDATE())
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfNav>(sql, param, CommandType.Text)?.ToList();
            var config = new TypeAdapterConfig();
            config.ForType<EtfNav, EtfPriceHistory>()
                .AfterMapping((src, dest) =>
                {
                    dest.NetAssetValueDate = DateTimeExtensions.FormatDate(src.NetAssetValueDate);
                    dest.NetAssetValue = src.NetAssetValue.RoundingValue();
                });

            var result = collection.Adapt<List<EtfPriceHistory>>(config);
            return result;
        }

        /// <summary>
        /// 取得同類型ETF排行
        /// </summary>
        /// <returns></returns>
        private List<EtfTypeRanking> GetSameTypeETFRank()
        {
            string sqlQuery = """
                SELECT TOP 12 [FirstBankCode]
                    ,[ETFName]
                    ,[SixMonthReturnNetValueOriginalCurrency]
                    ,[NetAssetValue]
                    ,[NetAssetValueDate]
                FROM [vw_BasicETF]
                ORDER BY [SixMonthReturnNetValueOriginalCurrency] DESC
                """;
            var collection = DbManager.Custom.ExecuteIList<BasicEtfDto>(sqlQuery, null, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<BasicEtfDto, EtfTypeRanking>()
                .AfterMapping((src, dest) =>
                {
                    dest.ETFName = src.ETFName.Normalize(NormalizationForm.FormKC);
                    dest.SixMonthReturnNetValueOriginalCurrency = src.SixMonthReturnNetValueOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.SixMonthReturnNetValueOriginalCurrencyStyle = src.SixMonthReturnNetValueOriginalCurrency.DecimalNumberToStyle();
                    dest.NetAssetValue = src.NetAssetValue.FormatDecimalNumber(4);
                });

            var result = collection.Adapt<List<EtfTypeRanking>>(config);

            return result;
        }

        /// <summary>
        /// 近一年指數績效走勢
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public (List<EtfHistoryNetWorth>, string) GetGlobalIndexWithCloseYear(string etfId, string startdate, string enddate)
        {
            string sql = """
                SELECT Format([DataDate],'yyyy/MM/dd') NetAssetValueDate, [ChangePercentage] AS NetAssetValue
                FROM [Sysjust_GlobalIndex_History] WITH (NOLOCK)
                WHERE [IndexCode] =
                    (
                        SELECT [ReferenceIndexID]
                        FROM [vw_BasicETF]
                        WHERE [FirstBankCode] = @ETFId
                    )
                    AND [DataDate] BETWEEN @StartDate AND @EndDate
                """;
            var param = new { ETFId = etfId, StartDate = startdate, EndDate = enddate };
            List<EtfHistoryNetWorth> etfCloseYearNetValue = DbManager.Custom.ExecuteIList<EtfHistoryNetWorth>(sql, param, CommandType.Text)?.ToList();

            sql = """
                SELECT [StockIndexName]
                FROM [vw_BasicETF]
                WHERE [FirstBankCode] = @ETFid
                """;
            string indicator = DbManager.Custom.Execute<string>(sql, param, CommandType.Text);
            return (etfCloseYearNetValue, indicator);
        }

        /// <summary>
        /// 取得近30日市價/淨值
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        private List<EtfNav> GetThrityDaysNav()
        {
            string sqlQuery = """
                SELECT [FirstBankCode],[ExchangeCode],[NetAssetValueDate],[MarketPrice],[NetAssetValue],[RowNumber] FROM (
                    SELECT [FirstBankCode],[ExchangeCode],[NetAssetValueDate],[MarketPrice],[NetAssetValue],ROW_NUMBER() OVER(PARTITION BY [FirstBankCode] ORDER BY [NetAssetValueDate] DESC) AS [RowNumber]
                    FROM [dbo].[Sysjust_Nav_ETF] WITH (NOLOCK)
                ) T1
                WHERE [FirstBankCode] = @ETFId
                    AND [RowNumber] < 31
                """;
            var param = new { ETFId = this.ETFId };
            var result = DbManager.Custom.ExecuteIList<EtfNav>(sqlQuery, param, CommandType.Text)?.ToList();
            return result;
        }

        /// <summary>
        /// 取得近五年報酬
        /// </summary>
        /// <returns></returns>
        private List<EtfReferenceIndexAnnualReturn> GetAnnualReturn()
        {
            string sqlQuery = """
                SELECT TOP 5 *
                FROM [dbo].[Sysjust_Return_ETF_2] WITH (NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [DataDate] DESC
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfReturn2>(sqlQuery, param, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<EtfReturn2, EtfReferenceIndexAnnualReturn>()
                .AfterMapping((src, dest) =>
                {
                    dest.Year = src.DataDate.Value.Year;
                    dest.NetValueAnnualReturnOriginalCurrency = src.NetValueAnnualReturnOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.NetValueAnnualReturnOriginalCurrencyStyle = src.NetValueAnnualReturnOriginalCurrency.DecimalNumberToStyle();
                    dest.ReferenceIndexAnnualReturn = src.ReferenceIndexAnnualReturn.FormatDecimalNumber(needPercent: true);
                    dest.ReferenceIndexAnnualReturnStyle = src.ReferenceIndexAnnualReturn.DecimalNumberToStyle();
                    dest.Difference = (src.NetValueAnnualReturnOriginalCurrency - src.ReferenceIndexAnnualReturn).FormatDecimalNumber();
                });

            var result = collection.Adapt<List<EtfReferenceIndexAnnualReturn>>(config);

            return result;
        }

        /// <summary>
        /// 取得近一年各月報酬率
        /// </summary>
        /// <returns></returns>
        private List<EtfReferenceIndexMonthlyReturn> GetNetWortMonthlyReturn()
        {
            string sqlQuery = """
                SELECT TOP 12 *
                FROM [dbo].[Sysjust_Return_ETF_3] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [DataDate] DESC
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfReturn3>(sqlQuery, param, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<EtfReturn3, EtfReferenceIndexMonthlyReturn>()
                .AfterMapping((src, dest) =>
                {
                    dest.Month = DateTimeExtensions.FormatDate(src.DataDate.Value, "yyyy/MM");
                    dest.NetValueMonthlyReturnOriginalCurrency = src.NetValueMonthlyReturnOriginalCurrency.FormatDecimalNumber(needPercent: true);
                    dest.NetValueMonthlyReturnOriginalCurrencyStyle = src.NetValueMonthlyReturnOriginalCurrency.DecimalNumberToStyle();
                    dest.ReferenceIndexMonthlyReturn = src.ReferenceIndexMonthlyReturn.FormatDecimalNumber(needPercent: true);
                    dest.ReferenceIndexMonthlyReturnStyle = src.ReferenceIndexMonthlyReturn.DecimalNumberToStyle();
                    dest.Difference = (src.NetValueMonthlyReturnOriginalCurrency - src.ReferenceIndexMonthlyReturn).FormatDecimalNumber();
                });

            var result = collection.Adapt<List<EtfReferenceIndexMonthlyReturn>>(config);

            return result;
        }

        /// <summary>
        /// 取得一銀買賣價資料來源
        /// </summary>
        /// <returns></returns>
        private EtfTradingPrice GetBuyAndSellPriceData()
        {
            var sql = """
                WITH [FundETFCTE] AS(
                    SELECT *
                    FROM (
                        SELECT [BankProductCode]
                            ,[ETFCurrency]
                            ,CAST([BankBuyPrice] AS DECIMAL(12, 4)) AS [BankBuyPrice]
                            ,CAST([BankSellPrice] AS DECIMAL(12, 4)) AS [BankSellPrice]
                            ,CAST(FORMAT( CONVERT(DATE, CONVERT(NVARCHAR(8), [PriceBaseDate] + 19110000), 112), 'yyyy/MM/dd' ) AS Date) AS [PriceBaseDate]
                            ,[ProductName]
                            ,[DataDate]
                            , ROW_NUMBER() OVER(PARTITION BY [BankProductCode] ORDER BY [PriceBaseDate] DESC) AS [RowNumber]
                        FROM [dbo].[FUND_ETF] WITH (NOLOCK)
                    ) T1
                    WHERE [RowNumber] < 3
                ), [YearPriceCTE] AS (
                    SELECT [BankProductCode]
                        ,MAX([BankBuyPrice]) AS [MaxBuyPrice]
                        ,MIN([BankBuyPrice]) AS [MinBuyPrice]
                        ,MAX([BankSellPrice]) AS [MaxSellPrice]
                        ,MIN([BankSellPrice]) AS [MinSellPrice]
                    FROM (
                        SELECT [BankProductCode]
                            ,CAST(FORMAT( CONVERT(DATE, CONVERT(NVARCHAR(8), [PriceBaseDate] + 19110000), 112), 'yyyy/MM/dd' ) AS Date) AS [PriceBaseDate]
                            ,CAST([BankBuyPrice] AS DECIMAL(12, 4)) AS [BankBuyPrice]
                            ,CAST([BankSellPrice] AS DECIMAL(12, 4)) AS [BankSellPrice]
                        FROM [dbo].[FUND_ETF] WITH (NOLOCK)
                    ) T1
                    WHERE [PriceBaseDate] >= DATEADD(YEAR, -1, GETDATE())
                    GROUP BY [BankProductCode]
                )

                SELECT
                     [Table].[BankProductCode]
                    ,[Table].[ETFCurrency]
                    ,[Table].[BankBuyPrice]
                    ,[Table].[BankSellPrice]
                    ,[Table].[PriceBaseDate]
                    ,[Table].[ProductName]
                    ,[PreviousTable].[BankBuyPrice]  AS [PreviousBuyPrice]
                    ,[PreviousTable].[BankSellPrice] AS [PreviousSellPrice]
                    ,[PreviousTable].[PriceBaseDate] AS [PreviousPriceBaseDate]
                    ,CASE WHEN [PreviousTable].[BankBuyPrice] IS NOT NULL
                        THEN [Table].[BankBuyPrice] - [PreviousTable].[BankBuyPrice]
                        ELSE NULL
                    END [BuyPriceChange]
                    ,CASE WHEN [PreviousTable].[BankBuyPrice] IS NOT NULL
                        THEN ( [Table].[BankBuyPrice] - [PreviousTable].[BankBuyPrice] ) / [PreviousTable].[BankBuyPrice]
                        ELSE NULL
                    END [BuyPriceChangePercentage]
                    ,CASE WHEN [PreviousTable].[BankSellPrice] IS NOT NULL
                        THEN [Table].[BankSellPrice] - [PreviousTable].[BankSellPrice]
                        ELSE NULL
                    END [SellPriceChange]
                    ,CASE WHEN [PreviousTable].[BankSellPrice] IS NOT NULL
                        THEN ( [Table].[BankSellPrice] - [PreviousTable].[BankSellPrice] ) / [PreviousTable].[BankSellPrice]
                        ELSE NULL
                    END [SellPriceChangePercentage]
                    ,[MaxBuyPrice]
                    ,[MinBuyPrice]
                    ,[MaxSellPrice]
                    ,[MinSellPrice]
                FROM
                (
                	SELECT * FROM [FundETFCTE]
                	WHERE [RowNumber] = 1
                ) AS [Table]
                LEFT JOIN
                (
                	SELECT * FROM [FundETFCTE]
                	WHERE [RowNumber] = 2
                ) AS [PreviousTable]
                ON [PreviousTable].[BankProductCode] = [Table].[BankProductCode]
                LEFT JOIN
                (
                    SELECT * FROM [YearPriceCTE]
                ) AS [YearPriceTable]
                ON [YearPriceTable].[BankProductCode] = [Table].[BankProductCode]
                WHERE [Table].[BankProductCode] = @ETFId
                """;
            var param = new { ETFId = this.ETFId };
            var result = DbManager.Custom.ExecuteIList<EtfTradingPrice>(sql, param, CommandType.Text)
                .Select(r =>
                {
                    r.BuyPriceChangeStyle = r.BuyPriceChange.DecimalNumberToStyle();
                    r.BuyPriceChangePercentageStyle = r.BuyPriceChangePercentage.DecimalNumberToStyle();
                    r.SellPriceChangeStyle = r.SellPriceChange.DecimalNumberToStyle();
                    r.SellPriceChangePercentageStyle = r.SellPriceChangePercentage.DecimalNumberToStyle();
                    return r;
                });

            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 取得歷史買賣盤
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        public (List<EtfHistoryPrice>, string) GetHistoryPrice(string etfId, string startdate, string enddate)
        {
            var sql = """
                SELECT [BankBuyPrice]
                    ,[BankSellPrice]
                    ,FORMAT( CONVERT(DATE, CONVERT(NVARCHAR(8), [PriceBaseDate] + 19110000), 112), 'yyyy/MM/dd' ) AS [Date]
                FROM [FUND_ETF] WITH (NOLOCK)
                WHERE [BankProductCode] = @ETFId
                    AND [PriceBaseDate] BETWEEN FORMAT(CONVERT(DATE, @StartDate), 'yyyyMMdd') -19110000 AND  FORMAT(CONVERT(DATE, @EndDate), 'yyyyMMdd') -19110000
                ORDER BY [PriceBaseDate]
                """;
            var param = new { ETFId = etfId, StartDate = startdate, EndDate = enddate };
            List<EtfHistoryPrice> etfScaleMove = DbManager.Custom.ExecuteIList<EtfHistoryPrice>(sql, param, CommandType.Text)?.ToList();

            sql = """
                SELECT [QuoteCurrency]
                FROM [vw_BasicETF]
                WHERE [FirstBankCode] = @ETFId
                """;
            string indicator = DbManager.Custom.Execute<string>(sql, param, CommandType.Text);
            return (etfScaleMove, indicator);
        }

        /// <summary>
        /// 取得近30日報價
        /// </summary>
        public List<EtfTradingPrice> GetThrityDaysBuyAndSellPrice()
        {
            var sql = """
                WITH [FundETFCTE] AS(
                    SELECT *
                    FROM (
                        SELECT [BankProductCode]
                            ,[ETFCurrency]
                            ,CAST([BankBuyPrice] AS DECIMAL(12, 4)) AS [BankBuyPrice]
                            ,CAST([BankSellPrice] AS DECIMAL(12, 4)) AS [BankSellPrice]
                            ,CAST(FORMAT( CONVERT(DATE, CONVERT(NVARCHAR(8), [PriceBaseDate] + 19110000), 112), 'yyyy/MM/dd' ) AS Date) AS [PriceBaseDate]
                            ,[ProductName]
                            ,[DataDate]
                            , ROW_NUMBER() OVER(PARTITION BY [BankProductCode] ORDER BY [PriceBaseDate] DESC) AS [RowNumber]
                        FROM [dbo].[FUND_ETF] WITH (NOLOCK)
                    ) T1
                )

                SELECT *
                FROM [FundETFCTE]
                WHERE [BankProductCode] = @ETFId AND [RowNumber] < 31
                """;
            var param = new { ETFId = this.ETFId };
            var result = DbManager.Custom.ExecuteIList<EtfTradingPrice>(sql, param, CommandType.Text);
            return result?.ToList();
        }

        /// <summary>
        /// 取得產業持股狀況
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        public List<EtfIndustryHolding> GetETFIndustryPercent(string etfId)
        {
            string sql = """
                SELECT [FirstBankCode]
                      ,[Date]
                      ,[IndustryName]
                      ,[Percentage]
                      ,[Currency]
                      ,[Amount]
                FROM [dbo].[Sysjust_Holding_ETF] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [Percentage] DESC
                """;

            var param = new { ETFId = etfId };
            var collection = DbManager.Custom.ExecuteIList<EtfHolding>(sql, param, CommandType.Text);
            var config = new TypeAdapterConfig();
            config.ForType<EtfHolding, EtfIndustryHolding>()
                .AfterMapping((src, dest) =>
                {
                    dest.Date = DateTimeExtensions.FormatDate(src.Date.Value, "yyyy/MM/dd");
                });

            var result = collection.Adapt<List<EtfIndustryHolding>>(config);
            return result;
        }

        /// <summary>
        /// 取得區域持股狀況
        /// </summary>
        /// <param name="etfId"></param>
        /// <returns></returns>
        public List<EtfRegionHolding> GetETFRegionPercent(string etfId)
        {
            string sql = """
                SELECT [FirstBankCode]
                      ,[Date]
                      ,[RegionName]
                      ,[Percentage]
                      ,[Amount]
                FROM [dbo].[Sysjust_Holding_ETF_2] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [Percentage] DESC
                """;

            var param = new { ETFId = etfId };
            var collection = DbManager.Custom.ExecuteIList<EtfHolding2>(sql, param, CommandType.Text);
            var config = new TypeAdapterConfig();
            config.ForType<EtfHolding2, EtfRegionHolding>()
                .AfterMapping((src, dest) =>
                {
                    dest.Date = DateTimeExtensions.FormatDate(src.Date.Value, "yyyy/MM/dd");
                });

            var result = collection.Adapt<List<EtfRegionHolding>>(config);
            return result;
        }

        /// <summary>
        /// 取得前十大持股
        /// </summary>
        /// <returns></returns>
        private List<EtfStrockHolding> GetETFStockHolding()
        {
            string sql = """
                SELECT TOP 10 [FirstBankCode]
                      ,[Date]
                      ,[ETFCode]
                      ,[StockCode]
                      ,[StockName]
                      ,[Percentage]
                      ,[NumberofSharesHeld]
                FROM [dbo].[Sysjust_Holding_ETF_3] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [Percentage] DESC
                """;

            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfHolding3>(sql, param, CommandType.Text);
            var config = new TypeAdapterConfig();
            config.ForType<EtfHolding3, EtfStrockHolding>()
                .AfterMapping((src, dest) =>
                {
                    dest.Date = DateTimeExtensions.FormatDate(src.Date.Value, "yyyy/MM/dd");
                    dest.Percentage = src.Percentage.FormatDecimalNumber(needPercent: true);
                    dest.NumberofSharesHeld = src.NumberofSharesHeld.HasValue ? src.NumberofSharesHeld.Value.ToString("N0") : string.Empty;
                });

            var result = collection.Adapt<List<EtfStrockHolding>>(config);
            return result;
        }

        /// <summary>
        /// 取得風險指標
        /// </summary>
        /// <returns></returns>
        private EtfRiskIndicator GetETFRiskIndicator()
        {
            string sqlQuery = """
                SELECT *
                FROM [Sysjust_Risk_ETF_2] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                """;
            var param = new { ETFId = this.ETFId };
            var risk = DbManager.Custom.Execute<EtfRisk2>(sqlQuery, param, CommandType.Text);

            var config = new TypeAdapterConfig();
            config.ForType<EtfRisk2, EtfRiskIndicator>()
                .AfterMapping((src, dest) =>
                {
                    #region 指標

                    dest.Date = DateTimeExtensions.FormatDate(src.Date.Value, "yyyy/MM/dd");
                    dest.SixMonthStandardDeviation = src.SixMonthStandardDeviation.FormatDecimalNumber(needAbs: false);
                    dest.OneYearStandardDeviation = src.OneYearStandardDeviation.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearStandardDeviation = src.ThreeYearStandardDeviation.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearStandardDeviation = src.FiveYearStandardDeviation.FormatDecimalNumber(needAbs: false);
                    dest.TenYearStandardDeviation = src.TenYearStandardDeviation.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthSharpe = src.SixMonthSharpe.FormatDecimalNumber(needAbs: false);
                    dest.OneYearSharpe = src.OneYearSharpe.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearSharpe = src.ThreeYearSharpe.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearSharpe = src.FiveYearSharpe.FormatDecimalNumber(needAbs: false);
                    dest.TenYearSharpe = src.TenYearSharpe.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthAlpha = src.SixMonthAlpha.FormatDecimalNumber(needAbs: false);
                    dest.OneYearAlpha = src.OneYearAlpha.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearAlpha = src.ThreeYearAlpha.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearAlpha = src.FiveYearAlpha.FormatDecimalNumber(needAbs: false);
                    dest.TenYearAlpha = src.TenYearAlpha.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthBeta = src.SixMonthBeta.FormatDecimalNumber(needAbs: false);
                    dest.OneYearBeta = src.OneYearBeta.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearBeta = src.ThreeYearBeta.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearBeta = src.FiveYearBeta.FormatDecimalNumber(needAbs: false);
                    dest.TenYearBeta = src.TenYearBeta.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthRsquared = src.SixMonthRsquared.FormatDecimalNumber(needAbs: false);
                    dest.OneYearRsquared = src.OneYearRsquared.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearRsquared = src.ThreeYearRsquared.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearRsquared = src.FiveYearRsquared.FormatDecimalNumber(needAbs: false);
                    dest.TenYearRsquared = src.TenYearRsquared.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthCorrelationCoefficientIndex = src.SixMonthCorrelationCoefficientIndex.FormatDecimalNumber(needAbs: false);
                    dest.OneYearCorrelationCoefficientIndex = src.OneYearCorrelationCoefficientIndex.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearCorrelationCoefficientIndex = src.ThreeYearCorrelationCoefficientIndex.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearCorrelationCoefficientIndex = src.FiveYearCorrelationCoefficientIndex.FormatDecimalNumber(needAbs: false);
                    dest.TenYearCorrelationCoefficientIndex = src.TenYearCorrelationCoefficientIndex.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthTrackingError = src.SixMonthTrackingError.FormatDecimalNumber(needAbs: false);
                    dest.OneYearTrackingError = src.OneYearTrackingError.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearTrackingError = src.ThreeYearTrackingError.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearTrackingError = src.FiveYearTrackingError.FormatDecimalNumber(needAbs: false);
                    dest.TenYearTrackingError = src.TenYearTrackingError.FormatDecimalNumber(needAbs: false);
                    dest.SixMonthVariance = src.SixMonthVariance.FormatDecimalNumber(needAbs: false);
                    dest.OneYearVariance = src.OneYearVariance.FormatDecimalNumber(needAbs: false);
                    dest.ThreeYearVariance = src.ThreeYearVariance.FormatDecimalNumber(needAbs: false);
                    dest.FiveYearVariance = src.FiveYearVariance.FormatDecimalNumber(needAbs: false);
                    dest.TenYearVariance = src.TenYearVariance.FormatDecimalNumber(needAbs: false);

                    #endregion 指標

                    #region 樣式

                    dest.SixMonthStandardDeviationStyle = src.SixMonthStandardDeviation.DecimalNegativeStyle();
                    dest.OneYearStandardDeviationStyle = src.OneYearStandardDeviation.DecimalNegativeStyle();
                    dest.ThreeYearStandardDeviationStyle = src.ThreeYearStandardDeviation.DecimalNegativeStyle();
                    dest.FiveYearStandardDeviationStyle = src.FiveYearStandardDeviation.DecimalNegativeStyle();
                    dest.TenYearStandardDeviationStyle = src.TenYearStandardDeviation.DecimalNegativeStyle();
                    dest.SixMonthSharpeStyle = src.SixMonthSharpe.DecimalNegativeStyle();
                    dest.OneYearSharpeStyle = src.OneYearSharpe.DecimalNegativeStyle();
                    dest.ThreeYearSharpeStyle = src.ThreeYearSharpe.DecimalNegativeStyle();
                    dest.FiveYearSharpeStyle = src.FiveYearSharpe.DecimalNegativeStyle();
                    dest.TenYearSharpeStyle = src.TenYearSharpe.DecimalNegativeStyle();
                    dest.SixMonthAlphaStyle = src.SixMonthAlpha.DecimalNegativeStyle();
                    dest.OneYearAlphaStyle = src.OneYearAlpha.DecimalNegativeStyle();
                    dest.ThreeYearAlphaStyle = src.ThreeYearAlpha.DecimalNegativeStyle();
                    dest.FiveYearAlphaStyle = src.FiveYearAlpha.DecimalNegativeStyle();
                    dest.TenYearAlphaStyle = src.TenYearAlpha.DecimalNegativeStyle();
                    dest.SixMonthBetaStyle = src.SixMonthBeta.DecimalNegativeStyle();
                    dest.OneYearBetaStyle = src.OneYearBeta.DecimalNegativeStyle();
                    dest.ThreeYearBetaStyle = src.ThreeYearBeta.DecimalNegativeStyle();
                    dest.FiveYearBetaStyle = src.FiveYearBeta.DecimalNegativeStyle();
                    dest.TenYearBetaStyle = src.TenYearBeta.DecimalNegativeStyle();
                    dest.SixMonthRsquaredStyle = src.SixMonthRsquared.DecimalNegativeStyle();
                    dest.OneYearRsquaredStyle = src.OneYearRsquared.DecimalNegativeStyle();
                    dest.ThreeYearRsquaredStyle = src.ThreeYearRsquared.DecimalNegativeStyle();
                    dest.FiveYearRsquaredStyle = src.FiveYearRsquared.DecimalNegativeStyle();
                    dest.TenYearRsquaredStyle = src.TenYearRsquared.DecimalNegativeStyle();
                    dest.SixMonthCorrelationCoefficientIndexStyle = src.SixMonthCorrelationCoefficientIndex.DecimalNegativeStyle();
                    dest.OneYearCorrelationCoefficientIndexStyle = src.OneYearCorrelationCoefficientIndex.DecimalNegativeStyle();
                    dest.ThreeYearCorrelationCoefficientIndexStyle = src.ThreeYearCorrelationCoefficientIndex.DecimalNegativeStyle();
                    dest.FiveYearCorrelationCoefficientIndexStyle = src.FiveYearCorrelationCoefficientIndex.DecimalNegativeStyle();
                    dest.TenYearCorrelationCoefficientIndexStyle = src.TenYearCorrelationCoefficientIndex.DecimalNegativeStyle();
                    dest.SixMonthTrackingErrorStyle = src.SixMonthTrackingError.DecimalNegativeStyle();
                    dest.OneYearTrackingErrorStyle = src.OneYearTrackingError.DecimalNegativeStyle();
                    dest.ThreeYearTrackingErrorStyle = src.ThreeYearTrackingError.DecimalNegativeStyle();
                    dest.FiveYearTrackingErrorStyle = src.FiveYearTrackingError.DecimalNegativeStyle();
                    dest.TenYearTrackingErrorStyle = src.TenYearTrackingError.DecimalNegativeStyle();
                    dest.SixMonthVarianceStyle = src.SixMonthVariance.DecimalNegativeStyle();
                    dest.OneYearVarianceStyle = src.OneYearVariance.DecimalNegativeStyle();
                    dest.ThreeYearVarianceStyle = src.ThreeYearVariance.DecimalNegativeStyle();
                    dest.FiveYearVarianceStyle = src.FiveYearVariance.DecimalNegativeStyle();
                    dest.TenYearVarianceStyle = src.TenYearVariance.DecimalNegativeStyle();

                    #endregion 樣式
                });

            var result = risk.Adapt<EtfRiskIndicator>(config);
            return result;
        }

        /// <summary>
        /// 取得風險像線圖
        /// </summary>
        /// <param name="etfId"></param>
        /// <param name="selectType"></param>
        /// <returns></returns>
        public List<EtfRiskGraph> GetRiskindicatorsGraph(string etfId, string selectType)
        {
            var param = new { ETFId = etfId, condition = selectType };
            var result = DbManager.Custom.ExecuteIList<EtfRiskGraph>("sp_ETFRiskindicatorsPicture", param, CommandType.StoredProcedure)
                .Select(r =>
                {
                    r.ETFName = r.ETFName.Normalize(NormalizationForm.FormKC);
                    return r;
                });

            return result?.ToList();
        }

        /// <summary>
        /// 取得年報酬率比較表
        /// </summary>
        /// <returns></returns>
        private List<EtfYearReturnCompare> GetETFYearReturnCompare()
        {
            string sql = """
                SELECT TOP 5 *
                FROM [dbo].[Sysjust_Risk_ETF] WITH(NOLOCK)
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [Date] DESC
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfRisk>(sql, param, CommandType.Text);
            var config = new TypeAdapterConfig();
            config.ForType<EtfRisk, EtfYearReturnCompare>()
                .AfterMapping((src, dest) =>
                {
                    dest.Year = src.Date.Value.Year;
                    dest.AnnualizedStandardDeviationMarketPrice = src.AnnualizedStandardDeviationMarketPrice.FormatDecimalNumber(needAbs: false);
                    dest.BetaMarketPrice = src.BetaMarketPrice.FormatDecimalNumber(needAbs: false);
                    dest.SharpeMarketPrice = src.SharpeMarketPrice.FormatDecimalNumber(needAbs: false);
                    dest.InformationRatioMarketPrice = src.InformationRatioMarketPrice.FormatDecimalNumber(needAbs: false);
                    dest.JensenIndexMarketPrice = src.JensenIndexMarketPrice.FormatDecimalNumber(needAbs: false);
                    dest.TreynorIndexMarketPrice = src.TreynorIndexMarketPrice.FormatDecimalNumber(needAbs: false);

                    #region 樣式

                    dest.AnnualizedStandardDeviationMarketPriceStyle = src.AnnualizedStandardDeviationMarketPrice.DecimalNegativeStyle();
                    dest.BetaMarketPriceStyle = src.BetaMarketPrice.DecimalNegativeStyle();
                    dest.SharpeMarketPriceStyle = src.SharpeMarketPrice.DecimalNegativeStyle();
                    dest.InformationRatioMarketPriceStyle = src.InformationRatioMarketPrice.DecimalNegativeStyle();
                    dest.JensenIndexMarketPriceStyle = src.JensenIndexMarketPrice.DecimalNegativeStyle();
                    dest.TreynorIndexMarketPriceStyle = src.TreynorIndexMarketPrice.DecimalNegativeStyle();

                    #endregion 樣式
                });

            var result = collection.Adapt<List<EtfYearReturnCompare>>(config);
            return result;
        }

        /// <summary>
        /// 取得配息紀錄
        /// </summary>
        /// <returns></returns>
        private Dictionary<int?, List<EtfDividendRecord>> GetDividendRecords()
        {
            string sql = """
                WITH [DividendCTE] AS(
                	SELECT [FirstBankCode]
                        ,[ETFCode]
                        ,[ExDividendDate]
                        ,[RecordDate]
                        ,[PaymentDate]
                        ,[TotalDividendAmount]
                        ,[DividendFrequency]
                        ,[ShortTermCapitalGains]
                        ,[LongTermCapitalGains]
                        ,[Currency]
                    FROM [dbo].[Sysjust_Dividend_ETF] WITH (NOLOCK)
                	UNION
                	SELECT [FirstBankCode]
                        ,[ETFCode]
                        ,[ExDividendDate]
                        ,[RecordDate]
                        ,[PaymentDate]
                        ,[TotalDividendAmount]
                        ,[DividendFrequency]
                        ,[ShortTermCapitalGains]
                        ,[LongTermCapitalGains]
                        ,[Currency]
                    FROM [dbo].[Sysjust_Dividend_ETF_History] WITH (NOLOCK)
                )

                SELECT *
                FROM [DividendCTE]
                WHERE [FirstBankCode] = @ETFId
                ORDER BY [ExDividendDate] DESC
                """;
            var param = new { ETFId = this.ETFId };
            var collection = DbManager.Custom.ExecuteIList<EtfDividend>(sql, param, CommandType.Text);
            var config = new TypeAdapterConfig();
            config.ForType<EtfDividend, EtfDividendRecord>()
                .AfterMapping((src, dest) =>
                {
                    dest.Year = src.ExDividendDate.HasValue ? src.ExDividendDate.Value.Year : null;
                    dest.ExDividendDate = DateTimeExtensions.FormatDate(src.ExDividendDate).CheckNullOrEmptyString();
                    dest.RecordDate = DateTimeExtensions.FormatDate(src.RecordDate).CheckNullOrEmptyString();
                    dest.PaymentDate = DateTimeExtensions.FormatDate(src.PaymentDate).CheckNullOrEmptyString();
                    dest.TotalDividendAmount = src.TotalDividendAmount.FormatDecimalNumber(4);
                    dest.ShortTermCapitalGains = src.ShortTermCapitalGains.FormatDecimalNumber(4);
                    dest.LongTermCapitalGains = src.LongTermCapitalGains.FormatDecimalNumber(4);
                });

            var result = collection.Adapt<List<EtfDividendRecord>>(config)
                .Where(i => i.Year != null)
                .GroupBy(item => item.Year)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );

            return result;
        }

        private void GetLastDividendRecord(EtfDetail basic, Dictionary<int?, List<EtfDividendRecord>> records)
        {
            if (basic == null)
            {
                return;
            }

            var dividendList = records.OrderByDescending(i => i.Key.Value).FirstOrDefault().Value;
            if (dividendList == null || !dividendList.Any())
            {
                return;
            }

            var lastDividend = dividendList.FirstOrDefault();

            if (lastDividend != null)
            {
                basic.ExDividendDate = lastDividend.ExDividendDate;
                basic.RecordDate = lastDividend.RecordDate;
                basic.PaymentDate = lastDividend.PaymentDate;
                basic.TotalDividendAmount = lastDividend.TotalDividendAmount;
                basic.DividendFrequency = lastDividend.DividendFrequency;
                basic.ShortTermCapitalGains = lastDividend.ShortTermCapitalGains;
                basic.LongTermCapitalGains = lastDividend.LongTermCapitalGains;
            }
        }

        /// <summary>
        /// 取得規模變動
        /// </summary>
        /// <returns></returns>
        public List<EtfScaleRecord> GetScalechange()
        {
            string sql = """
                WITH [FundsizeCTE] AS(
                	SELECT [FirstBankCode],[FundCode], FORMAT([ScaleDate],'yyyy/MM/dd') AS [ScaleDate] ,[ScaleMillions],[Currency]
                	FROM [dbo].[Sysjust_Fundsize_ETF] WITH (NOLOCK)
                    WHERE [FirstBankCode] = @ETFId
                )

                SELECT *
                FROM [FundsizeCTE]
                WHERE [ScaleDate] >= CAST(
                    CAST( YEAR(
                            DATEADD( YEAR, -2, (SELECT MAX([ScaleDate]) FROM [FundsizeCTE]) )
                        ) AS varchar(4)
                    ) + '-01-01' AS smalldatetime)
                ORDER BY [ScaleDate] DESC
                """;
            var param = new { ETFId = this.ETFId };

            var result = DbManager.Custom.ExecuteIList<EtfScaleRecord>(sql, param, CommandType.Text)/*?.ToList()*/
                .Select(r =>
                {
                    r.ScaleMillions = NumberExtensions.Rounding(r.ScaleMillions, 2);
                    return r;
                });

            return result?.ToList();
        }
    }
}