using System.Collections.Generic;

namespace Feature.Wealth.ScheduleAgent.Models.News.List
{
    public class NewsListDto
    {
        /// <summary>
        /// 新聞日期
        /// </summary>
        public string NewsDate { get; set; }

        /// <summary>
        /// 新聞時間
        /// </summary>
        public string NewsTime { get; set; }

        /// <summary>
        /// 新聞標題
        /// </summary>
        public string NewsTitle { get; set; }

        /// <summary>
        /// 新聞序號
        /// </summary>
        public string NewsSerialNumber { get; set; }
    }

    internal class NewsListDtoComparer : IEqualityComparer<NewsListDto>
    {
        public bool Equals(NewsListDto x, NewsListDto y)
        {
            return x.NewsSerialNumber == y.NewsSerialNumber;
        }

        public int GetHashCode(NewsListDto obj)
        {
            return obj.NewsSerialNumber.GetHashCode();
        }
    }
}