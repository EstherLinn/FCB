namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    public class FundSize
    {
        public string BankProductCode { get; set; }
        public string ISINCode { get; set; }
        public string FundCurrency { get; set; }
        public string FundSizeMillionOriginalCurrency { get; set; }
        public string FundSizeMillionTWD { get; set; }
        public string ParentFundBankProductCode { get; set; }
        public string ParentFundISINCode { get; set; }
        public string ReferenceExchangeRate { get; set; }
    }
}