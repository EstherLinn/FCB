using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Web;

namespace Feature.Wealth.Component.Models.FundSearch
{
    public class FundSearchViewModel
    {
        public Item Item { get; set; }
        public IList<FundSearchModel> FundSearchData { get; set; }
        public SearchBarData SearchBarData { get; set; }
        public List<string> HotKeywordTags { get; set; }
        public List<string> HotProductTags { get; set; }
        public List<string> TopicNameTags { get; set; }
        public string Content { get; set; }
        public HtmlString RiskIndicatorContent { get; set; }
        public HtmlString SharpeContent { get; set; }
        public HtmlString BetaContent { get; set; }
        public HtmlString AlphaContent { get; set; }
        public HtmlString StandardDeviationContent { get; set; }
        public HtmlString AnnualizedStandardDeviationContent { get; set; }
    }
}
