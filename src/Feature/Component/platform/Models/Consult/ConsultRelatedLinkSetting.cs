using Sitecore.Data;
using Sitecore.Data.Items;
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

        public static string GetConsultUrl()
        {
            return ConsultPage()?.Url;
        }

        public static Item GetConsultPageItem()
        {
            return ConsultPage()?.TargetItem;
        }

        public struct Template
        {
            public struct ConsultRelatedLink
            {
                public static readonly ID Root = new ID("{3966B7A6-C804-4DA4-8B20-C08B57585BDA}");

                public struct Fields
                {
                    public static readonly ID ConsultLink = new ID("{23DB4C97-B0FA-4229-BAE2-A15FF7260A89}");
                }
            }

        }
    }
}
