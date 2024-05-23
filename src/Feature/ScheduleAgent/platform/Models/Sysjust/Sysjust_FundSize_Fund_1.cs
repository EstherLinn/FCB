using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustFundSizeFund1
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string ScaleDate { get; set; }
        public string Scale { get; set; }
        public string Currency { get; set; }
    }
}