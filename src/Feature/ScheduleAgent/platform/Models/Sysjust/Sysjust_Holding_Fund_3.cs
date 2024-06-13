using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 國內外基金-持股資料-依照區域，檔案名稱：Sysjust-Holding-Fund-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund3
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 產業
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Sector { get; set; }

        /// <summary>
        /// Holding%(Sector)
        /// </summary>
        public decimal? HoldingSector { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 投資金額
        /// </summary>
        public decimal? InvestmentAmount { get; set; }
    }
}