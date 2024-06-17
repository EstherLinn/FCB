using System.Web;

namespace Feature.Wealth.Component.Models.News
{
    public class MarketNewsDetailModel
    {
        public MarketNewsDetailData MarketNewsDetailData { get; set; }
    }

    public class MarketNewsDetailData
    {
        public string NewsSerialNumber { get; set; }
        public string NewsDetailDate { get; set; }
        public string NewsTitle { get; set; }
        public string NewsContent { get; set; }
        public HtmlString NewsContentHtmlString { get; set; }
        public string NewsRelatedProducts { get; set; }
        public string NewsType { get; set; }
        public string NewsViewCount { get; set; }
        public string PreviousPageTitle { get; set; }
        public string PreviousPageLink { get; set; }
        public string PreviousPageId { get; set; }
        public string NextPageTitle { get; set; }
        public string NextPageLink { get; set; }
        public string NextPageId { get; set; }
        public string NewsListUrl { get; set; }
    }
}
