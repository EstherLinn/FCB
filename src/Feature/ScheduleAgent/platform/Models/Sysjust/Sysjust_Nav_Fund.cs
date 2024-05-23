using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavFund
    {
        public string FirstBankCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public string NetAssetValue { get; set; }
        public string SysjustCode { get; set; }
    }
}