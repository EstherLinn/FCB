using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string ETFCode { get; set; }
        public string IndustryName { get; set; }
        public string Percentage { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
    }
}