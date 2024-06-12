using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 風險/報酬率比較表(境外)，檔案名稱：Sysjust-Risk-Fund-2-Oversea.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund2Oversea
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
        public decimal? OneYearReturnRateAnnualAverageReturn { get; set; }

        /// <summary>
        /// 一年報酬率(同類型平均)
        /// </summary>
        public decimal? OneYearReturnRateAverageType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        public int? OneYearReturnRateRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名基金數)
        /// </summary>
        public int? OneYearReturnRateFundsRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同標的平均)
        /// </summary>
        public decimal? OneYearReturnRateAverageTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名)
        /// </summary>
        public int? OneYearReturnRateRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名基金數)
        /// </summary>
        public int? OneYearReturnRateFundsRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同區域平均)
        /// </summary>
        public decimal? OneYearReturnRateAverageRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名)
        /// </summary>
        public int? OneYearReturnRateRankingRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名基金數)
        /// </summary>
        public int? OneYearReturnRateFundsRankingRegion { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Sharpe(同類型平均)
        /// </summary>
        public decimal? SharpeAverageType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名)
        /// </summary>
        public int? SharpeRankingType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名基金數)
        /// </summary>
        public int? SharpeFundsRankingType { get; set; }

        /// <summary>
        /// Sharpe(同標的平均)
        /// </summary>
        public decimal? SharpeAverageTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名)
        /// </summary>
        public int? SharpeRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名基金數)
        /// </summary>
        public int? SharpeFundsRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同區域平均)
        /// </summary>
        public decimal? SharpeAverageRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名)
        /// </summary>
        public int? SharpeRankingRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名基金數)
        /// </summary>
        public int? SharpeFundsRankingRegion { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        public decimal? BetaAverageType { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        public int? BetaRankingType { get; set; }

        /// <summary>
        /// Beta(同類型排名基金數)
        /// </summary>
        public int? BetaFundsRankingType { get; set; }

        /// <summary>
        /// Beta(同標的平均)
        /// </summary>
        public decimal? BetaAverageTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名)
        /// </summary>
        public int? BetaRankingTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名基金數)
        /// </summary>
        public int? BetaFundsRankingTarget { get; set; }

        /// <summary>
        /// Beta(同區域平均)
        /// </summary>
        public decimal? BetaAverageRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名)
        /// </summary>
        public int? BetaRankingRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名基金數)
        /// </summary>
        public int? BetaFundsRankingRegion { get; set; }

        /// <summary>
        /// 標準差
        /// </summary>
        public decimal? StandardDeviation { get; set; }

        /// <summary>
        /// 標準差(同類型平均)
        /// </summary>
        public decimal? StandardDeviationAverageType { get; set; }

        /// <summary>
        /// 標準差(同類型排名)
        /// </summary>
        public int? StandardDeviationRankingType { get; set; }

        /// <summary>
        /// 標準差(同類型排名基金數)
        /// </summary>
        public int? StandardDeviationFundsRankingType { get; set; }

        /// <summary>
        /// 標準差(同標的平均)
        /// </summary>
        public decimal? StandardDeviationAverageTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名)
        /// </summary>
        public int? StandardDeviationRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名基金數)
        /// </summary>
        public int? StandardDeviationFundsRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同區域平均)
        /// </summary>
        public decimal? StandardDeviationAverageRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名)
        /// </summary>
        public int? StandardDeviationRankingRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名基金數)
        /// </summary>
        public int? StandardDeviationFundsRankingRegion { get; set; }
    }
}