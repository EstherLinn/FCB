using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustUsStockList
    {
        public string FirstBankCode { get; set; }
        public string FundCode { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string DataDate { get; set; }
        public string ClosingPrice { get; set; }
        public string DailyReturn { get; set; }
        public string WeeklyReturn { get; set; }
        public string MonthlyReturn { get; set; }
        public string ThreeMonthReturn { get; set; }
        public string OneYearReturn { get; set; }
        public string YeartoDateReturn { get; set; }
    }
}