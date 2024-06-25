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
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 類股 ID
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string StockID { get; set; }

        /// <summary>
        /// 類股名稱
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string StockName { get; set; }

        /// <summary>
        /// 持股比例
        /// </summary>
        [Index(6)]
        public decimal? Shareholding { get; set; }

        /// <summary>
        /// 基金規模(億)
        /// </summary>
        [Index(7)]
        public decimal? FundSizeMillion { get; set; }
    }
}