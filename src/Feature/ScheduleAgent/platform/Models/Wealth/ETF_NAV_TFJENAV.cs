using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// ETF淨值及報酬率資料檔案，檔案名稱：TFJENAV.YYMMDD.1000.TXT (ETF_NAV.TXT)
    /// </summary>
    [Delimiter(";")]
    [HasHeaderRecord(false)]
    public class EtfNavTfjeNav
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        public string ISINCode { get; set; }

        /// <summary>
        /// 銀行商品代碼
        /// </summary>
        public string BankProductCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 基金幣別
        /// </summary>
        public string FundCurrency { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public string NetAssetValue { get; set; }

        /// <summary>
        /// 申購價
        /// </summary>
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 贖回價
        /// </summary>
        public string RedemptionFee { get; set; }

        /// <summary>
        /// 一日報酬率(台幣)
        /// </summary>
        public string DailyReturnTWD { get; set; }

        /// <summary>
        /// 一週報酬率(台幣)
        /// </summary>
        public string WeeklyReturnTWD { get; set; }

        /// <summary>
        /// 一個月報酬率(台幣)
        /// </summary>
        public string OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬率(台幣)
        /// </summary>
        public string ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬率(台幣)
        /// </summary>
        public string SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬率(台幣)
        /// </summary>
        public string OneYearReturnTWD { get; set; }

        /// <summary>
        /// 二年報酬率(台幣)
        /// </summary>
        public string TwoYearReturnTWD { get; set; }

        /// <summary>
        /// 三年報酬率(台幣)
        /// </summary>
        public string ThreeYearReturnTWD { get; set; }

        /// <summary>
        /// 五年報酬率(台幣)
        /// </summary>
        public string FiveYearReturnTWD { get; set; }

        /// <summary>
        /// 十年報酬率(台幣)
        /// </summary>
        public string TenYearReturnTWD { get; set; }

        /// <summary>
        /// 一日報酬率(原幣)
        /// </summary>
        public string DailyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬率(原幣)
        /// </summary>
        public string WeeklyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬率(原幣)
        /// </summary>
        public string OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬率(原幣)
        /// </summary>
        public string ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬率(原幣)
        /// </summary>
        public string SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬率(原幣)
        /// </summary>
        public string OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬率(原幣)
        /// </summary>
        public string TwoYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬率(原幣)
        /// </summary>
        public string ThreeYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬率(原幣)
        /// </summary>
        public string FiveYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬率(原幣)
        /// </summary>
        public string TenYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Alpha值(台幣)
        /// </summary>
        public string OneYearAlphaTWD { get; set; }

        /// <summary>
        /// 三年Alpha值(台幣)
        /// </summary>
        public string ThreeYearAlphaTWD { get; set; }

        /// <summary>
        /// 五年Alpha值(台幣)
        /// </summary>
        public string FiveYearAlphaTWD { get; set; }

        /// <summary>
        /// 十年Alpha值(台幣)
        /// </summary>
        public string TenYearAlphaTWD { get; set; }

        /// <summary>
        /// 一年Beta值(台幣)
        /// </summary>
        public string OneYearBetaTWD { get; set; }

        /// <summary>
        /// 三年Beta值(台幣)
        /// </summary>
        public string ThreeYearBetaTWD { get; set; }

        /// <summary>
        /// 五年Beta值(台幣)
        /// </summary>
        public string FiveYearBetaTWD { get; set; }

        /// <summary>
        /// 十年Beta值(台幣)
        /// </summary>
        public string TenYearBetaTWD { get; set; }

        /// <summary>
        /// 一年Sharpe值(台幣)
        /// </summary>
        public string OneYearSharpeTWD { get; set; }

        /// <summary>
        /// 三年Sharpe值(台幣)
        /// </summary>
        public string ThreeYearSharpeTWD { get; set; }

        /// <summary>
        /// 五年Sharpe值(台幣)
        /// </summary>
        public string FiveYearSharpeTWD { get; set; }

        /// <summary>
        /// 十年Sharpe值(台幣)
        /// </summary>
        public string TenYearSharpeTWD { get; set; }

        /// <summary>
        /// 一年標準差(台幣)
        /// </summary>
        public string OneYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 三年標準差(台幣)
        /// </summary>
        public string ThreeYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 五年標準差(台幣)
        /// </summary>
        public string FiveYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 十年標準差(台幣)
        /// </summary>
        public string TenYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 一年Alpha值(原幣)
        /// </summary>
        public string OneYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Alpha值(原幣)
        /// </summary>
        public string ThreeYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Alpha值(原幣)
        /// </summary>
        public string FiveYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Alpha值(原幣)
        /// </summary>
        public string TenYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Beta值(原幣)
        /// </summary>
        public string OneYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Beta值(原幣)
        /// </summary>
        public string ThreeYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Beta值(原幣)
        /// </summary>
        public string FiveYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Beta值(原幣)
        /// </summary>
        public string TenYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Sharpe值(原幣)
        /// </summary>
        public string OneYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Sharpe值(原幣)
        /// </summary>
        public string ThreeYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Sharpe值(原幣)
        /// </summary>
        public string FiveYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Sharpe值(原幣)
        /// </summary>
        public string TenYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 一年標準差(原幣)
        /// </summary>
        public string OneYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 三年標準差(原幣)
        /// </summary>
        public string ThreeYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 五年標準差(原幣)
        /// </summary>
        public string FiveYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 十年標準差(原幣)
        /// </summary>
        public string TenYearStandardDeviationOriginalCurrency { get; set; }
    }
}
