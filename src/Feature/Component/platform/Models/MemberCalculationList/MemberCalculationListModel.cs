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
}