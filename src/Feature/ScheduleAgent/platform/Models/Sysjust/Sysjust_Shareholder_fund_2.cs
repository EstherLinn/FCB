using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustShareholderFund2
    {
        public string FundCompanyCode { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}