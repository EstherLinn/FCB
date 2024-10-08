using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.MemberCalculationList
{
    public class MemberCalculationListModel
    {
        public string PlatFormId { get; set; }
        public CalculationListData SavingList { get; set; }
        public CalculationListData EducationFundList { get; set; }
        public CalculationListData BuyHouseList { get; set; }
        public CalculationListData RetirementPreparationList { get; set; }
    }

    public class MemberCalculationListPdfModel
    {
        public CalculationListData Data { get; set; }
    }

    public class CalculationListData
    {
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
        public decimal target { get; set; }
        public decimal current { get; set; }
        public decimal gap { get; set; }
    }

    public class ChartsRevenuesData
    {
        public string id { get; set; }
        public decimal gap { get; set; }
        public decimal revenue { get; set; }
        public decimal invest { get; set; }
        public decimal prepared { get; set; }
        public decimal retire { get; set; }
    }

    public class ChartsRewardsData
    {
        public string id { get; set; }
        public decimal target { get; set; }
        public List<decimal?> gap { get; set; }
        public List<decimal> revenue { get; set; }
        public List<decimal> invest { get; set; }
        public List<decimal> prepared { get; set; }
        public List<decimal> retire { get; set; }
    }
}