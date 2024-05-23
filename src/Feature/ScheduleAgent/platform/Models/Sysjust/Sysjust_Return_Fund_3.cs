using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund3
    {
        public string FirstBankCode { get; set; }
        public string SysjustFundCode { get; set; }
        public string Date { get; set; }
        public string MonthlyReturnRate { get; set; }
        public string IndicatorIndexPriceChange { get; set; }
    }
}