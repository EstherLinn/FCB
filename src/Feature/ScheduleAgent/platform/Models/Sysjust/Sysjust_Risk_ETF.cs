using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 年報酬率比較表，檔案名稱：Sysjust-Risk-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskEtf
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// ETF代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// ETF名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 年化標準差(市價)
        /// </summary>
        public string AnnualizedStandardDeviationMarketPrice { get; set; }

        /// <summary>
        /// Beta(市價)
        /// </summary>
        public string BetaMarketPrice { get; set; }

        /// <summary>
        /// Sharpe(市價)
        /// </summary>
        public string SharpeMarketPrice { get; set; }

        /// <summary>
        /// Information Ratio(市價)
        /// </summary>
        public string InformationRatioMarketPrice { get; set; }

        /// <summary>
        /// Jensen Index(市價)
        /// </summary>
        public string JensenIndexMarketPrice { get; set; }

        /// <summary>
        /// Treynor Index(市價)
        /// </summary>
        public string TreynorIndexMarketPrice { get; set; }
    }
}