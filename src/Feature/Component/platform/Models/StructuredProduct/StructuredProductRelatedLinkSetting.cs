using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public class StructuredProductRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link GetStructuredProductSearchPage()
        {
            Item item = ItemUtils.GetItem(Templates.StructuredProductRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.StructuredProductRelatedLink.Fields.StructuredProductSearchLink);
            return link;
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link GetStructuredProductDetailPage()
        {
            Item item = ItemUtils.GetItem(Templates.StructuredProductRelatedLink.RootItemId);
            var link = item.GeneralLink(Templates.StructuredProductRelatedLink.Fields.StructuredProductDetailLink);
            return link;
        }

        public static string GetStructuredProductSearchUrl()
        {
            return GetStructuredProductSearchPage()?.Url;
        }

        public static Item GetStructuredProductSearchPageItem()
        {
            return GetStructuredProductSearchPage()?.TargetItem;
        }

        public static string GetStructuredProductSearchPageItemId()
        {
            return GetStructuredProductSearchPageItem()?.ID.ToString();
        }

        public static string GetStructuredProductDetailUrl()
        {
            return GetStructuredProductDetailPage()?.Url;
        }

        public static Item GetStructuredProductDetailPageItem()
        {
            return GetStructuredProductDetailPage()?.TargetItem;
        }

        public static string GetStructuredProductDetailPageItemId()
        {
            return GetStructuredProductDetailPageItem()?.ID.ToString();
        }

        public struct Templates
        {
            public struct StructuredProductRelatedLink
            {
                public static readonly ID RootItemId = new ID("{E5558887-45FB-48B8-AE13-5C284635613A}");
                public static readonly ID Id = new ID("{1FC0823F-87FC-4621-8248-ABDB4491791B}");

                public struct Fields
                {
                    public static readonly ID StructuredProductSearchLink = new ID("{B7A297BB-F52D-4E39-8EBF-718C12003201}");
                    public static readonly ID StructuredProductDetailLink = new ID("{1B8382DD-8309-4366-8A73-0FFC61CD04BC}");
                }
            }
        }
    }
}
