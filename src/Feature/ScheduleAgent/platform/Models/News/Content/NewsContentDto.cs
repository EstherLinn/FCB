namespace Feature.Wealth.ScheduleAgent.Models.News.Content
{
    public class NewsContentDto
    {
        /// <summary>
        /// 新聞序號
        /// </summary>
        public string NewsSerialNumber { get; set; }

        /// <summary>
        /// 新聞日期/時間
        /// </summary>
        public string NewsDetailDate { get; set; }

        /// <summary>
        /// 新聞標題
        /// </summary>
        public string NewsTitle { get; set; }

        /// <summary>
        /// 新聞內文
        /// </summary>
        public string NewsContent { get; set; }

        /// <summary>
        /// 新聞相關商品
        /// </summary>
        public string NewsRelatedProducts { get; set; }

        /// <summary>
        /// 新聞類別
        /// </summary>
        public string NewsType { get; set; }
    }
}