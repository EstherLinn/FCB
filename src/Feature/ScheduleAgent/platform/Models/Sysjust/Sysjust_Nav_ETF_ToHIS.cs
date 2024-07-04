using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// ETF – 淨值，Sysjust-Nav-ETF.txt 匯入 Sysjust_ETFNAV_HIS
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavEtfToHis
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        [Index(3)]
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        [Index(4)]
        public decimal? NetAssetValue { get; set; }
    }
}