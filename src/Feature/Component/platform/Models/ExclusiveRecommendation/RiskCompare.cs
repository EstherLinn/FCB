using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ExclusiveRecommendation
{
    /// <summary>
    /// 風險屬性對應
    /// </summary>
    public static class RiskCompare
    {
        public static readonly Dictionary<string, List<string>> RiskCompareDic = new Dictionary<string, List<string>>
    {
        { "1", new List<string> { "RR1", "RR2"} },
        { "2", new List<string> { "RR1", "RR2","RR3","RR4" } },
        { "3", new List<string> { "RR1", "RR2","RR3","RR4","RR5" }  },
        { "4", new List<string> { "RR1" }  }
    };
    }
}
