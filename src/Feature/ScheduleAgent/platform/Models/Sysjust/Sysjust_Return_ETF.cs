using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 各區間(市價/淨值)報酬率，檔案名稱：Sysjust-Return-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        ///嘉實代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 價格(市價)
        /// </summary>
        [Index(3)]
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        [Index(4)]
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 年化標準差(淨值風險)
        /// </summary>
        [Index(6)]
        public decimal? AnnualizedStandardDeviationNetValueRisk { get; set; }

        /// <summary>
        /// Sharpe(淨值風險)
        /// </summary>
        [Index(7)]
        public decimal? SharpeNetValueRisk { get; set; }

        /// <summary>
        /// beta(淨值風險)
        /// </summary>
        [Index(8)]
        public decimal? BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差(市價風險)
        /// </summary>
        [Index(9)]
        public decimal? AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        /// <summary>
        /// Sharpe(市價風險)
        /// </summary>
        [Index(10)]
        public decimal? SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// beta(市價風險)
        /// </summary>
        [Index(11)]
        public decimal? BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值原幣)
        /// </summary>
        [Index(12)]
        public decimal? YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(淨值原幣)
        /// </summary>
        [Index(13)]
        public decimal? DailyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(淨值原幣)
        /// </summary>
        [Index(14)]
        public decimal? WeeklyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(淨值原幣)
        /// </summary>
        [Index(15)]
        public decimal? MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(淨值原幣)
        /// </summary>
        [Index(16)]
        public decimal? ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(淨值原幣)
        /// </summary>
        [Index(17)]
        public decimal? SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(淨值原幣)
        /// </summary>
        [Index(18)]
        public decimal? OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(淨值原幣)
        /// </summary>
        [Index(19)]
        public decimal? TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(淨值原幣)
        /// </summary>
        [Index(20)]
        public decimal? ThreeYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(淨值原幣)
        /// </summary>
        [Index(21)]
        public decimal? FiveYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(淨值原幣)
        /// </summary>
        [Index(22)]
        public decimal? TenYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(市價原幣)
        /// </summary>
        [Index(23)]
        public decimal? YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(市價原幣)
        /// </summary>
        [Index(24)]
        public decimal? DailyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(市價原幣)
        /// </summary>
        [Index(25)]
        public decimal? WeeklyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(市價原幣)
        /// </summary>
        [Index(26)]
        public decimal? MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(市價原幣)
        /// </summary>
        [Index(27)]
        public decimal? ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(市價原幣)
        /// </summary>
        [Index(28)]
        public decimal? SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(市價原幣)
        /// </summary>
        [Index(29)]
        public decimal? OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(市價原幣)
        /// </summary>
        [Index(30)]
        public decimal? TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(市價原幣)
        /// </summary>
        [Index(31)]
        public decimal? ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(市價原幣)
        /// </summary>
        [Index(32)]
        public decimal? FiveYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(市價原幣)
        /// </summary>
        [Index(33)]
        public decimal? TenYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        [Index(34)]
        public decimal? YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一日報酬(淨值台幣)
        /// </summary>
        [Index(35)]
        public decimal? DailyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一週報酬(淨值台幣)
        /// </summary>
        [Index(36)]
        public decimal? WeeklyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月報酬(淨值台幣)
        /// </summary>
        [Index(37)]
        public decimal? MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月報酬(淨值台幣)
        /// </summary>
        [Index(38)]
        public decimal? ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月報酬(淨值台幣)
        /// </summary>
        [Index(39)]
        public decimal? SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年報酬(淨值台幣)
        /// </summary>
        [Index(40)]
        public decimal? OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年報酬(淨值台幣)
        /// </summary>
        [Index(41)]
        public decimal? TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年報酬(淨值台幣)
        /// </summary>
        [Index(42)]
        public decimal? ThreeYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 五年報酬(淨值台幣)
        /// </summary>
        [Index(43)]
        public decimal? FiveYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 十年報酬(淨值台幣)
        /// </summary>
        [Index(44)]
        public decimal? TenYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        [Index(45)]
        public decimal? YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一日報酬(市價台幣)
        /// </summary>
        [Index(46)]
        public decimal? DailyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一週報酬(市價台幣)
        /// </summary>
        [Index(47)]
        public decimal? WeeklyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月報酬(市價台幣)
        /// </summary>
        [Index(48)]
        public decimal? MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月報酬(市價台幣)
        /// </summary>
        [Index(49)]
        public decimal? ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月報酬(市價台幣)
        /// </summary>
        [Index(50)]
        public decimal? SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年報酬(市價台幣)
        /// </summary>
        [Index(51)]
        public decimal? OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年報酬(市價台幣)
        /// </summary>
        [Index(52)]
        public decimal? TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年報酬(市價台幣)
        /// </summary>
        [Index(53)]
        public decimal? ThreeYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 五年報酬(市價台幣)
        /// </summary>
        [Index(54)]
        public decimal? FiveYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 十年報酬(市價台幣)
        /// </summary>
        [Index(55)]
        public decimal? TenYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 折溢價
        /// </summary>
        [Index(56)]
        public decimal? DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        [Index(57)]
        public decimal? LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 十日均量
        /// </summary>
        [Index(58)]
        public decimal? TenDayAverageVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量 
        /// </summary>
        [Index(59)]
        public decimal? LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        /// <summary>
        /// 本月以來(淨值原幣)
        /// </summary>
        [Index(60)]
        public decimal? MonthtoDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(淨值台幣)
        /// </summary>
        [Index(61)]
        public decimal? MonthtoDateNetValueTWD { get; set; }

        /// <summary>
        /// 本月以來(市價原幣)
        /// </summary>
        [Index(62)]
        public decimal? MonthtoDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(市價台幣)
        /// </summary>
        [Index(63)]
        public decimal? MonthtoDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(市價原幣)
        /// </summary>
        [Index(64)]
        public decimal? InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(市價台幣)
        /// </summary>
        [Index(65)]
        public decimal? InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值原幣)
        /// </summary>
        [Index(66)]
        public decimal? InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值台幣)
        /// </summary>
        [Index(67)]
        public decimal? InceptionDateNetValueTWD { get; set; }

        /// <summary>
        /// 市價日期
        /// </summary>
        [Index(68)]
        [NullValues("", "NULL", null)]
        public string MarketPriceDate { get; set; }
    }
}