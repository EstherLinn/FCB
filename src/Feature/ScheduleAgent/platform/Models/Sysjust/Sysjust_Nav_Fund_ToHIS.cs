using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金歷史淨值，檔案名稱：SYSJUST-FUNDNAV-HIS.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavFundToHis
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
        /// 淨值
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string NetAssetValue { get; set; }
    }
}
