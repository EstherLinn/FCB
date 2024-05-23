using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund2
    {
        public string FundCompanyCode { get; set; }
        public string FundCompanyName { get; set; }
        public string EnglishName { get; set; }
    }
}