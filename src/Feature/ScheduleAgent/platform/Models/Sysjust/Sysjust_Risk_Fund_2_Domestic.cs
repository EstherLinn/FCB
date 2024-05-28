using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 風險/報酬率比較表(境內)，檔案名稱：Sysjust-Risk-Fund-2-Domestic.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund2Domestic
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
        /// 一年報酬率(年平均報酬)
        /// </summary>
        public string OneYearReturnRateAnnualAverageReturn { get; set; }

        /// <summary>
        /// 一年報酬率(同類型平均)
        /// </summary>
        public string OneYearReturnRateAverage { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        public string OneYearReturnRateRanking { get; set; }

        /// <summary>
        /// 一年報酬率(排名基金數)
        /// </summary>
        public string OneYearReturnRateFundsRanking { get; set; }

        /// <summary>
        /// Sharp
        /// </summary>
        public string Sharpe { get; set; }

        /// <summary>
        /// Sharp(同類型平均)
        /// </summary>
        public string SharpeAverage { get; set; }

        /// <summary>
        /// Sharp(同類型排名)
        /// </summary>
        public string SharpeRanking { get; set; }

        /// <summary>
        /// Sharp(排名基金數)
        /// </summary>
        public string SharpeFundsRanking { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public string Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        public string BetaAverage { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        public string BetaRanking { get; set; }

        /// <summary>
        /// Beta(排名基金數)
        /// </summary>
        public string BetaFundsRanking { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public string AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// 年化標準差(同類型平均)
        /// </summary>
        public string AnnualizedStandardDeviationAverage { get; set; }

        /// <summary>
        /// 年化標準差(同類型排名)
        /// </summary>
        public string AnnualizedStandardDeviationRanking { get; set; }

        /// <summary>
        /// 年化標準差(排名基金數)
        /// </summary>
        public string AnnualizedStandardDeviationFundsRanking { get; set; }

        /// <summary>
        /// Information Ratio
        /// </summary>
        public string InformationRatio { get; set; }

        /// <summary>
        /// Information Ratio(同類型平均)
        /// </summary>
        public string InformationRatioAverage { get; set; }

        /// <summary>
        /// Information Ratio(同類型排名)
        /// </summary>
        public string InformationRatioRanking { get; set; }

        /// <summary>
        /// Information Ratio(排名基金數)
        /// </summary>
        public string InformationRatioFundsRanking { get; set; }

        /// <summary>
        /// Jensen
        /// </summary>
        public string JensenIndex { get; set; }

        /// <summary>
        /// Jensen(同類型平均)
        /// </summary>
        public string JensenIndexAverage { get; set; }

        /// <summary>
        /// Jensen(同類型排名)
        /// </summary>
        public string JensenIndexRanking { get; set; }

        /// <summary>
        /// Jensen(排名基金數)
        /// </summary>
        public string JensenIndexFundsRanking { get; set; }

        /// <summary>
        /// Treynor Index
        /// </summary>
        public string TreynorIndex { get; set; }

        /// <summary>
        /// Treynor Index(同類型平均)
        /// </summary>
        public string TreynorIndexAverage { get; set; }

        /// <summary>
        /// Treynor Index(同類型排名)
        /// </summary>
        public string TreynorIndexRanking { get; set; }

        /// <summary>
        /// Treynor Index(排名基金數)
        /// </summary>
        public string TreynorIndexFundsRanking { get; set; }
    }
}