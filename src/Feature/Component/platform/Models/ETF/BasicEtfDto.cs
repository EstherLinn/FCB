using System;

namespace Feature.Wealth.Component.Models.ETF
{
    public class BasicEtfDto
    {
        #region MainTable

        /// <summary>
        /// 一銀產品代號
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 產品類型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 已撤銷核備
        /// </summary>
        public string WithdrawnApproval { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>

        public string AvailabilityStatus { get; set; }

        /// <summary>
        /// 上架時間
        /// </summary>
        public string ListingDate { get; set; }

        /// <summary>
        /// 下架時間
        /// </summary>
        public string DelistingDate { get; set; }

        /// <summary>
        /// 最低單筆投資金額
        /// </summary>
        public string MinimumSingleInvestmentAmount { get; set; }

        /// <summary>
        /// 最低定期定額投資金額
        /// </summary>
        public string MinimumRegularInvestmentAmount { get; set; }

        /// <summary>
        /// 最低定期不定額投資金額
        /// </summary>
        public string MinimumIrregularInvestmentAmount { get; set; }

        /// <summary>
        /// 申購手續費
        /// </summary>
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 贖回手續費
        /// </summary>
        public string RedemptionFee { get; set; }

        /// <summary>
        /// 申購淨值日
        /// </summary>
        public string SubscriptionNAVDate { get; set; }

        /// <summary>
        /// 贖回淨值日
        /// </summary>
        public string RedemptionNAVDate { get; set; }

        /// <summary>
        /// 贖回入款日
        /// </summary>
        public string RedemptionDepositDate { get; set; }

        /// <summary>
        /// 銀行相關說明
        /// </summary>
        public string BankRelatedInstructions { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendDistributionFrequency { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        public string RiskLevel { get; set; }

        #endregion MainTable

        #region Basic

        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實產品代號
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// ETF名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 交易所代碼
        /// </summary>
        public string ExchangeCode { get; set; }

        /// <summary>
        /// ETF英文名稱
        /// </summary>
        public string ETFEnglishName { get; set; }

        /// <summary>
        /// 交易所 ID
        /// </summary>
        public string ExchangeID { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        public string RegisteredLocation { get; set; }

        /// <summary>
        /// 放空交易
        /// </summary>
        public string ShortSellingTransactions { get; set; }

        /// <summary>
        /// 選擇權交易
        /// </summary>
        public string OptionsTrading { get; set; }

        /// <summary>
        /// 槓桿多空註記
        /// </summary>
        public string LeverageLongShort { get; set; }

        /// <summary>
        /// 經銷商
        /// </summary>
        public string Dealer { get; set; }

        /// <summary>
        /// 保管機構
        /// </summary>
        public string Depository { get; set; }

        /// <summary>
        /// 經理人
        /// </summary>
        public string StockManager { get; set; }

        /// <summary>
        /// 投資策略
        /// </summary>
        public string InvestmentStrategy { get; set; }

        /// <summary>
        /// ETF規模(百萬)
        /// </summary>
        public decimal ScaleMillions { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime EstablishmentDate { get; set; }

        /// <summary>
        /// 成立年資
        /// </summary>
        public int EstablishmentSeniority { get; set; }

        /// <summary>
        /// 投資標的 ID
        /// </summary>
        public int InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資標的 名稱
        /// </summary>
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 投資區域 ID
        /// </summary>
        public int InvestmentRegionID { get; set; }

        /// <summary>
        /// 投資區域 名稱
        /// </summary>
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 發行公司 ID
        /// </summary>
        public int PublicLimitedCompanyID { get; set; }

        /// <summary>
        /// 發行公司 名稱
        /// </summary>
        public string PublicLimitedCompanyName { get; set; }

        /// <summary>
        /// 投資風格 ID
        /// </summary>
        public int InvestmentStyleID { get; set; }

        /// <summary>
        /// 投資風格 名稱
        /// </summary>
        public string InvestmentStyleName { get; set; }

        /// <summary>
        /// 連結指數(指標指數)
        /// </summary>
        public string IndicatorIndex { get; set; }

        /// <summary>
        /// 總管理費用(%)
        /// </summary>
        public decimal TotalManagementFee { get; set; }

        /// <summary>
        /// 市價漲跌
        /// </summary>
        public decimal MarketPriceChange { get; set; }

        /// <summary>
        /// 市價漲跌幅
        /// </summary>
        public decimal MarketPriceChangePercentage { get; set; }

        /// <summary>
        /// 淨值漲跌
        /// </summary>
        public decimal NetAssetValueChange { get; set; }

        /// <summary>
        /// 淨值漲跌幅
        /// </summary>
        public decimal NetAssetValueChangePercentage { get; set; }

        /// <summary>
        /// 最高市價(年)
        /// </summary>
        public decimal HighestMarketPrice { get; set; }

        /// <summary>
        /// 最低市價(年)
        /// </summary>
        public decimal LowestMarketPrice { get; set; }

        /// <summary>
        /// 最高淨值(年)
        /// </summary>
        public decimal HighestNetAssetValue { get; set; }

        /// <summary>
        /// 最低淨值(年)
        /// </summary>
        public decimal LowestNetAssestValue { get; set; }

        #endregion Basic

        #region ReturnTable

        /// <summary>
        /// 價格(市價)
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public decimal NetAssetValue { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public DateTime? NetAssetValueDate { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        public string QuoteCurrency { get; set; }

        #region 報酬率 (淨值原幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值原幣)
        /// </summary>
        public decimal InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 月初至今 報酬率 (淨值原幣)
        /// </summary>
        public decimal MonthtoDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值原幣)
        /// </summary>
        public decimal YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一日 報酬率 (淨值原幣)
        /// </summary>
        public decimal DailyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一週 報酬率 (淨值原幣)
        /// </summary>
        public decimal WeeklyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值原幣)
        /// </summary>
        public decimal MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值原幣)
        /// </summary>
        public decimal ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值原幣)
        /// </summary>
        public decimal SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值原幣)
        /// </summary>
        public decimal OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值原幣)
        /// </summary>
        public decimal TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值原幣)
        /// </summary>
        public decimal ThreeYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 五年 報酬率 (淨值原幣)
        /// </summary>
        public decimal FiveYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 十年 報酬率 (淨值原幣)
        /// </summary>
        public decimal TenYearReturnNetValueOriginalCurrency { get; set; }

        #endregion 報酬率 (淨值原幣)

        #region 報酬率 (市價原幣)

        /// <summary>
        /// 成立至今 報酬率 (市價原幣)
        /// </summary>
        public decimal InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 月初至今 報酬率 (市價原幣)
        /// </summary>
        public decimal MonthtoDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價原幣)
        /// </summary>
        public decimal YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一日 報酬率 (市價原幣)
        /// </summary>
        public decimal DailyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一週 報酬率 (市價原幣)
        /// </summary>
        public decimal WeeklyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價原幣)
        /// </summary>
        public decimal MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價原幣)
        /// </summary>
        public decimal ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價原幣)
        /// </summary>
        public decimal SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價原幣)
        /// </summary>
        public decimal OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價原幣)
        /// </summary>
        public decimal TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價原幣)
        /// </summary>
        public decimal ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 五年 報酬率 (市價原幣)
        /// </summary>
        public decimal FiveYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 十年 報酬率 (市價原幣)
        /// </summary>
        public decimal TenYearReturnMarketPriceOriginalCurrency { get; set; }

        #endregion 報酬率 (市價原幣)

        #region 報酬率 (淨值台幣)

        /// <summary>
        /// 成立至今 報酬率 (淨值台幣)
        /// </summary>
        public decimal InceptionDateNetValueTWD { get; set; }

        /// <summary>
        /// 月初至今 報酬率 (淨值台幣)
        /// </summary>
        public decimal MonthtoDateNetValueTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (淨值台幣)
        /// </summary>
        public decimal YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一日 報酬率 (淨值台幣)
        /// </summary>
        public decimal DailyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一週 報酬率 (淨值台幣)
        /// </summary>
        public decimal WeeklyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (淨值台幣)
        /// </summary>
        public decimal MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (淨值台幣)
        /// </summary>
        public decimal ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值台幣)
        /// </summary>
        public decimal SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (淨值台幣)
        /// </summary>
        public decimal OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (淨值台幣)
        /// </summary>
        public decimal TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (淨值台幣)
        /// </summary>
        public decimal ThreeYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 五年 報酬率 (淨值台幣)
        /// </summary>
        public decimal FiveYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 十年 報酬率 (淨值台幣)
        /// </summary>
        public decimal TenYearReturnNetValueTWD { get; set; }

        #endregion 報酬率 (淨值台幣)

        #region 報酬率 (市價台幣)

        /// <summary>
        /// 成立至今 報酬率 (市價台幣)
        /// </summary>
        public decimal InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 月初至今 報酬率 (市價台幣)
        /// </summary>
        public decimal MonthtoDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 年初至今 報酬率 (市價台幣)
        /// </summary>
        public decimal YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一日 報酬率 (市價台幣)
        /// </summary>
        public decimal DailyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一週 報酬率 (市價台幣)
        /// </summary>
        public decimal WeeklyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月 報酬率 (市價台幣)
        /// </summary>
        public decimal MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月 報酬率 (市價台幣)
        /// </summary>
        public decimal ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月 報酬率 (市價台幣)
        /// </summary>
        public decimal SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年 報酬率 (市價台幣)
        /// </summary>
        public decimal OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年 報酬率 (市價台幣)
        /// </summary>
        public decimal TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年 報酬率 (市價台幣)
        /// </summary>
        public decimal ThreeYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 五年 報酬率 (市價台幣)
        /// </summary>
        public decimal FiveYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 十年 報酬率 (市價台幣)
        /// </summary>
        public decimal TenYearReturnMarketPriceTWD { get; set; }

        #endregion 報酬率 (市價台幣)

        #region 淨值風險

        /// <summary>
        /// Sharpe (淨值風險)
        /// </summary>
        public decimal SharpeNetValueRisk { get; set; }

        /// <summary>
        /// Beta (淨值風險)
        /// </summary>
        public decimal BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差 (淨值風險)
        /// </summary>
        public decimal AnnualizedStandardDeviationNetValueRisk { get; set; }

        #endregion 淨值風險

        #region 市價風險

        /// <summary>
        /// Sharpe (市價風險)
        /// </summary>
        public decimal SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// Beta (市價風險)
        /// </summary>
        public decimal BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 年化標準差 (市價風險)
        /// </summary>
        public decimal AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        #endregion 市價風險

        #region 交易狀況

        /// <summary>
        /// 折溢價
        /// </summary>
        public decimal DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        public decimal LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 十日均量
        /// </summary>
        public decimal TenDayAverageVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量
        /// </summary>
        public decimal LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        #endregion 交易狀況

        #endregion ReturnTable

        #region Currency

        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        public string Currency { get; set; }

        #endregion
    }
}