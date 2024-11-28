using Sitecore.Configuration;
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

        public static bool GetNeedEmployeeIPCheck()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GetFieldValue(Template.ConsultRelatedLink.Fields.NeedEmployeeIPCheck) == "1";
        }

        public static bool GetSkipIMVPAPI()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GetFieldValue(Template.ConsultRelatedLink.Fields.SkipIMVPAPI) == "1";
        }

        public static bool GetUseTestEmail()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return Settings.GetSetting("ConsultUseTestEmail") == "1" && item.GetFieldValue(Template.ConsultRelatedLink.Fields.UseTestEmail) == "1";
        }

        public static string GetTestEmail()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GetFieldValue(Template.ConsultRelatedLink.Fields.TestEmail);
        }

        public static bool GetIsMaintain()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GetFieldValue(Template.ConsultRelatedLink.Fields.IsMaintain) == "1";
        }

        public static string GetMaintainInfo()
        {
            Item item = ItemUtils.GetItem(Template.ConsultRelatedLink.Root);
            return item.GetFieldValue(Template.ConsultRelatedLink.Fields.MaintainInfo);
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

                    public static readonly ID NeedEmployeeIPCheck = new ID("{616A86FF-47D3-48C6-B682-EB879AD80194}");
                    public static readonly ID SkipIMVPAPI = new ID("{CBC2E5FE-FFA1-47E5-8ECB-B8692DA96E6F}");

                    public static readonly ID UseTestEmail = new ID("{CF5A5BF5-B530-4ABB-8D88-AA54FC27FA35}");
                    public static readonly ID TestEmail = new ID("{A1CC4A9D-B62D-4E87-BCB9-198646633A90}");

                    public static readonly ID IsMaintain = new ID("{F73AB36F-8E2F-4C54-99F3-4D089DC4F5C3}");
                    public static readonly ID MaintainInfo = new ID("{9C26D94D-6591-49B8-8BB5-9BD36EA3BAD8}");
                }
            }

        }
    }
}
