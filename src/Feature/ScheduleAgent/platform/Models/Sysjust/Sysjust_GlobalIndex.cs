using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustGlobalIndex
    {
        public string IndexCode { get; set; }

        [NullValues("", "NULL", null)]
        public string IndexName { get; set; }

        [NullValues("", "NULL", null)]
        public string IndexCategoryID { get; set; }

        [NullValues("", "NULL", null)]
        public string IndexCategoryName { get; set; }

        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        [NullValues("", "NULL", null)]
        public string MarketPrice { get; set; }

        public decimal? Change { get; set; }

        public decimal? ChangePercentage { get; set; }
    }
}