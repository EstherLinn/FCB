using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class FundTrb
    {
        public string ProductCode { get; set; }
        public string BankDeferPurchase { get; set; }
        public string FundConversionFee { get; set; }
        public string FundShareConvFeeToBank { get; set; }
        public string FundMgmtFee { get; set; }
        public string FundShareMgmtFeeToBank { get; set; }
        public string FundSingleSaleToBank { get; set; }
        public string FundRegInvToBank { get; set; }
        public string FundSponsorSeminarsEduToBank { get; set; }
        public string FundOtherMarketingToBank { get; set; }
        public string FeePostCollectionType { get; set; }
        public string UpdateTime { get; set; }
    }
}