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
        /// 一年報酬率(年平均報酬)
        /// </summary>
        [Index(3)]
        public decimal? OneYearReturnRateAnnualAverageReturn { get; set; }

        /// <summary>
        /// 一年報酬率(同類型平均)
        /// </summary>
        [Index(4)]
        public decimal? OneYearReturnRateAverage { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        [Index(5)]
        public int? OneYearReturnRateRanking { get; set; }

        /// <summary>
        /// 一年報酬率(排名基金數)
        /// </summary>
        [Index(6)]
        public int? OneYearReturnRateFundsRanking { get; set; }

        /// <summary>
        /// Sharp
        /// </summary>
        [Index(7)]
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Sharp(同類型平均)
        /// </summary>
        [Index(8)]
        public decimal? SharpeAverage { get; set; }

        /// <summary>
        /// Sharp(同類型排名)
        /// </summary>
        [Index(9)]
        public int? SharpeRanking { get; set; }

        /// <summary>
        /// Sharp(排名基金數)
        /// </summary>
        [Index(10)]
        public int? SharpeFundsRanking { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        [Index(11)]
        public decimal? Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        [Index(12)]
        public decimal? BetaAverage { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        [Index(13)]
        public int? BetaRanking { get; set; }

        /// <summary>
        /// Beta(排名基金數)
        /// </summary>
        [Index(14)]
        public int? BetaFundsRanking { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        [Index(15)]
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// 年化標準差(同類型平均)
        /// </summary>
        [Index(16)]
        public decimal? AnnualizedStandardDeviationAverage { get; set; }

        /// <summary>
        /// 年化標準差(同類型排名)
        /// </summary>
        [Index(17)]
        public int? AnnualizedStandardDeviationRanking { get; set; }

        /// <summary>
        /// 年化標準差(排名基金數)
        /// </summary>
        [Index(18)]
        public int? AnnualizedStandardDeviationFundsRanking { get; set; }

        /// <summary>
        /// Information Ratio
        /// </summary>
        [Index(19)]
        public decimal? InformationRatio { get; set; }

        /// <summary>
        /// Information Ratio(同類型平均)
        /// </summary>
        [Index(20)]
        public decimal? InformationRatioAverage { get; set; }

        /// <summary>
        /// Information Ratio(同類型排名)
        /// </summary>
        [Index(21)]
        public int? InformationRatioRanking { get; set; }

        /// <summary>
        /// Information Ratio(排名基金數)
        /// </summary>
        [Index(22)]
        public int? InformationRatioFundsRanking { get; set; }

        /// <summary>
        /// Jensen
        /// </summary>
        [Index(23)]
        public decimal? JensenIndex { get; set; }

        /// <summary>
        /// Jensen(同類型平均)
        /// </summary>
        [Index(24)]
        public decimal? JensenIndexAverage { get; set; }

        /// <summary>
        /// Jensen(同類型排名)
        /// </summary>
        [Index(25)]
        public int? JensenIndexRanking { get; set; }

        /// <summary>
        /// Jensen(排名基金數)
        /// </summary>
        [Index(26)]
        public int? JensenIndexFundsRanking { get; set; }

        /// <summary>
        /// Treynor Index
        /// </summary>
        [Index(27)]
        public decimal? TreynorIndex { get; set; }

        /// <summary>
        /// Treynor Index(同類型平均)
        /// </summary>
        [Index(28)]
        public decimal? TreynorIndexAverage { get; set; }

        /// <summary>
        /// Treynor Index(同類型排名)
        /// </summary>
        [Index(29)]
        public int? TreynorIndexRanking { get; set; }

        /// <summary>
        /// Treynor Index(排名基金數)
        /// </summary>
        [Index(30)]
        public int? TreynorIndexFundsRanking { get; set; }
    }
}