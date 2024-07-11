using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.News.NewsList
{
    public class NewsListFilterModel
    {
        /// <summary>
        /// 新聞類別
        /// </summary>
        public IEnumerable<string> CategoryList { get; set; }
    }
}
