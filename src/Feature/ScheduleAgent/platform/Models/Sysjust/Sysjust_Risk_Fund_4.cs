using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內、外基金十年標準差(分布圖使用)，檔案名稱：SYSJUST-RISK-FUND-4.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund4
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 一年標準差
        /// </summary>
        public decimal? OneYearStandardDeviation { get; set; }

        /// <summary>
        /// 二年標準差
        /// </summary>
        public decimal? TwoYearStandardDeviation { get; set; }

        /// <summary>
        /// 三年標準差
        /// </summary>
        public decimal? ThreeYearStandardDeviation { get; set; }

        /// <summary>
        /// 四年標準差
        /// </summary>
        public decimal? FourYearStandardDeviation { get; set; }

        /// <summary>
        /// 五年標準差
        /// </summary>
        public decimal? FiveYearStandardDeviation { get; set; }

        /// <summary>
        /// 六年標準差
        /// </summary>
        public decimal? SixYearStandardDeviation { get; set; }

        /// <summary>
        /// 七年標準差
        /// </summary>
        public decimal? SevenYearStandardDeviation { get; set; }

        /// <summary>
        /// 八年標準差
        /// </summary>
        public decimal? EightYearStandardDeviation { get; set; }

        /// <summary>
        /// 九年標準差
        /// </summary>
        public decimal? NineYearStandardDeviation { get; set; }

        /// <summary>
        /// 十年標準差
        /// </summary>
        public decimal? TenYearStandardDeviation { get; set; }
    }
}