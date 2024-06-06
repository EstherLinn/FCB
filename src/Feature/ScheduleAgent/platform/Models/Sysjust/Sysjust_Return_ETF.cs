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
        public string FirstBankCode { get; set; }

        /// <summary>
        ///嘉實代碼
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 價格(市價)
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 年化標準差(淨值風險)
        /// </summary>
        public decimal? AnnualizedStandardDeviationNetValueRisk { get; set; }

        /// <summary>
        /// Sharpe(淨值風險)
        /// </summary>
        public decimal? SharpeNetValueRisk { get; set; }

        /// <summary>
        /// beta(淨值風險)
        /// </summary>
        public decimal? BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差(市價風險)
        /// </summary>
        public decimal? AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        /// <summary>
        /// Sharpe(市價風險)
        /// </summary>
        public decimal? SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// beta(市價風險)
        /// </summary>
        public decimal? BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值原幣)
        /// </summary>
        public decimal? YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(淨值原幣)
        /// </summary>
        public decimal? DailyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(淨值原幣)
        /// </summary>
        public decimal? WeeklyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(淨值原幣)
        /// </summary>
        public decimal? MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(淨值原幣)
        /// </summary>
        public decimal? ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(淨值原幣)
        /// </summary>
        public decimal? SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(淨值原幣)
        /// </summary>
        public decimal? OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(淨值原幣)
        /// </summary>
        public decimal? TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(淨值原幣)
        /// </summary>
        public decimal? ThreeYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(淨值原幣)
        /// </summary>
        public decimal? FiveYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(淨值原幣)
        /// </summary>
        public decimal? TenYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(市價原幣)
        /// </summary>
        public decimal? YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(市價原幣)
        /// </summary>
        public decimal? DailyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(市價原幣)
        /// </summary>
        public decimal? WeeklyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(市價原幣)
        /// </summary>
        public decimal? MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(市價原幣)
        /// </summary>
        public decimal? ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(市價原幣)
        /// </summary>
        public decimal? SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(市價原幣)
        /// </summary>
        public decimal? OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(市價原幣)
        /// </summary>
        public decimal? TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(市價原幣)
        /// </summary>
        public decimal? ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(市價原幣)
        /// </summary>
        public decimal? FiveYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(市價原幣)
        /// </summary>
        public decimal? TenYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        public decimal? YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一日報酬(淨值台幣)
        /// </summary>
        public decimal? DailyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一週報酬(淨值台幣)
        /// </summary>
        public decimal? WeeklyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月報酬(淨值台幣)
        /// </summary>
        public decimal? MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月報酬(淨值台幣)
        /// </summary>
        public decimal? ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月報酬(淨值台幣)
        /// </summary>
        public decimal? SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年報酬(淨值台幣)
        /// </summary>
        public decimal? OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年報酬(淨值台幣)
        /// </summary>
        public decimal? TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年報酬(淨值台幣)
        /// </summary>
        public decimal? ThreeYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 五年報酬(淨值台幣)
        /// </summary>
        public decimal? FiveYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 十年報酬(淨值台幣)
        /// </summary>
        public decimal? TenYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        public decimal? YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一日報酬(市價台幣)
        /// </summary>
        public decimal? DailyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一週報酬(市價台幣)
        /// </summary>
        public decimal? WeeklyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月報酬(市價台幣)
        /// </summary>
        public decimal? MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月報酬(市價台幣)
        /// </summary>
        public decimal? ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月報酬(市價台幣)
        /// </summary>
        public decimal? SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年報酬(市價台幣)
        /// </summary>
        public decimal? OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年報酬(市價台幣)
        /// </summary>
        public decimal? TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年報酬(市價台幣)
        /// </summary>
        public decimal? ThreeYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 五年報酬(市價台幣)
        /// </summary>
        public decimal? FiveYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 十年報酬(市價台幣)
        /// </summary>
        public decimal? TenYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 折溢價
        /// </summary>
        public decimal? DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        public decimal? LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 十日均量
        /// </summary>
        public decimal? TenDayAverageVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量 
        /// </summary>
        public decimal? LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        /// <summary>
        /// 本月以來(淨值原幣)
        /// </summary>
        public decimal? MonthtoDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(淨值台幣)
        /// </summary>
        public decimal? MonthtoDateNetValueTWD { get; set; }

        /// <summary>
        /// 本月以來(市價原幣)
        /// </summary>
        public decimal? MonthtoDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(市價台幣)
        /// </summary>
        public decimal? MonthtoDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(市價原幣)
        /// </summary>
        public decimal? InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(市價台幣)
        /// </summary>
        public decimal? InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值原幣)
        /// </summary>
        public decimal? InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值台幣)
        /// </summary>
        public decimal? InceptionDateNetValueTWD { get; set; }

        /// <summary>
        /// 市價日期
        /// </summary>
        public string MarketPriceDate { get; set; }
    }
}