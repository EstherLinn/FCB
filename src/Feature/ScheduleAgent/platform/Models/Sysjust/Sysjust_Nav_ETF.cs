using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustNavEtf
    {
        public string FirstBankCode { get; set; }
        public string ExchangeCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public string MarketPrice { get; set; }
        public string NetAssetValue { get; set; }
    }
}