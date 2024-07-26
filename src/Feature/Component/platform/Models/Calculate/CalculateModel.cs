using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.Calculate
{
    public class CalculateModel
    {
        public Item Datasource { get; set; }
        public string MainTitle { get; set; }
        public string MainContent { get; set; }
        public string Image { get; set; }
        public string AnticipatedInvestmentTipTitle { get; set; }
        public string AnticipatedInvestmentTipContent { get; set; }
        public string LifeExpectancyTipTitle { get; set; }
        public string LifeExpectancyTipContent { get; set; }
        public string RetirementExpensesTipTitle { get; set; }
        public string RetirementExpensesTipContent { get; set; }
        public string EstimatedPensionTipTitle { get; set; }
        public string EstimatedPensionTipContent { get; set; }
        public string SuccessImage { get; set; }
        public string SuccessContent { get; set; }
        public string UnsuccessfulImage { get; set; }
        public string UnsuccessfulContent { get; set; }
        public string ExpectedReturnRemarks { get; set; }
        public string DefaultRiskAttributes { get; set; }
        public int? StockAllocation { get; set; }
        public int? BondAllocation { get; set; }
        public int? CurrencyAllocation { get; set; }
        public string StockAllocationFieldName { get; set; }
        public string BondAllocationFieldName { get; set; }
        public string CurrencyAllocationFieldName { get; set; }
        public string StockAllocationText { get; set; }
        public string BondAllocationText { get; set; }
        public string CurrencyAllocationText { get; set; }
        public string Notice { get; set; }
        public string RemoteConsultationTitle { get; set; }
        public string RemoteConsultationContent { get; set; }
        public string RemoteConsultationButtonText { get; set; }
        public string RemoteConsultationButtonLink { get; set; }
        public string RemoteConsultationImage { get; set; }
    }

    public class CalculationResultData
    {
        public string PlatFormId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        public bool ResultHasGap { get; set; }
        public string Description { get; set; }
        public List<string> EarningsChart { get; set; }
        public ReadingbarData Readingbar { get; set; }
        public List<ChartsRevenuesData> ChartsRevenues { get; set; }
        public List<ChartsRewardsData> ChartsRewards { get; set; }
        public List<string> ChartsRewardsCategories { get; set; }
    }

    public class ReadingbarData
    {
        public int target { get; set; }
        public int current { get; set; }
        public int gap { get; set; }
    }

    public class ChartsRevenuesData
    {
        public string id { get; set; }
        public int gap { get; set; }
        public int revenue { get; set; }
        public int invest { get; set; }
        public int prepared { get; set; }
        public int retire { get; set; }
    }

    public class ChartsRewardsData
    {
        public string id { get; set; }
        public int target { get; set; }
        public List<int?> gap { get; set; }
        public List<int> revenue { get; set; }
        public List<int> invest { get; set; }
        public List<int> prepared { get; set; }
        public List<int> retire { get; set; }
    }

    public class RecommendedProducts
    {
        public List<FundModel> FundData { get; set; }
    }

    public class FundModel
    {
        public string ProductCode { get; set; }

        public string FundName { get; set; }

        public decimal? OneMonthReturnOriginalCurrency { get; set; }

        public string DisplayOneMonthReturnOriginalCurrency { get; set; }

        public List<decimal> SysjustNavFundData { get; set; }
    }
}