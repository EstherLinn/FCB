using Sitecore.Data;
using Sitecore.Data.Items;

using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public static class FundRelatedSettingModel
    {

        public struct Template
        {
            public struct FundRelatedLink
            {
                public static readonly ID Root = new ID("{0FC0B4B7-AB28-4C66-91A5-C9A7A45A5499}");

                public struct Fields
                {
                    public static readonly ID FundSearch = new ID("{D949379D-CDFC-45AE-9C20-91BF2A5058F6}");
                    public static readonly ID FundDetails = new ID("{E9D1628F-C48D-4353-8526-FEDAB06A8050}");
                    public static readonly ID FundCloseFiveYears = new ID("{8805B824-C351-4703-A3EC-D7C17EFCBD31}");
                    public static readonly ID FundExpose = new ID("{97A54554-F36A-4B0C-9C25-014FE8ECF596}");
                    public static readonly ID FundEtfCompare = new ID("{BF15DB31-1977-4605-959A-ADDBA77093E7}");

                }
            }

        }

        public static string GetFundSearchUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundSearch)?.Url;
        }
        public static string GetFundDetailsUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundDetails)?.Url;
        }
        public static string GetFundCloseFiveYearsUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundCloseFiveYears)?.Url;
        }
        public static string GetFundExposeUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundExpose)?.Url;
        }
        public static string GetFundEtfCompareUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundEtfCompare)?.Url;
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link FundDetailPage()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.FundRelatedLink.Root);
            var link = FundRealtedItem.GeneralLink(Template.FundRelatedLink.Fields.FundDetails);
            return link;
        }
        public static Item GetFundDetailPageItem()
        {
            return FundDetailPage()?.TargetItem;
        }

    }
}
