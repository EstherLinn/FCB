using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfRiskGraph
    {
        public string FirstBankCode { get; set; }
        public DateTime Date { get; set; }
        public string ETFName { get; set; }
        public decimal? OneYearStandardDeviation { get; set; }
        public decimal? TwoYearStandardDeviation { get; set; }
        public decimal? ThreeYearStandardDeviation { get; set; }
        public decimal? OneYearReturnOriginalCurrency { get; set; }
        public decimal? TwoYearReturnOriginalCurrency { get; set; }
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }
    }
}