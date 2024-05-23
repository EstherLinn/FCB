using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf2
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string ETFCode { get; set; }
        public string RegionName { get; set; }
        public string Percentage { get; set; }
        public string Amount { get; set; }
    }
}