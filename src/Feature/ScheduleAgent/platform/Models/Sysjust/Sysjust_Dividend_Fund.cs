namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    internal class SysjustDividendFund
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string ExDividendDate { get; set; }
        public string BaseDate { get; set; }
        public string ReleaseDate { get; set; }
        public string Dividend { get; set; }
        public string AnnualizedDividendRate { get; set; }
        public string Currency { get; set; }
        public string AfterTaxInterestValue { get; set; }
        public string UpdateTime { get; set; }
    }
}