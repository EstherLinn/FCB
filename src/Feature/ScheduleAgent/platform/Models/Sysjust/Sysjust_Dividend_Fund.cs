using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內、外基金配息，檔案名稱：Sysjust-Dividend-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustDividendFund
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
        /// 配息日(除息日)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ExDividendDate { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string BaseDate { get; set; }

        /// <summary>
        /// 發放日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 配息
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Dividend { get; set; }

        /// <summary>
        /// 年化配息率
        /// </summary>
        public decimal? AnnualizedDividendRate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 稅後息值
        /// </summary>
        [NullValues("", "NULL", null)]
        public string AfterTaxInterestValue { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [NullValues("", "NULL", null)]
        public string UpdateTime { get; set; }
    }
}