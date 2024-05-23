using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class FundNavTfjsNav
    {
        public string DataDate { get; set; }
        public string ISINCode { get; set; }
        public string BankProductCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public string FundCurrency { get; set; }
        public string NetAssetValue { get; set; }
        public string SubscriptionFee { get; set; }
        public string RedemptionFee { get; set; }
        public string DailyReturnTWD { get; set; }
        public string WeeklyReturnTWD { get; set; }
        public string OneMonthReturnTWD { get; set; }
        public string ThreeMonthReturnTWD { get; set; }
        public string SixMonthReturnTWD { get; set; }
        public string OneYearReturnTWD { get; set; }
        public string TwoYearReturnTWD { get; set; }
        public string ThreeYearReturnTWD { get; set; }
        public string FiveYearReturnTWD { get; set; }
        public string TenYearReturnTWD { get; set; }
        public string OneYearAlphaTWD { get; set; }
        public string ThreeYearAlphaTWD { get; set; }
        public string FiveYearAlphaTWD { get; set; }
        public string TenYearAlphaTWD { get; set; }
        public string OneYearBetaTWD { get; set; }
        public string ThreeYearBetaTWD { get; set; }
        public string FiveYearBetaTWD { get; set; }
        public string TenYearBetaTWD { get; set; }
        public string OneYearSharpeTWD { get; set; }
        public string ThreeYearSharpeTWD { get; set; }
        public string FiveYearSharpeTWD { get; set; }
        public string TenYearSharpeTWD { get; set; }
        public string OneYearStandardDeviationTWD { get; set; }
        public string ThreeYearStandardDeviationTWD { get; set; }
        public string FiveYearStandardDeviationTWD { get; set; }
        public string TenYearStandardDeviationTWD { get; set; }
        public string DailyReturnOriginalCurrency { get; set; }
        public string WeeklyReturnOriginalCurrency { get; set; }
        public string OneMonthReturnOriginalCurrency { get; set; }
        public string ThreeMonthReturnOriginalCurrency { get; set; }
        public string SixMonthReturnOriginalCurrency { get; set; }
        public string OneYearReturnOriginalCurrency { get; set; }
        public string TwoYearReturnOriginalCurrency { get; set; }
        public string ThreeYearReturnOriginalCurrency { get; set; }
        public string FiveYearReturnOriginalCurrency { get; set; }
        public string TenYearReturnOriginalCurrency { get; set; }
        public string OneYearAlphaOriginalCurrency { get; set; }
        public string ThreeYearAlphaOriginalCurrency { get; set; }
        public string FiveYearAlphaOriginalCurrency { get; set; }
        public string TenYearAlphaOriginalCurrency { get; set; }
        public string OneYearBetaOriginalCurrency { get; set; }
        public string ThreeYearBetaOriginalCurrency { get; set; }
        public string FiveYearBetaOriginalCurrency { get; set; }
        public string TenYearBetaOriginalCurrency { get; set; }
        public string OneYearSharpeOriginalCurrency { get; set; }
        public string ThreeYearSharpeOriginalCurrency { get; set; }
        public string FiveYearSharpeOriginalCurrency { get; set; }
        public string TenYearSharpeOriginalCurrency { get; set; }
        public string OneYearStandardDeviationOriginalCurrency { get; set; }
        public string ThreeYearStandardDeviationOriginalCurrency { get; set; }
        public string FiveYearStandardDeviationOriginalCurrency { get; set; }
        public string TenYearStandardDeviationOriginalCurrency { get; set; }
    }
}