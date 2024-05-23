using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf2
    {
        public string FirstBankCode { get; set; }
        public string ETFCode { get; set; }
        public string DataDate { get; set; }
        public string NetValueAnnualReturnOriginalCurrency { get; set; }
        public string ReferenceIndexAnnualReturn { get; set; }
    }
}