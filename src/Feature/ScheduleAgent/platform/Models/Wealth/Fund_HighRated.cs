using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(";")]
    [HasHeaderRecord(false)]
    public class FundHighRated
    {
        public string ProductCode { get; set; }
        public string DataDate { get; set; }
    }
}