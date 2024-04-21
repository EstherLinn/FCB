using Sitecore.Data;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchResultModel
    {
        public string DetailPageLink { get; set; }

        public List<EtfSearchResult> ResultProducts { get; set; }
    }

    public struct Templates
    {
        public struct EtfSearchDatasource
        {
            public static readonly ID Id = new ID("{6664CA2A-3452-4F39-8F75-F18DEF5238DF}");

            public struct Fields
            {
                public static readonly ID DetailPageLink = new ID("{56695F44-A5FD-4BF8-8430-BD7B5D5B06A5}");
            }
        }
    }
}