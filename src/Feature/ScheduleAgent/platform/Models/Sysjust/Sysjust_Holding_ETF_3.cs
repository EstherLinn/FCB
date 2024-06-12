using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 持股 - 持股明細，檔案名稱：Sysjust-Holding-ETF-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf3
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
        /// 個股代碼
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// 持有股數
        /// </summary>
        public decimal? NumberofSharesHeld { get; set; }
    }
}