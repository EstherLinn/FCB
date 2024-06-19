using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustGlobalIndexRoi
    {
        public string IndexID { get; set; }

        [NullValues("", "NULL", null)]
        public string IndexName { get; set; }

        [NullValues("", "NULL", null)]
        public string DataDate { get; set; }

        public decimal? Closing { get; set; }

        public decimal? Change { get; set; }

        public decimal? DailyReturn { get; set; }

        public decimal? WeeklyReturn { get; set; }

        public decimal? OneMonthReturn { get; set; }

        public decimal? ThreeMonthReturn { get; set; }

        public decimal? SixMonthReturn { get; set; }

        public decimal? YeartoDateReturn { get; set; }

        public decimal? OneYearReturn { get; set; }
        public decimal? ThreeYearReturn { get; set; }
    }
}