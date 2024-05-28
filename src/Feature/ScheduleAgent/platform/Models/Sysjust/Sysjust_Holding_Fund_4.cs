using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境外基金-持股資料-依照持股明細，檔案名稱：Sysjust-Holding-Fund-4.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund4
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        public string FundName { get; set; }

        /// <summary>
        /// 股名
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        public string Shareholding { get; set; }
    }
}