namespace Feature.Wealth.Component.Models.News
{
    public class MarketNewsTableModel
    {
        public string NewsDate { get; set; }
        public string NewsTime { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSerialNumber { get; set; }
    }

    public class MarketNewsModel : MarketNewsTableModel
    {
        public string HotNews { get; set; }
        public string NewsType { get; set; }
        public int? NewsViewCount { get; set; }
        public string DisplayNewsViewCount { get; set; }
        public string NewsDetailLink { get; set; }
        public string value { get; set; }
        public MarketNewsData Data { get; set; }
    }

    public class MarketNewsData
    {
        /// <summary>
        /// autocomplete回傳內容
        /// </summary>
        public string Type { get; set; }
        public bool IsLogin { get; set; }
        public bool IsNews { get; set; }
        public bool IsLike { get; set; }
        public string DetailUrl { get; set; }
        public bool Purchase { get; set; }
    }
}