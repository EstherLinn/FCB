using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund2
    {
        public string FirstBankCode { get; set; }
        public string SysjustFundCode { get; set; }
        public string Year { get; set; }
        public string AnnualReturnRateOriginalCurrency { get; set; }
        public string AnnualReturnRateTWD { get; set; }
        public string IndicatorIndexPriceChange { get; set; }
    }
}