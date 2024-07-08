using Sitecore.Data.Items;

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
        public string Notice { get; set; }
        public string RemoteConsultationTitle { get; set; }
        public string RemoteConsultationContent { get; set; }
        public string RemoteConsultationButtonText { get; set; }
        public string RemoteConsultationButtonLink { get; set; }
        public string RemoteConsultationImage { get; set; }
    }
}
