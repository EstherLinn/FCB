using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund4
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string SysjustCode { get; set; }
        public string FundName { get; set; }
        public string OneYearStandardDeviation { get; set; }
        public string TwoYearStandardDeviation { get; set; }
        public string ThreeYearStandardDeviation { get; set; }
        public string FourYearStandardDeviation { get; set; }
        public string FiveYearStandardDeviation { get; set; }
        public string SixYearStandardDeviation { get; set; }
        public string SevenYearStandardDeviation { get; set; }
        public string EightYearStandardDeviation { get; set; }
        public string NineYearStandardDeviation { get; set; }
        public string TenYearStandardDeviation { get; set; }
    }
}