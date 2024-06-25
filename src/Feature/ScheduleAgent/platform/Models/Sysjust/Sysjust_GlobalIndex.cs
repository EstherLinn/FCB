using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustGlobalIndex
    {
        [Index(0)]
        public string IndexCode { get; set; }

        [Index(1)]
        [NullValues("", "NULL", null)]
        public string IndexName { get; set; }

        [Index(2)]
        [NullValues("", "NULL", null)]
        public string IndexCategoryID { get; set; }

        [Index(3)]
        [NullValues("", "NULL", null)]
        public string IndexCategoryName { get; set; }

        [Index(4)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        [Index(5)]
        [NullValues("", "NULL", null)]
        public string MarketPrice { get; set; }

        [Index(6)]
        public decimal? Change { get; set; }

        [Index(7)]
        public decimal? ChangePercentage { get; set; }
    }
}