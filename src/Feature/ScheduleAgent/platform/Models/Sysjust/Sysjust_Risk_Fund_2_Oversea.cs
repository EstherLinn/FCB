using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund2Oversea
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string Date { get; set; }
        public string OneYearReturnRateAnnualAverageReturn { get; set; }
        public string OneYearReturnRateAverageType { get; set; }
        public string OneYearReturnRateRankingType { get; set; }
        public string OneYearReturnRateFundsRankingType { get; set; }
        public string OneYearReturnRateAverageTarget { get; set; }
        public string OneYearReturnRateRankingTarget { get; set; }
        public string OneYearReturnRateFundsRankingTarget { get; set; }
        public string OneYearReturnRateAverageRegion { get; set; }
        public string OneYearReturnRateRankingRegion { get; set; }
        public string OneYearReturnRateFundsRankingRegion { get; set; }
        public string Sharpe { get; set; }
        public string SharpeAverageType { get; set; }
        public string SharpeRankingType { get; set; }
        public string SharpeFundsRankingType { get; set; }
        public string SharpeAverageTarget { get; set; }
        public string SharpeRankingTarget { get; set; }
        public string SharpeFundsRankingTarget { get; set; }
        public string SharpeAverageRegion { get; set; }
        public string SharpeRankingRegion { get; set; }
        public string SharpeFundsRankingRegion { get; set; }
        public string Beta { get; set; }
        public string BetaAverageType { get; set; }
        public string BetaRankingType { get; set; }
        public string BetaFundsRankingType { get; set; }
        public string BetaAverageTarget { get; set; }
        public string BetaRankingTarget { get; set; }
        public string BetaFundsRankingTarget { get; set; }
        public string BetaAverageRegion { get; set; }
        public string BetaRankingRegion { get; set; }
        public string BetaFundsRankingRegion { get; set; }
        public string StandardDeviation { get; set; }
        public string StandardDeviationAverageType { get; set; }
        public string StandardDeviationRankingType { get; set; }
        public string StandardDeviationFundsRankingType { get; set; }
        public string StandardDeviationAverageTarget { get; set; }
        public string StandardDeviationRankingTarget { get; set; }
        public string StandardDeviationFundsRankingTarget { get; set; }
        public string StandardDeviationAverageRegion { get; set; }
        public string StandardDeviationRankingRegion { get; set; }
        public string StandardDeviationFundsRankingRegion { get; set; }
    }
}