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
        public decimal? OneYearReturnRateAverageType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名)
        /// </summary>
        [Index(5)]
        public int? OneYearReturnRateRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同類型排名基金數)
        /// </summary>
        [Index(6)]
        public int? OneYearReturnRateFundsRankingType { get; set; }

        /// <summary>
        /// 一年報酬率(同標的平均)
        /// </summary>
        [Index(7)]
        public decimal? OneYearReturnRateAverageTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名)
        /// </summary>
        [Index(8)]
        public int? OneYearReturnRateRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同標的排名基金數)
        /// </summary>
        [Index(9)]
        public int? OneYearReturnRateFundsRankingTarget { get; set; }

        /// <summary>
        /// 一年報酬率(同區域平均)
        /// </summary>
        [Index(10)]
        public decimal? OneYearReturnRateAverageRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名)
        /// </summary>
        [Index(11)]
        public int? OneYearReturnRateRankingRegion { get; set; }

        /// <summary>
        /// 一年報酬率(同區域排名基金數)
        /// </summary>
        [Index(12)]
        public int? OneYearReturnRateFundsRankingRegion { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        [Index(13)]
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Sharpe(同類型平均)
        /// </summary>
        [Index(14)]
        public decimal? SharpeAverageType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名)
        /// </summary>
        [Index(15)]
        public int? SharpeRankingType { get; set; }

        /// <summary>
        /// Sharpe(同類型排名基金數)
        /// </summary>
        [Index(16)]
        public int? SharpeFundsRankingType { get; set; }

        /// <summary>
        /// Sharpe(同標的平均)
        /// </summary>
        [Index(17)]
        public decimal? SharpeAverageTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名)
        /// </summary>
        [Index(18)]
        public int? SharpeRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同標的排名基金數)
        /// </summary>
        [Index(19)]
        public int? SharpeFundsRankingTarget { get; set; }

        /// <summary>
        /// Sharpe(同區域平均)
        /// </summary>
        [Index(20)]
        public decimal? SharpeAverageRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名)
        /// </summary>
        [Index(21)]
        public int? SharpeRankingRegion { get; set; }

        /// <summary>
        /// Sharpe(同區域排名基金數)
        /// </summary>
        [Index(22)]
        public int? SharpeFundsRankingRegion { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        [Index(23)]
        public decimal? Beta { get; set; }

        /// <summary>
        /// Beta(同類型平均)
        /// </summary>
        [Index(24)]
        public decimal? BetaAverageType { get; set; }

        /// <summary>
        /// Beta(同類型排名)
        /// </summary>
        [Index(25)]
        public int? BetaRankingType { get; set; }

        /// <summary>
        /// Beta(同類型排名基金數)
        /// </summary>
        [Index(26)]
        public int? BetaFundsRankingType { get; set; }

        /// <summary>
        /// Beta(同標的平均)
        /// </summary>
        [Index(27)]
        public decimal? BetaAverageTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名)
        /// </summary>
        [Index(28)]
        public int? BetaRankingTarget { get; set; }

        /// <summary>
        /// Beta(同標的排名基金數)
        /// </summary>
        [Index(29)]
        public int? BetaFundsRankingTarget { get; set; }

        /// <summary>
        /// Beta(同區域平均)
        /// </summary>
        [Index(30)]
        public decimal? BetaAverageRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名)
        /// </summary>
        [Index(31)]
        public int? BetaRankingRegion { get; set; }

        /// <summary>
        /// Beta(同區域排名基金數)
        /// </summary>
        [Index(32)]
        public int? BetaFundsRankingRegion { get; set; }

        /// <summary>
        /// 標準差
        /// </summary>
        [Index(33)]
        public decimal? StandardDeviation { get; set; }

        /// <summary>
        /// 標準差(同類型平均)
        /// </summary>
        [Index(34)]
        public decimal? StandardDeviationAverageType { get; set; }

        /// <summary>
        /// 標準差(同類型排名)
        /// </summary>
        [Index(35)]
        public int? StandardDeviationRankingType { get; set; }

        /// <summary>
        /// 標準差(同類型排名基金數)
        /// </summary>
        [Index(36)]
        public int? StandardDeviationFundsRankingType { get; set; }

        /// <summary>
        /// 標準差(同標的平均)
        /// </summary>
        [Index(37)]
        public decimal? StandardDeviationAverageTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名)
        /// </summary>
        [Index(38)]
        public int? StandardDeviationRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同標的排名基金數)
        /// </summary>
        [Index(39)]
        public int? StandardDeviationFundsRankingTarget { get; set; }

        /// <summary>
        /// 標準差(同區域平均)
        /// </summary>
        [Index(40)]
        public decimal? StandardDeviationAverageRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名)
        /// </summary>
        [Index(41)]
        public int? StandardDeviationRankingRegion { get; set; }

        /// <summary>
        /// 標準差(同區域排名基金數)
        /// </summary>
        [Index(42)]
        public int? StandardDeviationFundsRankingRegion { get; set; }
    }
}