using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 國內外基金-最新區間績效，檔案名稱：Sysjust-Return-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// 近一日報酬(原幣)
        /// </summary>
        public decimal? OneDayReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(原幣)
        /// </summary>
        public decimal? OneWeekReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)
        /// </summary>
        public decimal? MonthtoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)
        /// </summary>
        public decimal? YeartoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)
        /// </summary>
        public decimal? OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)
        /// </summary>
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)
        /// </summary>
        public decimal? SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(原幣)
        /// </summary>
        public decimal? OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(原幣)
        /// </summary>
        public decimal? TwoYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(原幣)
        /// </summary>
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(原幣)
        /// </summary>
        public decimal? FiveYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(原幣)
        /// </summary>
        public decimal? TenYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來(原幣)
        /// </summary>
        public decimal? InceptionDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(台幣)
        /// </summary>
        public decimal? OneWeekReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)
        /// </summary>
        public decimal? MonthtoDateReturnTWD { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)
        /// </summary>
        public decimal? YeartoDateReturnTWD { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)
        /// </summary>
        public decimal? OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)
        /// </summary>
        public decimal? ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)
        /// </summary>
        public decimal? SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬(台幣)
        /// </summary>
        public decimal? OneYearReturnTWD { get; set; }

        /// <summary>
        /// 二年報酬(台幣)
        /// </summary>
        public decimal? TwoYearReturnTWD { get; set; }

        /// <summary>
        /// 三年報酬(台幣)
        /// </summary>
        public decimal? ThreeYearReturnTWD { get; set; }

        /// <summary>
        /// 五年報酬(台幣)
        /// </summary>
        public decimal? FiveYearReturnTWD { get; set; }

        /// <summary>
        /// 十年報酬(台幣)
        /// </summary>
        public decimal? TenYearReturnTWD { get; set; }

        /// <summary>
        /// 成立以來(台幣)
        /// </summary>
        public decimal? InceptionDateReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)(定期定額)
        /// </summary>
        public decimal? MonthtoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)(定期定額)
        /// </summary>
        public decimal? YeartoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)(定期定額)
        /// </summary>
        public decimal? OneMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)(定期定額)
        /// </summary>
        public decimal? ThreeMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)(定期定額)
        /// </summary>
        public decimal? SixMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(台幣)(定期定額)
        /// </summary>
        public decimal? OneYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(台幣)(定期定額)
        /// </summary>
        public decimal? TwoYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(台幣)(定期定額)
        /// </summary>
        public decimal? ThreeYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(台幣)(定期定額)
        /// </summary>
        public decimal? FiveYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(台幣)(定期定額)
        /// </summary>
        public decimal? TenYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(台幣)(定期定額)
        /// </summary>
        public decimal? InceptionDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(定期定額)
        /// </summary>
        public decimal? MonthtoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)(定期定額)
        /// </summary>
        public decimal? YeartoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(定期定額)
        /// </summary>
        public decimal? OneMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(定期定額)
        /// </summary>
        public decimal? ThreeMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(定期定額)
        /// </summary>
        public decimal? SixMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(定期定額)
        /// </summary>
        public decimal? OneYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(定期定額)
        /// </summary>
        public decimal? TwoYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(定期定額)
        /// </summary>
        public decimal? ThreeYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(定期定額)
        /// </summary>
        public decimal? FiveYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(定期定額)
        /// </summary>
        public decimal? TenYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(原幣)(定期定額)
        /// </summary>
        public decimal? InceptionDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? MonthtoDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? OneMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? ThreeMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? SixMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? OneYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? TwoYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? ThreeYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? FiveYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(年化報酬)
        /// </summary>
        public decimal? TenYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(原幣)(年化報酬)
        /// </summary>
        public decimal? InceptionDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? OneMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? ThreeMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? SixMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? OneYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? TwoYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? ThreeYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? FiveYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(定期定額)(年化報酬)
        /// </summary>
        public decimal? TenYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(定期定額)(年化報酬)
        /// </summary>
        public decimal? InceptionDateReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public decimal? Beta { get; set; }
        public string DataDate { get; set; }
    }
}