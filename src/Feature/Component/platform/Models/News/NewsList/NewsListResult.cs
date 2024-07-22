using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.News.NewsList
{
    public class NewsListResult
    {
        public string Id { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public KeyValuePair<string, string> PageTitlePair { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public KeyValuePair<long, string> DatePair { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public KeyValuePair<string, string> CategoryPair { get; set; }

        /// <summary>
        /// 焦點
        /// </summary>
        public KeyValuePair<int, bool> FocusPair { get; set; }

        /// <summary>
        /// 最新消息連結
        /// </summary>
        public string Url { get; set; }

        public string Target { get; set; }
    }
}