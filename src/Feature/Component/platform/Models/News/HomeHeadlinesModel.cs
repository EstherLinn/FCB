using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.News
{
    public class HomeHeadlinesModel
    {
        public Item Datasource { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public HomeHeadlinesData LatestHeadlines { get; set; }
        public List<HomeHeadlinesData> Headlines { get; set; }
    }

    public class HomeHeadlinesData
    {
        public string NewsImage { get; set; }
        public string NewsDate { get; set; }
        public string NewsTime { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSerialNumber { get; set; }
        public string NewsDetailLink { get; set; }
    }
}