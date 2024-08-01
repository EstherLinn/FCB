using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Consult
{
    public static class ConsultRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link ConsultPage()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GeneralLink(Template.ConsultRelatedLink.Fields.ConsultLink);
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link ConsultListPage()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GeneralLink(Template.ConsultRelatedLink.Fields.ConsultListLink);
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link ConsultSchedulePage()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GeneralLink(Template.ConsultRelatedLink.Fields.ConsultScheduleLink);
        }

        public static string GetConsultUrl()
        {
            return ConsultPage()?.Url;
        }

        public static Item GetConsultPageItem()
        {
            return ConsultPage()?.TargetItem;
        }

        public static string GetConsultListUrl()
        {
            return ConsultListPage()?.Url;
        }

        public static Item GetConsultListPageItem()
        {
            return ConsultListPage()?.TargetItem;
        }

        public static string GetConsultScheduleUrl()
        {
            return ConsultSchedulePage()?.Url;
        }

        public static Item GetConsultSchedulePageItem()
        {
            return ConsultSchedulePage()?.TargetItem;
        }

        public struct Template
        {
            public struct ConsultRelatedLink
            {
                public static readonly ID Root = new ID("{3966B7A6-C804-4DA4-8B20-C08B57585BDA}");

                public struct Fields
                {
                    public static readonly ID ConsultLink = new ID("{23DB4C97-B0FA-4229-BAE2-A15FF7260A89}");
                    public static readonly ID ConsultListLink = new ID("{E770630B-C8E6-443C-8387-1D3525B11151}");
                    public static readonly ID ConsultScheduleLink = new ID("{E10AE7C2-4C31-4A92-AB8D-DD82898DFCD1}");
                }
            }

        }
    }
}
