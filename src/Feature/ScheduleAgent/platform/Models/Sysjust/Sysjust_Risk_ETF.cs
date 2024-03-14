namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    internal class SysjustRiskEtf
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string ETFCode { get; set; }
        public string ETFName { get; set; }
        public string AnnualizedStandardDeviationMarketPrice { get; set; }
        public string BetaMarketPrice { get; set; }
        public string SharpeMarketPrice { get; set; }
        public string InformationRatioMarketPrice { get; set; }
        public string JensenIndexMarketPrice { get; set; }
        public string TreynorIndexMarketPrice { get; set; }
    }
}