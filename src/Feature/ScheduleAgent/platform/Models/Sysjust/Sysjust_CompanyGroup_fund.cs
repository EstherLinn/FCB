using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyGroupFund
    {
        public string FundCompanyCode { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string Shareholding { get; set; }
        public string ShareholdingPercentage { get; set; }
        public string MainExperience { get; set; }
    }
}