using Sitecore.Data;
using System.Collections.Generic;
using System.Web;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchResultModel
    {
        public string DetailPageLink { get; set; }

        public HtmlString MarketPricePerformanceIntro { get; set; }

        public HtmlString NetWorthPerformanceIntro { get; set; }

        public HtmlString MarketPriceRiskIntro { get; set; }

        public HtmlString NetWorthRiskIntro { get; set; }

        public HtmlString InformationIntro { get; set; }

        public HtmlString TransactionStatusIntro { get; set; }

        public List<EtfSearchResult> ResultProducts { get; set; }
    }

    public struct Templates
    {
        public struct EtfSearchDatasource
        {
            public static readonly ID Id = new ID("{6664CA2A-3452-4F39-8F75-F18DEF5238DF}");

            public struct Fields
            {
                public static readonly ID MarketPricePerformanceIntro = new ID("{A86ED3AF-1FA9-45F3-B3AE-0C6ACFD1B4BE}");
                public static readonly ID NetWorthPerformanceIntro = new ID("{416BAEBB-0278-49A4-AEAF-4DD489E46634}");
                public static readonly ID MarketPriceRiskIntro = new ID("{10E0B26A-7B4C-48A3-B112-0F90C10481D4}");
                public static readonly ID NetWorthRiskIntro = new ID("{222994D3-23C5-4B79-BF60-6FFE59EA5F8D}");
                public static readonly ID TransactionStatusIntro = new ID("{50445FDF-AE92-432D-A723-2D22C8858B9D}");
                public static readonly ID InformationIntro = new ID("{4EA3226D-9632-4F66-88AF-CD34AD274572}");
                public static readonly ID HotKeywords = new ID ("{51B8F96A-A2EE-4478-929A-900040F5931F}");
                public static readonly ID HotProduct = new ID("{26A932EB-7E4F-4DE6-9AD9-205FBC9BAFE5}");
            }
        }
    }
}