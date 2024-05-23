using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnEtf
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public string MarketPrice { get; set; }
        public string NetAssetValue { get; set; }
        public string QuoteCurrency { get; set; }
        public string AnnualizedStandardDeviationNetValueRisk { get; set; }
        public string SharpeNetValueRisk { get; set; }
        public string BetaNetValueRisk { get; set; }
        public string AnnualizedStandardDeviationMarketPriceRisk { get; set; }
        public string SharpeRatioMarketPriceRisk { get; set; }
        public string BetaMarketPriceRisk { get; set; }
        public string YeartoDateReturnNetValueOriginalCurrency { get; set; }
        public string DailyReturnNetValueOriginalCurrency { get; set; }
        public string WeeklyReturnNetValueOriginalCurrency { get; set; }
        public string MonthlyReturnNetValueOriginalCurrency { get; set; }
        public string ThreeMonthReturnNetValueOriginalCurrency { get; set; }
        public string SixMonthReturnNetValueOriginalCurrency { get; set; }
        public string OneYearReturnNetValueOriginalCurrency { get; set; }
        public string TwoYearReturnNetValueOriginalCurrency { get; set; }
        public string ThreeYearReturnNetValueOriginalCurrency { get; set; }
        public string FiveYearReturnNetValueOriginalCurrency { get; set; }
        public string TenYearReturnNetValueOriginalCurrency { get; set; }
        public string YeartoDateReturnMarketPriceOriginalCurrency { get; set; }
        public string DailyReturnMarketPriceOriginalCurrency { get; set; }
        public string WeeklyReturnMarketPriceOriginalCurrency { get; set; }
        public string MonthlyReturnMarketPriceOriginalCurrency { get; set; }
        public string ThreeMonthReturnMarketPriceOriginalCurrency { get; set; }
        public string SixMonthReturnMarketPriceOriginalCurrency { get; set; }
        public string OneYearReturnMarketPriceOriginalCurrency { get; set; }
        public string TwoYearReturnMarketPriceOriginalCurrency { get; set; }
        public string ThreeYearReturnMarketPriceOriginalCurrency { get; set; }
        public string FiveYearReturnMarketPriceOriginalCurrency { get; set; }
        public string TenYearReturnMarketPriceOriginalCurrency { get; set; }
        public string YeartoDateReturnNetValueTWD { get; set; }
        public string DailyReturnNetValueTWD { get; set; }
        public string WeeklyReturnNetValueTWD { get; set; }
        public string MonthlyReturnNetValueTWD { get; set; }
        public string ThreeMonthReturnNetValueTWD { get; set; }
        public string SixMonthReturnNetValueTWD { get; set; }
        public string OneYearReturnNetValueTWD { get; set; }
        public string TwoYearReturnNetValueTWD { get; set; }
        public string ThreeYearReturnNetValueTWD { get; set; }
        public string FiveYearReturnNetValueTWD { get; set; }
        public string TenYearReturnNetValueTWD { get; set; }
        public string YeartoDateReturnMarketPriceTWD { get; set; }
        public string DailyReturnMarketPriceTWD { get; set; }
        public string WeeklyReturnMarketPriceTWD { get; set; }
        public string MonthlyReturnMarketPriceTWD { get; set; }
        public string ThreeMonthReturnMarketPriceTWD { get; set; }
        public string SixMonthReturnMarketPriceTWD { get; set; }
        public string OneYearReturnMarketPriceTWD { get; set; }
        public string TwoYearReturnMarketPriceTWD { get; set; }
        public string ThreeYearReturnMarketPriceTWD { get; set; }
        public string FiveYearReturnMarketPriceTWD { get; set; }
        public string TenYearReturnMarketPriceTWD { get; set; }
        public string DiscountPremium { get; set; }
        public string LatestVolumeTradingVolume { get; set; }
        public string TenDayAverageVolume { get; set; }
        public string LatestVolumeTradingVolumeTenDayAverageVolume { get; set; }
        public string MonthtoDateNetValueOriginalCurrency { get; set; }
        public string MonthtoDateNetValueTWD { get; set; }
        public string MonthtoDateMarketPriceOriginalCurrency { get; set; }
        public string MonthtoDateMarketPriceTWD { get; set; }
        public string InceptionDateMarketPriceOriginalCurrency { get; set; }
        public string InceptionDateMarketPriceTWD { get; set; }
        public string InceptionDateNetValueOriginalCurrency { get; set; }
        public string InceptionDateNetValueTWD { get; set; }
    }
}