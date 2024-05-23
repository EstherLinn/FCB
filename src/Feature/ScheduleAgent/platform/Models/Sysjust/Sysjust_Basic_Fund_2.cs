using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicFund2
    {
        public string DataDate { get; set; }
        public string FirstBankCode { get; set; }
        public string SysjustFundCode { get; set; }
        public string FundRating { get; set; }
        public string FeeRemarks { get; set; }
        public string DividendFrequencyOneYear { get; set; }
        public string IndicatorIndexName { get; set; }
        public string IndicatorIndexCode { get; set; }
        public string FundApprovalEffectiveDate { get; set; }
        public string MasterAgentFundEffectiveDate { get; set; }
        public string DomesticInvestmentRatioDate { get; set; }
        public string DomesticInvestmentRatio { get; set; }
        public string InvestmentRegionName { get; set; }
        public string MaxManagerFee { get; set; }
        public string MaxStorageFee { get; set; }
        public string SingleQuote { get; set; }
    }
}