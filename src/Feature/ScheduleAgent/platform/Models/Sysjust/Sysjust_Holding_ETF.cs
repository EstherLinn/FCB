using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 持股 – 依產業，檔案名稱：Sysjust-Holding-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf
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
        /// ETF 代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// 產業名稱
        [Index(3)]
        /// </summary>
        [NullValues("", "NULL", null)]
        public string IndustryName { get; set; }

        /// <summary>
        /// 百分比例
        /// </summary>
        [Index(4)]
        public decimal? Percentage { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Index(6)]
        public decimal? Amount { get; set; }
    }
}