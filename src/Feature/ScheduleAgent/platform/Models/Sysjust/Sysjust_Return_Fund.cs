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
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 近一日報酬(原幣)
        /// </summary>
        [Index(2)]
        public decimal? OneDayReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(原幣)
        /// </summary>
        [Index(3)]
        public decimal? OneWeekReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)
        /// </summary>
        [Index(4)]
        public decimal? MonthtoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)
        /// </summary>
        [Index(5)]
        public decimal? YeartoDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)
        /// </summary>
        [Index(6)]
        public decimal? OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)
        /// </summary>
        [Index(7)]
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)
        /// </summary>
        [Index(8)]
        public decimal? SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬(原幣)
        /// </summary>
        [Index(9)]
        public decimal? OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬(原幣)
        /// </summary>
        [Index(10)]
        public decimal? TwoYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬(原幣)
        /// </summary>
        [Index(11)]
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬(原幣)
        /// </summary>
        [Index(12)]
        public decimal? FiveYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬(原幣)
        /// </summary>
        [Index(13)]
        public decimal? TenYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 成立以來(原幣)
        /// </summary>
        [Index(14)]
        public decimal? InceptionDateReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 近一週報酬(台幣)
        /// </summary>
        [Index(15)]
        public decimal? OneWeekReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)
        /// </summary>
        [Index(16)]
        public decimal? MonthtoDateReturnTWD { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)
        /// </summary>
        [Index(17)]
        public decimal? YeartoDateReturnTWD { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)
        /// </summary>
        [Index(18)]
        public decimal? OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)
        /// </summary>
        [Index(19)]
        public decimal? ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)
        /// </summary>
        [Index(20)]
        public decimal? SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬(台幣)
        /// </summary>
        [Index(21)]
        public decimal? OneYearReturnTWD { get; set; }

        /// <summary>
        /// 二年報酬(台幣)
        /// </summary>
        [Index(22)]
        public decimal? TwoYearReturnTWD { get; set; }

        /// <summary>
        /// 三年報酬(台幣)
        /// </summary>
        [Index(23)]
        public decimal? ThreeYearReturnTWD { get; set; }

        /// <summary>
        /// 五年報酬(台幣)
        /// </summary>
        [Index(24)]
        public decimal? FiveYearReturnTWD { get; set; }

        /// <summary>
        /// 十年報酬(台幣)
        /// </summary>
        [Index(25)]
        public decimal? TenYearReturnTWD { get; set; }

        /// <summary>
        /// 成立以來(台幣)
        /// </summary>
        [Index(26)]
        public decimal? InceptionDateReturnTWD { get; set; }

        /// <summary>
        /// 本月以來報酬(台幣)(定期定額)
        /// </summary>
        [Index(27)]
        public decimal? MonthtoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(台幣)(定期定額)
        /// </summary>
        [Index(28)]
        public decimal? YeartoDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(台幣)(定期定額)
        /// </summary>
        [Index(29)]
        public decimal? OneMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(台幣)(定期定額)
        /// </summary>
        [Index(30)]
        public decimal? ThreeMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(台幣)(定期定額)
        /// </summary>
        [Index(31)]
        public decimal? SixMonthReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(台幣)(定期定額)
        /// </summary>
        [Index(32)]
        public decimal? OneYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(台幣)(定期定額)
        /// </summary>
        [Index(33)]
        public decimal? TwoYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(台幣)(定期定額)
        /// </summary>
        [Index(34)]
        public decimal? ThreeYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(台幣)(定期定額)
        /// </summary>
        [Index(35)]
        public decimal? FiveYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(台幣)(定期定額)
        /// </summary>
        [Index(36)]
        public decimal? TenYearReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(台幣)(定期定額)
        /// </summary>
        [Index(37)]
        public decimal? InceptionDateReturnTWDRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(定期定額)
        /// </summary>
        [Index(38)]
        public decimal? MonthtoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 今年以來報酬(原幣)(定期定額)
        /// </summary>
        [Index(39)]
        public decimal? YeartoDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(定期定額)
        /// </summary>
        [Index(40)]
        public decimal? OneMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(定期定額)
        /// </summary>
        [Index(41)]
        public decimal? ThreeMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(定期定額)
        /// </summary>
        [Index(42)]
        public decimal? SixMonthReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(定期定額)
        /// </summary>
        [Index(43)]
        public decimal? OneYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(定期定額)
        /// </summary>
        [Index(44)]
        public decimal? TwoYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(定期定額)
        /// </summary>
        [Index(45)]
        public decimal? ThreeYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(定期定額)
        /// </summary>
        [Index(46)]
        public decimal? FiveYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(定期定額)
        /// </summary>
        [Index(47)]
        public decimal? TenYearReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 成立以來(原幣)(定期定額)
        /// </summary>
        [Index(48)]
        public decimal? InceptionDateReturnOriginalCurrencyRegularInvestment { get; set; }

        /// <summary>
        /// 本月以來報酬(原幣)(年化報酬)
        /// </summary>
        [Index(49)]
        public decimal? MonthtoDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(原幣)(年化報酬)
        /// </summary>
        [Index(50)]
        public decimal? OneMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(原幣)(年化報酬)
        /// </summary>
        [Index(51)]
        public decimal? ThreeMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(原幣)(年化報酬)
        /// </summary>
        [Index(52)]
        public decimal? SixMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(原幣)(年化報酬)
        /// </summary>
        [Index(53)]
        public decimal? OneYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(原幣)(年化報酬)
        /// </summary>
        [Index(54)]
        public decimal? TwoYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(原幣)(年化報酬)
        /// </summary>
        [Index(55)]
        public decimal? ThreeYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(原幣)(年化報酬)
        /// </summary>
        [Index(56)]
        public decimal? FiveYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(原幣)(年化報酬)
        /// </summary>
        [Index(57)]
        public decimal? TenYearReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(原幣)(年化報酬)
        /// </summary>
        [Index(58)]
        public decimal? InceptionDateReturnOriginalCurrencyAnnualizedReturn { get; set; }

        /// <summary>
        /// 一個月報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(59)]
        public decimal? OneMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三個月報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(60)]
        public decimal? ThreeMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 六個月報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(61)]
        public decimal? SixMonthReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 一年報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(62)]
        public decimal? OneYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 二年報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(63)]
        public decimal? TwoYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 三年報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(64)]
        public decimal? ThreeYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 五年報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(65)]
        public decimal? FiveYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 十年報酬(定期定額)(年化報酬)
        /// </summary>
        [Index(66)]
        public decimal? TenYearReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 成立以來(定期定額)(年化報酬)
        /// </summary>
        [Index(67)]
        public decimal? InceptionDateReturnRegularInvestmentAnnualizedReturn { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        [Index(68)]
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        [Index(69)]
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        [Index(70)]
        public decimal? Beta { get; set; }
        
        [Index(71)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }
    }
}