using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 持股 – 依照區域，檔案名稱：Sysjust-Holding-ETF-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf2
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 區域名稱
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public string Percentage { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public string Amount { get; set; }
    }
}