using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金歷史淨值，檔案名稱：SYSJUST-FUNDNAV-HIS.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustFundNavHis
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
        /// 淨值
        /// </summary>
        [NullValues("", "NULL", null)]
        public string NetAssetValue { get; set; }

        /// <summary>
        /// 贖回
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Redemption { get; set; }

        /// <summary>
        /// 申購
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Subscription { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }
    }
}
