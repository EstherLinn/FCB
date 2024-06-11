using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.News
{
    public class MarketNewsRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link GetMarketNewsListPage()
        {
            Item item = ItemUtils.GetItem(Templates.MarketNewsRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.MarketNewsRelatedLink.Fields.ListLink);
            return link;
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link GetMarketNewsDetailPage()
        {
            Item item = ItemUtils.GetItem(Templates.MarketNewsRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.MarketNewsRelatedLink.Fields.DetailLink);
            return link;
        }

        public static string GetMarketNewsListUrl()
        {
            return GetMarketNewsListPage()?.Url;
        }

        public static Item GetMarketNewsListPageItem()
        {
            return GetMarketNewsListPage()?.TargetItem;
        }

        public static string GetMarketNewsListPageItemId()
        {
            return GetMarketNewsListPageItem()?.ID.ToString();
        }

        public static string GetMarketNewsDetailUrl()
        {
            return GetMarketNewsDetailPage()?.Url;
        }

        public static Item GetMarketNewsDetailPageItem()
        {
            return GetMarketNewsDetailPage()?.TargetItem;
        }

        public static string GetMarketNewsDetailPageItemId()
        {
            return GetMarketNewsDetailPageItem()?.ID.ToString();
        }

        public struct Templates
        {
            public struct MarketNewsRelatedLink
            {
                public static readonly ID RootItemId = new ID("{B1AC6A99-3E1B-4CE6-9C8D-D03DBD288717}");
                public static readonly ID Id = new ID("{E4EB51CF-1843-4531-90CB-CBC78056A7CC}");

                public struct Fields
                {
                    public static readonly ID ListLink = new ID("{100C1B79-7E0D-49C1-A1F1-1CCA1921285B}");
                    public static readonly ID DetailLink = new ID("{1A44958A-1092-4B95-A3E3-D83E626BAF4D}");
                }
            }
        }
    }
}
