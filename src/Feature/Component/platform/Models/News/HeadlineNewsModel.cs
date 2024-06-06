using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.News
{
    public class HeadlineNewsModel
    {
        public HeadlineNewsData LatestHeadlines { get; set; }
        public List<HeadlineNewsData> Headlines { get; set; }
    }

    public class HeadlineNewsData
    {
        public string NewsDate { get; set; }
        public string NewsTime { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSerialNumber { get; set; }
        public string NewsViewCount { get; set; }
        public string NewsDetailLink { get; set; }
    }
}