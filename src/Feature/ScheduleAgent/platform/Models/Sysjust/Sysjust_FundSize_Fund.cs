using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustFundSizeFund
    {
        public string FirstBankCode { get; set; }
        public string FundCode { get; set; }
        public string ScaleDate { get; set; }
        public string ScaleMillions { get; set; }
        public string Currency { get; set; }
    }
}