using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustShareholderFund1
    {
        public string FundCompanyCode { get; set; }
        public string ShareholderName { get; set; }
        public string ShareQuantity { get; set; }
        public string SharePercentage { get; set; }
    }
}