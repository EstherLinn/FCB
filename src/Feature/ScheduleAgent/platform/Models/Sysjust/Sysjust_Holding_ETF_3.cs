using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingEtf3
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string ETFCode { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string Percentage { get; set; }
        public string NumberofSharesHeld { get; set; }
    }
}