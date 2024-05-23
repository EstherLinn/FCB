using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund2
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string Date { get; set; }
        public string FundName { get; set; }
        public string Area { get; set; }
        public string HoldingArea { get; set; }
        public string Currency { get; set; }
        public string InvestmentAmount { get; set; }
    }
}