namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    internal class SysjustDividendEtf
    {
        public string FirstBankCode { get; set; }
        public string ETFCode { get; set; }
        public string ExDividendDate { get; set; }
        public string RecordDate { get; set; }
        public string PaymentDate { get; set; }
        public string TotalDividendAmount { get; set; }
        public string DividendFrequency { get; set; }
        public string ShortTermCapitalGains { get; set; }
        public string LongTermCapitalGains { get; set; }
        public string Currency { get; set; }
    }
}