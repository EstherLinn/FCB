using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.GlobalIndex
{
    public class GlobalIndexModel
    {
        public Item Item { get; set; }
        public string DetailLink { get; set; }
        public IList<GlobalIndex> GlobalIndexList { get; set; }
        public Dictionary<string, GlobalIndex> GlobalIndexDictionary { get; set; }
    }

    public class GlobalIndex
    {
        public string IndexCode { get; set; }
        public string IndexName { get; set; }
        public string IndexCategoryID { get; set; }
        public string IndexCategoryName { get; set; }
        public string DataDate { get; set; }
        public string MarketPrice { get; set; }
        public string MarketPriceText { get; set; }
        public string Change { get; set; }
        public string ChangePercentage { get; set; }
        public bool UpOrDown { get; set; }
        public IList<GlobalIndex> GlobalIndexHistory { get; set; }
        public string GlobalIndexHistoryJson { get; set; }
        public string DetailLink { get; set; }
    }

    public struct Template
    {
        public readonly struct GlobalIndex
        {
            public static readonly ID Id = new ID("{B54DB03A-8CB6-45C7-ACCB-F84B87593659}");

            public readonly struct Fields
            {
                public static readonly ID DetailLink = new ID("{5F5A949F-D1E7-4186-B33E-2F18D9F0EDB6}");
            }
        }
    }
}