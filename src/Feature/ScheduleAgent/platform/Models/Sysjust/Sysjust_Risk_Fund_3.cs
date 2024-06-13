using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 年報酬率比較表(境內外)，檔案名稱：SYSJUST-RISK-FUND-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund3
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
        /// Date
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundName { get; set; }

        /// <summary>
        /// 整年報酬
        /// </summary>
        public float? AnnualReturn { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Information Ratio
        /// </summary>
        public decimal? InformationRatio { get; set; }

        /// <summary>
        /// Jensen Index
        /// </summary>
        public decimal? JensenIndex { get; set; }

        /// <summary>
        /// Treynor Index
        /// </summary>
        public decimal? TreynorIndex { get; set; }

        /// <summary>
        /// 境內/外基金 O:境內 D:境外
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundODomesticDForeign { get; set; }
    }
}