﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustReturnFund
    {
        public string FirstBankCode { get; set; }
        public string SysjustCode { get; set; }
        public string OneDayReturnOriginalCurrency { get; set; }
        public string OneWeekReturnOriginalCurrency { get; set; }
        public string MonthtoDateReturnOriginalCurrency { get; set; }
        public string YeartoDateReturnOriginalCurrency { get; set; }
        public string OneMonthReturnOriginalCurrency { get; set; }
        public string ThreeMonthReturnOriginalCurrency { get; set; }
        public string SixMonthReturnOriginalCurrency { get; set; }
        public string OneYearReturnOriginalCurrency { get; set; }
        public string TwoYearReturnOriginalCurrency { get; set; }
        public string ThreeYearReturnOriginalCurrency { get; set; }
        public string FiveYearReturnOriginalCurrency { get; set; }
        public string TenYearReturnOriginalCurrency { get; set; }
        public string InceptionDateReturnOriginalCurrency { get; set; }
        public string OneWeekReturnTWD { get; set; }
        public string MonthtoDateReturnTWD { get; set; }
        public string YeartoDateReturnTWD { get; set; }
        public string OneMonthReturnTWD { get; set; }
        public string ThreeMonthReturnTWD { get; set; }
        public string SixMonthReturnTWD { get; set; }
        public string OneYearReturnTWD { get; set; }
        public string TwoYearReturnTWD { get; set; }
        public string ThreeYearReturnTWD { get; set; }
        public string FiveYearReturnTWD { get; set; }
        public string TenYearReturnTWD { get; set; }
        public string InceptionDateReturnTWD { get; set; }
        public string MonthtoDateReturnTWDRegularInvestment { get; set; }
        public string YeartoDateReturnTWDRegularInvestment { get; set; }
        public string OneMonthReturnTWDRegularInvestment { get; set; }
        public string ThreeMonthReturnTWDRegularInvestment { get; set; }
        public string SixMonthReturnTWDRegularInvestment { get; set; }
        public string OneYearReturnTWDRegularInvestment { get; set; }
        public string TwoYearReturnTWDRegularInvestment { get; set; }
        public string ThreeYearReturnTWDRegularInvestment { get; set; }
        public string FiveYearReturnTWDRegularInvestment { get; set; }
        public string TenYearReturnTWDRegularInvestment { get; set; }
        public string InceptionDateReturnTWDRegularInvestment { get; set; }
        public string MonthtoDateReturnOriginalCurrencyRegularInvestment { get; set; }
        public string YeartoDateReturnOriginalCurrencyRegularInvestment { get; set; }
        public string OneMonthReturnOriginalCurrencyRegularInvestment { get; set; }
        public string ThreeMonthReturnOriginalCurrencyRegularInvestment { get; set; }
        public string SixMonthReturnOriginalCurrencyRegularInvestment { get; set; }
        public string OneYearReturnOriginalCurrencyRegularInvestment { get; set; }
        public string TwoYearReturnOriginalCurrencyRegularInvestment { get; set; }
        public string ThreeYearReturnOriginalCurrencyRegularInvestment { get; set; }
        public string FiveYearReturnOriginalCurrencyRegularInvestment { get; set; }
        public string TenYearReturnOriginalCurrencyRegularInvestment { get; set; }
        public string InceptionDateReturnOriginalCurrencyRegularInvestment { get; set; }
        public string MonthtoDateReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string OneMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string ThreeMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string SixMonthReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string OneYearReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string TwoYearReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string ThreeYearReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string FiveYearReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string TenYearReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string InceptionDateReturnOriginalCurrencyAnnualizedReturn { get; set; }
        public string OneMonthReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string ThreeMonthReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string SixMonthReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string OneYearReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string TwoYearReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string ThreeYearReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string FiveYearReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string TenYearReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string InceptionDateReturnRegularInvestmentAnnualizedReturn { get; set; }
        public string AnnualizedStandardDeviation { get; set; }
        public string Sharpe { get; set; }
        public string Beta { get; set; }
        public string DataDate { get; set; }
    }
}