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
        [NullValues("", "NULL", null)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 一年報酬率(年平均報酬)
        /// </summary>
        public decimal? OneYearReturnRateAnnualAverageReturn { get; set; }

        /// <summary>
        /// 一年報酬率(同類型平均)
        /// </summary>
        public decimal? OneYearReturnRateAverage { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        public int? OneYearReturnRateRanking { get; set; }

        /// <summary>
        /// 一年報酬率(排名基金數)
        /// </summary>
        public int? OneYearReturnRateFundsRanking { get; set; }

        /// <summary>
        /// Sharp
        /// </summary>
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Sharp(同類型平均)
        /// </summary>
        public decimal? SharpeAverage { get; set; }

        /// <summary>
        /// Sharp(同類型排名)
        /// </summary>
        public int? SharpeRanking { get; set; }

        /// <summary>
        /// Sharp(排名基金數)
        /// </summary>
        public int? SharpeFundsRanking { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        public decimal? BetaAverage { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        public int? BetaRanking { get; set; }

        /// <summary>
        /// Beta(排名基金數)
        /// </summary>
        public int? BetaFundsRanking { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// 年化標準差(同類型平均)
        /// </summary>
        public decimal? AnnualizedStandardDeviationAverage { get; set; }

        /// <summary>
        /// 年化標準差(同類型排名)
        /// </summary>
        public int? AnnualizedStandardDeviationRanking { get; set; }

        /// <summary>
        /// 年化標準差(排名基金數)
        /// </summary>
        public int? AnnualizedStandardDeviationFundsRanking { get; set; }

        /// <summary>
        /// Information Ratio
        /// </summary>
        public decimal? InformationRatio { get; set; }

        /// <summary>
        /// Information Ratio(同類型平均)
        /// </summary>
        public decimal? InformationRatioAverage { get; set; }

        /// <summary>
        /// Information Ratio(同類型排名)
        /// </summary>
        public int? InformationRatioRanking { get; set; }

        /// <summary>
        /// Information Ratio(排名基金數)
        /// </summary>
        public int? InformationRatioFundsRanking { get; set; }

        /// <summary>
        /// Jensen
        /// </summary>
        public decimal? JensenIndex { get; set; }

        /// <summary>
        /// Jensen(同類型平均)
        /// </summary>
        public decimal? JensenIndexAverage { get; set; }

        /// <summary>
        /// Jensen(同類型排名)
        /// </summary>
        public int? JensenIndexRanking { get; set; }

        /// <summary>
        /// Jensen(排名基金數)
        /// </summary>
        public int? JensenIndexFundsRanking { get; set; }

        /// <summary>
        /// Treynor Index
        /// </summary>
        public decimal? TreynorIndex { get; set; }

        /// <summary>
        /// Treynor Index(同類型平均)
        /// </summary>
        public decimal? TreynorIndexAverage { get; set; }

        /// <summary>
        /// Treynor Index(同類型排名)
        /// </summary>
        public int? TreynorIndexRanking { get; set; }

        /// <summary>
        /// Treynor Index(排名基金數)
        /// </summary>
        public int? TreynorIndexFundsRanking { get; set; }
    }
}