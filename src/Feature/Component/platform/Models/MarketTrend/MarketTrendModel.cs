using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Web;

namespace Feature.Wealth.Component.Models.MarketTrend
{
    public class MarketTrendModel
    {
        public Item Item { get; set; }
        public string IndexLink { get; set; }
        public string FundLink { get; set; }
        public string ETFLink { get; set; }
        public string NewsLink { get; set; }
        public string MoreNewsButtonText { get; set; }
        public string MoreNewsButtonLink { get; set; }
        public string MoreETFButtonText { get; set; }
        public string MoreETFButtonLink { get; set; }
        public string MoreFundButtonText { get; set; }
        public string MoreFundButtonLink { get; set; }
        public string ReportTitle { get; set; }
        public string ReportText { get; set; }
        public string ReportButtonText { get; set; }
        public string ReportButtonLink { get; set; }
        public List<MarketTrend> Stock { get; set; } = new List<MarketTrend>();
        public List<MarketTrend> Bond { get; set; } = new List<MarketTrend>();
        public List<MarketTrend> Industry { get; set; } = new List<MarketTrend>();
        public HtmlString GlobalIndexHighchartsDataHtmlString { get; set; }
        public HtmlString StockRelevantFundHtmlString { get; set; }
        public HtmlString StockRelevantETFHtmlString { get; set; }
        public HtmlString BondRelevantFundHtmlString { get; set; }
        public HtmlString BondRelevantETFHtmlString { get; set; }
        public HtmlString IndustryRelevantFundHtmlString { get; set; }
        public HtmlString IndustryRelevantETFHtmlString { get; set; }
    }

    public class MarketTrend
    {
        public string Title { get; set; }
        public string Degree { get; set; }
        public string DegreeText { get; set; }
        public string Date { get; set; }
        public List<News> News { get; set; } = new List<News>();
        public List<List<GlobalIndex.GlobalIndex>> RelevantGlobalIndex { get; set; } = new List<List<GlobalIndex.GlobalIndex>>();
        public string Checked { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string Display { get; set; } = "display:none";
    }

    public class RelevantInformation
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public decimal? NetAssetValue { get; set; }
        public string NetAssetValueDate { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyLink { get; set; }
        public decimal? Change { get; set; }
        public decimal? M6Change { get; set; }
        public string DetailLink { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public string AvailabilityStatus { get; set; }
        public bool IsLogin { get; set; }
        public bool IsLike { get; set; }
        public string CurrencyLinkHtml { get; set; }
        public string FocusButtonHtml { get; set; }
        public string CompareButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }
    }

    public class News
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string ID { get; set; }
        public string DetailLink { get; set; }
        public int ViewCount { get; set; }
        public bool IsLike { get; set; }
    }

    public struct Template
    {
        public readonly struct MarketTrend
        {
            public static readonly ID Id = new ID("{A3534F1C-3359-4ADD-9FB2-ECAA305FCAAB}");

            public readonly struct Fields
            {
                public static readonly ID IndexLink = new ID("{7D72D69C-A56C-4C73-8B15-4AE7DC2E9C50}");

                // 這個些有給連結方法所以沒用了，但先留著
                public static readonly ID FundLink = new ID("{7185BBB2-D663-4827-924B-8777A8252EC2}");
                public static readonly ID ETFLink = new ID("{AD39F7C8-9A6D-457A-A308-9B14755805E1}");
                public static readonly ID NewsLink = new ID("{F61C2BDF-6644-4A7D-93AD-D4C388F2CCAD}");

                public static readonly ID MoreNewsButtonText = new ID("{D8B67B5C-D297-4C84-9EB3-E12A10F704C1}");
                public static readonly ID MoreNewsButtonLink = new ID("{4D235F0A-6908-4AF1-9443-DA0BB3728F50}");
                public static readonly ID MoreETFButtonText = new ID("{76AE3A7E-DD25-4505-97C2-EDAB73645A4F}");
                public static readonly ID MoreETFButtonLink = new ID("{F3FB886B-F445-4AA9-A185-2544A8A4944A}");
                public static readonly ID MoreFundButtonText = new ID("{FD065EFA-FE87-4D4F-905D-1303D96BEEB9}");
                public static readonly ID MoreFundButtonLink = new ID("{E4471620-F181-4075-A07B-93B732BD2601}");
                public static readonly ID ReportTitle = new ID("{B2CEDDFE-47D7-42FC-8343-2A8A861A8B8B}");
                public static readonly ID ReportText = new ID("{636D691D-767D-45E6-BE0D-6AFEE3766CF0}");
                public static readonly ID ReportButtonText = new ID("{BAF6878F-4E4A-4636-86BF-C61283CD6D7B}");
                public static readonly ID ReportButtonLink = new ID("{AFDD2409-8E38-4D97-9BEE-0E441F90C9FD}");
            }
        }

        public readonly struct MarketTrendItem
        {
            public static readonly ID Id = new ID("{70ACAF25-6D62-4605-960C-D7060A9E8F48}");

            public readonly struct Fields
            {
                public static readonly ID Title = new ID("{E389FE2B-8C1F-427B-A4F0-BA6FD8398AE2}");
                public static readonly ID Degree = new ID("{A60C3026-A664-42EC-AAF1-E8F47A4DA22C}");
                public static readonly ID Date = new ID("{5EFF629C-B5B0-4763-A204-30017E29D1FD}");
                public static readonly ID ETFCode = new ID("{60173C85-18D1-4370-9AA2-EFE762A2AECD}");
                public static readonly ID FundCode = new ID("{B0EDBA01-9064-408F-B51E-90727EB6FED6}");
                public static readonly ID IndexCode = new ID("{FD5253D9-1464-4F5F-BFCF-EB49E6C32001}");
                public static readonly ID NewsList = new ID("{88AC7917-1345-433E-9541-BA879B64ADA6}");
            }
        }

        public readonly struct IndexItem
        {
            public static readonly ID Id = new ID("{EBE0EF3C-BFA5-4C12-B36E-B99D6F3622AB}");

            public readonly struct Fields
            {
                public static readonly ID IndexCode = new ID("{D6DA0444-AC40-4424-AD0A-8AD3100B8AE1}");
                public static readonly ID IndexName = new ID("{2B9478EB-3E6B-4262-9FC6-976FB501C2FC}");
            }
        }

        public readonly struct Stock
        {
            public static readonly ID Id = new ID("{64476F41-3DEF-4169-87B8-D6E01D84705E}");
        }

        public readonly struct Bond
        {
            public static readonly ID Id = new ID("{9C0C6F5F-9D6E-4110-8094-4515564CDC49}");
        }

        public readonly struct Industry
        {
            public static readonly ID Id = new ID("{8F851745-0B44-407B-9A09-5EA100FCB1A6}");
        }

        public readonly struct DropdownOption
        {
            public static readonly ID Id = new ID("{362DF993-E969-4243-898A-B01297B4B18A}");

            public readonly struct Fields
            {
                public static readonly ID OptionText = new ID("{8532457A-4AF0-488D-8C45-34AC0AE7A859}");
                public static readonly ID OptionValue = new ID("{B7E1B3B4-5A73-4C5E-A67B-C7DD68779F83}");
            }
        }
    }

    public enum RelevantInformationType
    {
        Stock = 1,
        Bond = 2,
        Industry = 3
    }
}