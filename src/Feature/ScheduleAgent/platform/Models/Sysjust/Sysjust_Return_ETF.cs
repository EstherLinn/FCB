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
        public string MarketPrice { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public string NetAssetValue { get; set; }

        /// <summary>
        /// 計價(市價)幣別
        /// </summary>
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// 年化標準差(淨值風險)
        /// </summary>
        public string AnnualizedStandardDeviationNetValueRisk { get; set; }

        /// <summary>
        /// Sharpe(淨值風險)
        /// </summary>
        public string SharpeNetValueRisk { get; set; }

        /// <summary>
        /// beta(淨值風險)
        /// </summary>
        public string BetaNetValueRisk { get; set; }

        /// <summary>
        /// 年化標準差(市價風險)
        /// </summary>
        public string AnnualizedStandardDeviationMarketPriceRisk { get; set; }

        /// <summary>
        /// Sharpe(市價風險)
        /// </summary>
        public string SharpeRatioMarketPriceRisk { get; set; }

        /// <summary>
        /// beta(市價風險)
        /// </summary>
        public string BetaMarketPriceRisk { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值原幣)
        /// </summary>
        public string YeartoDateReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(淨值原幣)
        /// </summary>
        public string DailyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(淨值原幣)
        /// </summary>
        public string WeeklyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(淨值原幣)
        /// </summary>
        public string MonthlyReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(淨值原幣)
        /// </summary>
        public string ThreeMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(淨值原幣)
        /// </summary>
        public string SixMonthReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(淨值原幣)
        /// </summary>
        public string OneYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(淨值原幣)
        /// </summary>
        public string TwoYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(淨值原幣)
        /// </summary>
        public string ThreeYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(淨值原幣)
        /// </summary>
        public string FiveYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(淨值原幣)
        /// </summary>
        public string TenYearReturnNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(市價原幣)
        /// </summary>
        public string YeartoDateReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一日報酬(市價原幣)
        /// </summary>
        public string DailyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬(市價原幣)
        /// </summary>
        public string WeeklyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(市價原幣)
        /// </summary>
        public string MonthlyReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(市價原幣)
        /// </summary>
        public string ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(市價原幣)
        /// </summary>
        public string SixMonthReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(市價原幣)
        /// </summary>
        public string OneYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(市價原幣)
        /// </summary>
        public string TwoYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(市價原幣)
        /// </summary>
        public string ThreeYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(市價原幣)
        /// </summary>
        public string FiveYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(市價原幣)
        /// </summary>
        public string TenYearReturnMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        public string YeartoDateReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一日報酬(淨值台幣)
        /// </summary>
        public string DailyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一週報酬(淨值台幣)
        /// </summary>
        public string WeeklyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一個月報酬(淨值台幣)
        /// </summary>
        public string MonthlyReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三個月報酬(淨值台幣)
        /// </summary>
        public string ThreeMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 六個月報酬(淨值台幣)
        /// </summary>
        public string SixMonthReturnNetValueTWD { get; set; }

        /// <summary>
        /// 一年報酬(淨值台幣)
        /// </summary>
        public string OneYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 二年報酬(淨值台幣)
        /// </summary>
        public string TwoYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 三年報酬(淨值台幣)
        /// </summary>
        public string ThreeYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 五年報酬(淨值台幣)
        /// </summary>
        public string FiveYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 十年報酬(淨值台幣)
        /// </summary>
        public string TenYearReturnNetValueTWD { get; set; }

        /// <summary>
        /// 自今年以來報酬(淨值台幣)
        /// </summary>
        public string YeartoDateReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一日報酬(市價台幣)
        /// </summary>
        public string DailyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一週報酬(市價台幣)
        /// </summary>
        public string WeeklyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一個月報酬(市價台幣)
        /// </summary>
        public string MonthlyReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三個月報酬(市價台幣)
        /// </summary>
        public string ThreeMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 六個月報酬(市價台幣)
        /// </summary>
        public string SixMonthReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 一年報酬(市價台幣)
        /// </summary>
        public string OneYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 二年報酬(市價台幣)
        /// </summary>
        public string TwoYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 三年報酬(市價台幣)
        /// </summary>
        public string ThreeYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 五年報酬(市價台幣)
        /// </summary>
        public string FiveYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 十年報酬(市價台幣)
        /// </summary>
        public string TenYearReturnMarketPriceTWD { get; set; }

        /// <summary>
        /// 折溢價
        /// </summary>
        public string DiscountPremium { get; set; }

        /// <summary>
        /// 最新量(成交量)
        /// </summary>
        public string LatestVolumeTradingVolume { get; set; }

        /// <summary>
        /// 十日均量
        /// </summary>
        public string TenDayAverageVolume { get; set; }

        /// <summary>
        /// 最新量(成交量) – 十日均量 
        /// </summary>
        public string LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }

        /// <summary>
        /// 本月以來(淨值原幣)
        /// </summary>
        public string MonthtoDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(淨值台幣)
        /// </summary>
        public string MonthtoDateNetValueTWD { get; set; }

        /// <summary>
        /// 本月以來(市價原幣)
        /// </summary>
        public string MonthtoDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來(市價台幣)
        /// </summary>
        public string MonthtoDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(市價原幣)
        /// </summary>
        public string InceptionDateMarketPriceOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(市價台幣)
        /// </summary>
        public string InceptionDateMarketPriceTWD { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值原幣)
        /// </summary>
        public string InceptionDateNetValueOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來報酬(淨值台幣)
        /// </summary>
        public string InceptionDateNetValueTWD { get; set; }
    }
}