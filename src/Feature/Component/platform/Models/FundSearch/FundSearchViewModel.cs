using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FundSearch
{
    public class FundSearchViewModel
    {
        public Item Item { get; set; }
        public List<FundSearchModel> FundSearchData { get; set; }
        public SearchBarData SearchBarData { get; set; }
        public List<Item> HotKeywordTags { get; set; }
        public List<Item> HotProductTags { get; set; }
    }
}
