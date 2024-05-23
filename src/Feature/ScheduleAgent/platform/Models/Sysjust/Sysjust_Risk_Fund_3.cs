using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund3
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string Date { get; set; }
        public string FundName { get; set; }
        public string AnnualReturn { get; set; }
        public string AnnualizedStandardDeviation { get; set; }
        public string Beta { get; set; }
        public string Sharpe { get; set; }
        public string InformationRatio { get; set; }
        public string JensenIndex { get; set; }
        public string TreynorIndex { get; set; }
        public string FundODomesticDForeign { get; set; }
    }
}