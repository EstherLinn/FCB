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
        public string OneYearReturnRateAnnualAverageReturn { get; set; }

        /// <summary>
        /// 一年報酬率(同類型平均)
        /// </summary>
        public string OneYearReturnRateAverageType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        public string OneYearReturnRateRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名基金數)
        /// </summary>
        public string OneYearReturnRateFundsRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同標的平均)
        /// </summary>
        public string OneYearReturnRateAverageTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名)
        /// </summary>
        public string OneYearReturnRateRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名基金數)
        /// </summary>
        public string OneYearReturnRateFundsRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同區域平均)
        /// </summary>
        public string OneYearReturnRateAverageRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名)
        /// </summary>
        public string OneYearReturnRateRankingRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名基金數)
        /// </summary>
        public string OneYearReturnRateFundsRankingRegion { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public string Sharpe { get; set; }

        /// <summary>
        /// Sharpe(同類型平均)
        /// </summary>
        public string SharpeAverageType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名)
        /// </summary>
        public string SharpeRankingType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名基金數)
        /// </summary>
        public string SharpeFundsRankingType { get; set; }

        /// <summary>
        /// Sharpe(同標的平均)
        /// </summary>
        public string SharpeAverageTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名)
        /// </summary>
        public string SharpeRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名基金數)
        /// </summary>
        public string SharpeFundsRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同區域平均)
        /// </summary>
        public string SharpeAverageRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名)
        /// </summary>
        public string SharpeRankingRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名基金數)
        /// </summary>
        public string SharpeFundsRankingRegion { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public string Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        public string BetaAverageType { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        public string BetaRankingType { get; set; }

        /// <summary>
        /// Beta(同類型排名基金數)
        /// </summary>
        public string BetaFundsRankingType { get; set; }

        /// <summary>
        /// Beta(同標的平均)
        /// </summary>
        public string BetaAverageTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名)
        /// </summary>
        public string BetaRankingTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名基金數)
        /// </summary>
        public string BetaFundsRankingTarget { get; set; }

        /// <summary>
        /// Beta(同區域平均)
        /// </summary>
        public string BetaAverageRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名)
        /// </summary>
        public string BetaRankingRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名基金數)
        /// </summary>
        public string BetaFundsRankingRegion { get; set; }

        /// <summary>
        /// 標準差
        /// </summary>
        public string StandardDeviation { get; set; }

        /// <summary>
        /// 標準差(同類型平均)
        /// </summary>
        public string StandardDeviationAverageType { get; set; }

        /// <summary>
        /// 標準差(同類型排名)
        /// </summary>
        public string StandardDeviationRankingType { get; set; }

        /// <summary>
        /// 標準差(同類型排名基金數)
        /// </summary>
        public string StandardDeviationFundsRankingType { get; set; }

        /// <summary>
        /// 標準差(同標的平均)
        /// </summary>
        public string StandardDeviationAverageTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名)
        /// </summary>
        public string StandardDeviationRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名基金數)
        /// </summary>
        public string StandardDeviationFundsRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同區域平均)
        /// </summary>
        public string StandardDeviationAverageRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名)
        /// </summary>
        public string StandardDeviationRankingRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名基金數)
        /// </summary>
        public string StandardDeviationFundsRankingRegion { get; set; }
    }
}