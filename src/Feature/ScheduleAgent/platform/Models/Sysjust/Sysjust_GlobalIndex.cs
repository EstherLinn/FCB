using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustGlobalIndex
    {
        public string IndexCode { get; set; }
        public string IndexName { get; set; }
        public string IndexCategoryID { get; set; }
        public string IndexCategoryName { get; set; }
        public string DataDate { get; set; }
        public string MarketPrice { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercentage { get; set; }
    }
}