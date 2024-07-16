using CsvHelper.Configuration.Attributes;
using System;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金 – 淨值，檔案名稱：Sysjust-Nav-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavFund
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [Index(1)]
        public DateTime? NetAssetValueDate { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        [Index(2)]
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        ///嘉實代碼
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }
    }
}