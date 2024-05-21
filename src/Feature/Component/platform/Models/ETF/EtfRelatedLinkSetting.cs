using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.ETF
{
    public class EtfRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link GetETFSearchPage()
        {
            Item item = ItemUtils.GetItem(Templates.EtfRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.EtfRelatedLink.Fields.ETFSearch);
            return link;
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link ETFDetailPage()
        {
            Item item = ItemUtils.GetItem(Templates.EtfRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.EtfRelatedLink.Fields.ETFDetail);
            return link;
        }

        public static string GetETFSearchUrl()
        {
            return GetETFSearchPage()?.Url;
        }

        public static Item GetETFSearchPageItem()
        {
            return GetETFSearchPage()?.TargetItem;
        }

        public static string GetETFDetailUrl()
        {
            return ETFDetailPage()?.Url;
        }

        public static Item GetETFDetailPageItem()
        {
            return ETFDetailPage()?.TargetItem;
        }

        public struct Templates
        {
            public struct EtfRelatedLink
            {
                public static readonly ID RootItemId = new ID("{7E546D5C-6D92-451B-8E56-D45AE14DDEE8}");
                public static readonly ID Id = new ID("{6F6FDF18-3FF0-4C5A-A7E5-8B4A91EEFDB2}");

                public struct Fields
                {
                    public static readonly ID ETFSearch = new ID("{AA370C28-50C7-4BBA-A1C4-6154E04E8036}");
                    public static readonly ID ETFDetail = new ID("{E0FA77EF-C5E8-4B8D-A6C7-3A149F729CF7}");
                }
            }
        }
    }
}