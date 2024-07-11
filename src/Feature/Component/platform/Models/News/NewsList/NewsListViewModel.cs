using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.News.NewsList
{
    public class NewsListViewModel
    {
        public string DatasourceId { get; set; }

        /// <summary>
        /// 新聞類別
        /// </summary>
        public IEnumerable<string> CategoryList { get; set; }
    }
}
