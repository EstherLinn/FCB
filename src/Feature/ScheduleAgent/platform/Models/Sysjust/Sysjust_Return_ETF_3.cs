using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf3
    {
        public string FirstBankCode { get; set; }
        public string ETFCode { get; set; }
        public string DataDate { get; set; }
        public string NetValueMonthlyReturnOriginalCurrency { get; set; }
        public string ReferenceIndexMonthlyReturn { get; set; }
    }
}