namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    public class FundEtf
    {
        public string ProductIdentifier { get; set; }

        public string DataDate { get; set; }

        public string BankProductCode { get; set; }

        public string ETFCurrency { get; set; }

        public string BankBuyPrice { get; set; }

        public string BankSellPrice { get; set; }

        public string PriceBaseDate { get; set; }

        public string d8 { get; set; }

        public string ProductName { get; set; }

        public string ReservedColumn { get; set; }
    }
}