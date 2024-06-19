using CsvHelper.Configuration.Attributes;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// ETF淨值及報酬率資料檔案，檔案名稱：TFJENAV.YYMMDD.1000.TXT (ETF_NAV.TXT)
    /// </summary>
    [Delimiter(";")]
    [HasHeaderRecord(true)]
    public class EtfNavTfjeNav
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(0)]
        public string DataDate { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ISINCode { get; set; }

        /// <summary>
        /// 銀行商品代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string BankProductCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 基金幣別
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string FundCurrency { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        [Index(5)]
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 申購價
        /// </summary>
        [Index(6)]
        public decimal? SubscriptionFee { get; set; }

        /// <summary>
        /// 贖回價
        /// </summary>
        [Index(7)]
        public decimal? RedemptionFee { get; set; }

        /// <summary>
        /// 一日報酬率(台幣)
        /// </summary>
        [Index(8)]
        public decimal? DailyReturnTWD { get; set; }

        /// <summary>
        /// 一週報酬率(台幣)
        /// </summary>
        [Index(9)]
        public decimal? WeeklyReturnTWD { get; set; }

        /// <summary>
        /// 一個月報酬率(台幣)
        /// </summary>
        [Index(10)]
        public decimal? OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬率(台幣)
        /// </summary>
        [Index(11)]
        public decimal? ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬率(台幣)
        /// </summary>
        [Index(12)]
        public decimal? SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬率(台幣)
        /// </summary>
        [Index(13)]
        public decimal? OneYearReturnTWD { get; set; }

        /// <summary>
        /// 二年報酬率(台幣)
        /// </summary>
        [Index(14)]
        public decimal? TwoYearReturnTWD { get; set; }

        /// <summary>
        /// 三年報酬率(台幣)
        /// </summary>
        [Index(15)]
        public decimal? ThreeYearReturnTWD { get; set; }

        /// <summary>
        /// 五年報酬率(台幣)
        /// </summary>
        [Index(16)]
        public decimal? FiveYearReturnTWD { get; set; }

        /// <summary>
        /// 十年報酬率(台幣)
        /// </summary>
        [Index(17)]
        public decimal? TenYearReturnTWD { get; set; }

        /// <summary>
        /// 一日報酬率(原幣)
        /// </summary>
        [Index(18)]
        public decimal? DailyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一週報酬率(原幣)
        /// </summary>
        [Index(19)]
        public decimal? WeeklyReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬率(原幣)
        /// </summary>
        [Index(20)]
        public decimal? OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬率(原幣)
        /// </summary>
        [Index(21)]
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬率(原幣)
        /// </summary>
        [Index(22)]
        public decimal? SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬率(原幣)
        /// </summary>
        [Index(23)]
        public decimal? OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 二年報酬率(原幣)
        /// </summary>
        [Index(24)]
        public decimal? TwoYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三年報酬率(原幣)
        /// </summary>
        [Index(25)]
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 五年報酬率(原幣)
        /// </summary>
        [Index(26)]
        public decimal? FiveYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 十年報酬率(原幣)
        /// </summary>
        [Index(27)]
        public decimal? TenYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Alpha值(台幣)
        /// </summary>
        [Index(28)]
        public decimal? OneYearAlphaTWD { get; set; }

        /// <summary>
        /// 三年Alpha值(台幣)
        /// </summary>
        [Index(29)]
        public decimal? ThreeYearAlphaTWD { get; set; }

        /// <summary>
        /// 五年Alpha值(台幣)
        /// </summary>
        [Index(30)]
        public decimal? FiveYearAlphaTWD { get; set; }

        /// <summary>
        /// 十年Alpha值(台幣)
        /// </summary>
        [Index(31)]
        public decimal? TenYearAlphaTWD { get; set; }

        /// <summary>
        /// 一年Beta值(台幣)
        /// </summary>
        [Index(32)]
        public decimal? OneYearBetaTWD { get; set; }

        /// <summary>
        /// 三年Beta值(台幣)
        /// </summary>
        [Index(33)]
        public decimal? ThreeYearBetaTWD { get; set; }

        /// <summary>
        /// 五年Beta值(台幣)
        /// </summary>
        [Index(34)]
        public decimal? FiveYearBetaTWD { get; set; }

        /// <summary>
        /// 十年Beta值(台幣)
        /// </summary>
        [Index(35)]
        public decimal? TenYearBetaTWD { get; set; }

        /// <summary>
        /// 一年Sharpe值(台幣)
        /// </summary>
        [Index(36)]
        public decimal? OneYearSharpeTWD { get; set; }

        /// <summary>
        /// 三年Sharpe值(台幣)
        /// </summary>
        [Index(37)]
        public decimal? ThreeYearSharpeTWD { get; set; }

        /// <summary>
        /// 五年Sharpe值(台幣)
        /// </summary>
        [Index(38)]
        public decimal? FiveYearSharpeTWD { get; set; }

        /// <summary>
        /// 十年Sharpe值(台幣)
        /// </summary>
        [Index(39)]
        public decimal? TenYearSharpeTWD { get; set; }

        /// <summary>
        /// 一年標準差(台幣)
        /// </summary>
        [Index(40)]
        public decimal? OneYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 三年標準差(台幣)
        /// </summary>
        [Index(41)]
        public decimal? ThreeYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 五年標準差(台幣)
        /// </summary>
        [Index(42)]
        public decimal? FiveYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 十年標準差(台幣)
        /// </summary>
        [Index(43)]
        public decimal? TenYearStandardDeviationTWD { get; set; }

        /// <summary>
        /// 一年Alpha值(原幣)
        /// </summary>
        [Index(44)]
        public decimal? OneYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Alpha值(原幣)
        /// </summary>
        [Index(45)]
        public decimal? ThreeYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Alpha值(原幣)
        /// </summary>
        [Index(46)]
        public decimal? FiveYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Alpha值(原幣)
        /// </summary>
        [Index(47)]
        public decimal? TenYearAlphaOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Beta值(原幣)
        /// </summary>
        [Index(48)]
        public decimal? OneYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Beta值(原幣)
        /// </summary>
        [Index(49)]
        public decimal? ThreeYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Beta值(原幣)
        /// </summary>
        [Index(50)]
        public decimal? FiveYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Beta值(原幣)
        /// </summary>
        [Index(51)]
        public decimal? TenYearBetaOriginalCurrency { get; set; }

        /// <summary>
        /// 一年Sharpe值(原幣)
        /// </summary>
        [Index(52)]
        public decimal? OneYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 三年Sharpe值(原幣)
        /// </summary>
        [Index(53)]
        public decimal? ThreeYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 五年Sharpe值(原幣)
        /// </summary>
        [Index(54)]
        public decimal? FiveYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 十年Sharpe值(原幣)
        /// </summary>
        [Index(55)]
        public decimal? TenYearSharpeOriginalCurrency { get; set; }

        /// <summary>
        /// 一年標準差(原幣)
        /// </summary>
        [Index(56)]
        public decimal? OneYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 三年標準差(原幣)
        /// </summary>
        [Index(57)]
        public decimal? ThreeYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 五年標準差(原幣)
        /// </summary>
        [Index(58)]
        public decimal? FiveYearStandardDeviationOriginalCurrency { get; set; }

        /// <summary>
        /// 十年標準差(原幣)
        /// </summary>
        [Index(59)]
        public decimal? TenYearStandardDeviationOriginalCurrency { get; set; }
    }
}
