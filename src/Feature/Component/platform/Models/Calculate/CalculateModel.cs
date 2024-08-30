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
        public string ConservativeRiskImage { get; set; }
        public int? ConservativeRiskStockAllocation { get; set; }
        public int? ConservativeRiskBondAllocation { get; set; }
        public int? ConservativeRiskCurrencyAllocation { get; set; }
        public string ConservativeRiskStockAllocationFieldName { get; set; }
        public string ConservativeRiskBondAllocationFieldName { get; set; }
        public string ConservativeRiskCurrencyAllocationFieldName { get; set; }
        public string ConservativeRiskStockAllocationText { get; set; }
        public string ConservativeRiskBondAllocationText { get; set; }
        public string ConservativeRiskCurrencyAllocationText { get; set; }
        public string ConservativeRiskNotice { get; set; }
        public string RobustRiskImage { get; set; }
        public int? RobustRiskStockAllocation { get; set; }
        public int? RobustRiskBondAllocation { get; set; }
        public int? RobustRiskCurrencyAllocation { get; set; }
        public string RobustRiskStockAllocationFieldName { get; set; }
        public string RobustRiskBondAllocationFieldName { get; set; }
        public string RobustRiskCurrencyAllocationFieldName { get; set; }
        public string RobustRiskStockAllocationText { get; set; }
        public string RobustRiskBondAllocationText { get; set; }
        public string RobustRiskCurrencyAllocationText { get; set; }
        public string RobustRiskNotice { get; set; }
        public string PositiveRiskImage { get; set; }
        public int? PositiveRiskStockAllocation { get; set; }
        public int? PositiveRiskBondAllocation { get; set; }
        public int? PositiveRiskCurrencyAllocation { get; set; }
        public string PositiveRiskStockAllocationFieldName { get; set; }
        public string PositiveRiskBondAllocationFieldName { get; set; }
        public string PositiveRiskCurrencyAllocationFieldName { get; set; }
        public string PositiveRiskStockAllocationText { get; set; }
        public string PositiveRiskBondAllocationText { get; set; }
        public string PositiveRiskCurrencyAllocationText { get; set; }
        public string PositiveRiskNotice { get; set; }
        public string RemoteConsultationSuccessTitle { get; set; }
        public string RemoteConsultationSuccessContent { get; set; }
        public string RemoteConsultationSuccessButtonText { get; set; }
        public string RemoteConsultationSuccessButtonLink { get; set; }
        public string RemoteConsultationSuccessImage { get; set; }
        public string RemoteConsultationSuccessfulTitle { get; set; }
        public string RemoteConsultationSuccessfulContent { get; set; }
        public string RemoteConsultationSuccessfulButtonText { get; set; }
        public string RemoteConsultationSuccessfulButtonLink { get; set; }
        public string RemoteConsultationSuccessfulImage { get; set; }
        public string[] FundID { get; set; }
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
        public List<EtfModel> ETFData { get; set; }
    }

    public class FundModel
    {
        public string ProductCode { get; set; }
        public string FundName { get; set; }
        public decimal? OneMonthReturnOriginalCurrency { get; set; }
        public string AvailabilityStatus { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public string DisplayOneMonthReturnOriginalCurrency { get; set; }
        public List<decimal> SysjustNavFundData { get; set; }
        public string FundDetailUrl { get; set; }
        public string FocusButtonHtml { get; set; }
        public string CompareButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }
        public bool DataIsFormSitecore { get; set; } = false;
    }

    public class EtfModel
    {
        public string ProductCode { get; set; }
        public string ETFName { get; set; }
        public decimal? MonthlyReturnNetValueOriginalCurrency { get; set; }
        public string AvailabilityStatus { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public string DisplayMonthlyReturnNetValueOriginalCurrency { get; set; }
        public List<decimal> SysjustNavEtfData { get; set; }
        public string ETFDetailUrl { get; set; }
        public string FocusButtonHtml { get; set; }
        public string CompareButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }
    }
}