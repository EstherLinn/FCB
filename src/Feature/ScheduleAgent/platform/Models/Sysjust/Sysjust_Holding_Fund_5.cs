using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustHoldingFund5
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string Date { get; set; }
        public string FundName { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string Shareholding { get; set; }
        public string ShareholdingThousands { get; set; }
        public string Change { get; set; }
    }
}