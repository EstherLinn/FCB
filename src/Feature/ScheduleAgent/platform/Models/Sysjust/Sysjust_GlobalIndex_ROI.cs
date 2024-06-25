using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustGlobalIndexRoi
    {
        [Index(0)]
        public string IndexID { get; set; }

        [Index(1)]
        [NullValues("", "NULL", null)]
        public string IndexName { get; set; }

        [Index(2)]
        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        [Index(3)]
        public decimal? Closing { get; set; }

        [Index(4)]
        public decimal? Change { get; set; }

        [Index(5)]
        public decimal? DailyReturn { get; set; }

        [Index(6)]
        public decimal? WeeklyReturn { get; set; }

        [Index(7)]
        public decimal? OneMonthReturn { get; set; }

        [Index(8)]
        public decimal? ThreeMonthReturn { get; set; }

        [Index(9)]
        public decimal? SixMonthReturn { get; set; }

        [Index(10)]
        public decimal? YeartoDateReturn { get; set; }

        [Index(11)]
        public decimal? OneYearReturn { get; set; }

        [Index(12)]
        public decimal? ThreeYearReturn { get; set; }
    }
}