using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicFund
    {
        public string DataDate { get; set; }
        public string FirstBankCode { get; set; }
        public string SysjustFundCode { get; set; }
        public string FundRating { get; set; }
        public string FeeRemarks { get; set; }
        public string DividendFrequencyOneYear { get; set; }
        public string IndicatorIndexName { get; set; }
        public string IndicatorIndexCode { get; set; }
        public string InvestmentRegionName { get; set; }
        public string PrimaryInvestmentRegion { get; set; }
        public string UnKnown { get; set; }
        public string FundType { get; set; }
    }
}