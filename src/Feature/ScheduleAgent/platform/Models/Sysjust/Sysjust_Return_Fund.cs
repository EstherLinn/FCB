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
        public string OneDayReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(原幣)
        /// </summary>
        public string OneWeekReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)
        /// </summary>
        public string MonthtoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)
        /// </summary>
        public string YeartoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)
        /// </summary>
        public string OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)
        /// </summary>
        public string ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)
        /// </summary>
        public string SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(原幣)
        /// </summary>
        public string OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(原幣)
        /// </summary>
        public string TwoYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(原幣)
        /// </summary>
        public string ThreeYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(原幣)
        /// </summary>
        public string FiveYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(原幣)
        /// </summary>
        public string TenYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來(原幣)
        /// </summary>
        public string InceptionDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(台幣)
        /// </summary>
        public string OneWeekReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)
        /// </summary>
        public string MonthtoDateReturnTWD { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)
        /// </summary>
        public string YeartoDateReturnTWD { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)
        /// </summary>
        public string OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)
        /// </summary>
        public string ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)
        /// </summary>
        public string SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬(台幣)
        /// </summary>
        public string OneYearReturnTWD { get; set; }

        /// <summary>
        /// 二年報酬(台幣)
        /// </summary>
        public string TwoYearReturnTWD { get; set; }

        /// <summary>
        /// 三年報酬(台幣)
        /// </summary>
        public string ThreeYearReturnTWD { get; set; }

        /// <summary>
        /// 五年報酬(台幣)
        /// </summary>
        public string FiveYearReturnTWD { get; set; }

        /// <summary>
        /// 十年報酬(台幣)
        /// </summary>
        public string TenYearReturnTWD { get; set; }

        /// <summary>
        /// 成立以來(台幣)
        /// </summary>
        public string InceptionDateReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)(定期定額)
        /// </summary>
        public string MonthtoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)(定期定額)
        /// </summary>
        public string YeartoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)(定期定額)
        /// </summary>
        public string OneMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)(定期定額)
        /// </summary>
        public string ThreeMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)(定期定額)
        /// </summary>
        public string SixMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(台幣)(定期定額)
        /// </summary>
        public string OneYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(台幣)(定期定額)
        /// </summary>
        public string TwoYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(台幣)(定期定額)
        /// </summary>
        public string ThreeYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(台幣)(定期定額)
        /// </summary>
        public string FiveYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(台幣)(定期定額)
        /// </summary>
        public string TenYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(台幣)(定期定額)
        /// </summary>
        public string InceptionDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(定期定額)
        /// </summary>
        public string MonthtoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)(定期定額)
        /// </summary>
        public string YeartoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(定期定額)
        /// </summary>
        public string OneMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(定期定額)
        /// </summary>
        public string ThreeMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(定期定額)
        /// </summary>
        public string SixMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(定期定額)
        /// </summary>
        public string OneYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(定期定額)
        /// </summary>
        public string TwoYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(定期定額)
        /// </summary>
        public string ThreeYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(定期定額)
        /// </summary>
        public string FiveYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(定期定額)
        /// </summary>
        public string TenYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(原幣)(定期定額)
        /// </summary>
        public string InceptionDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(年化報酬)
        /// </summary>
        public string MonthtoDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(年化報酬)
        /// </summary>
        public string OneMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(年化報酬)
        /// </summary>
        public string ThreeMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(年化報酬)
        /// </summary>
        public string SixMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(年化報酬)
        /// </summary>
        public string OneYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(年化報酬)
        /// </summary>
        public string TwoYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(年化報酬)
        /// </summary>
        public string ThreeYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(年化報酬)
        /// </summary>
        public string FiveYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(年化報酬)
        /// </summary>
        public string TenYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(原幣)(年化報酬)
        /// </summary>
        public string InceptionDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(定期定額)(年化報酬)
        /// </summary>
        public string OneMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(定期定額)(年化報酬)
        /// </summary>
        public string ThreeMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(定期定額)(年化報酬)
        /// </summary>
        public string SixMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(定期定額)(年化報酬)
        /// </summary>
        public string OneYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(定期定額)(年化報酬)
        /// </summary>
        public string TwoYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(定期定額)(年化報酬)
        /// </summary>
        public string ThreeYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(定期定額)(年化報酬)
        /// </summary>
        public string FiveYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(定期定額)(年化報酬)
        /// </summary>
        public string TenYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(定期定額)(年化報酬)
        /// </summary>
        public string InceptionDateReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public string AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public string Sharpe { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public string Beta { get; set; }
        public string DataDate { get; set; }
    }
}