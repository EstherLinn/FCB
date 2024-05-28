using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內基金-依持股類別，檔案名稱：Sysjust-Holding-Fund-1.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund1
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
        /// 類股 ID
        /// </summary>
        public string StockID { get; set; }

        /// <summary>
        /// 類股名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        public string Shareholding { get; set; }

        /// <summary>
        /// 基金規模(億)
        /// </summary>
        public string FundSizeMillion { get; set; }
    }
}