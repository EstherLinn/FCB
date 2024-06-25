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
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 一年標準差
        /// </summary>
        [Index(4)]
        public decimal? OneYearStandardDeviation { get; set; }

        /// <summary>
        /// 二年標準差
        /// </summary>
        [Index(5)]
        public decimal? TwoYearStandardDeviation { get; set; }

        /// <summary>
        /// 三年標準差
        /// </summary>
        [Index(6)]
        public decimal? ThreeYearStandardDeviation { get; set; }

        /// <summary>
        /// 四年標準差
        /// </summary>
        [Index(7)]
        public decimal? FourYearStandardDeviation { get; set; }

        /// <summary>
        /// 五年標準差
        /// </summary>
        [Index(8)]
        public decimal? FiveYearStandardDeviation { get; set; }

        /// <summary>
        /// 六年標準差
        /// </summary>
        [Index(9)]
        public decimal? SixYearStandardDeviation { get; set; }

        /// <summary>
        /// 七年標準差
        /// </summary>
        [Index(10)]
        public decimal? SevenYearStandardDeviation { get; set; }

        /// <summary>
        /// 八年標準差
        /// </summary>
        [Index(11)]
        public decimal? EightYearStandardDeviation { get; set; }

        /// <summary>
        /// 九年標準差
        /// </summary>
        [Index(12)]
        public decimal? NineYearStandardDeviation { get; set; }

        /// <summary>
        /// 十年標準差
        /// </summary>
        [Index(13)]
        public decimal? TenYearStandardDeviation { get; set; }
    }
}