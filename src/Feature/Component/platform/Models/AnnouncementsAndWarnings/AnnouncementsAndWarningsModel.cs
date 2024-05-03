using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.AnnouncementsAndWarnings
{
    public class AnnouncementsAndWarningsModel
    {
        public Item Item { get; set; }

        public string Content => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.Content);
        public string Image { get; set; }
        public string MainTitle => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.MainTitle);
        public string Description => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.Description);
        public string ButtonText => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.ButtonText);
        public string ButtonLink { get; set; }
        public string WarningTitle => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.WarningTitle);
        public string WarningContent => ItemUtils.GetFieldValue(Item, Templates.AnnouncementsandWarnings.Fields.WarningContent);
        public AnnouncementsAndWarningsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Item = item;
            this.Image = ItemUtils.ImageUrl(item, Templates.AnnouncementsandWarnings.Fields.Image);
            this.ButtonLink = ItemUtils.GeneralLink(item, Templates.AnnouncementsandWarnings.Fields.ButtonLink)?.Url;
        }
    }

    public struct Templates
    {
        public struct AnnouncementsandWarnings
        {
            public static readonly ID Id = new ID("{D736CE92-7180-4579-B707-A133E61156A0}");

            public struct Fields
            {
                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{8C95E0D1-E889-4705-9D5C-67311B970DF3}");

                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{165CF1CD-164B-4E21-88B3-DBF05EE58192}");

                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{CB6B7B75-4AB8-4F7A-9892-C4555C86DEE2}");

                /// <summary>
                /// 內容
                /// </summary>
		        public static readonly ID Description = new ID("{02FB31B5-E46C-4AD4-9F07-174C6B1D95FB}");

                /// <summary>
                /// 按鈕文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{429092D1-9938-4B19-8021-894E95F67F85}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
		        public static readonly ID ButtonLink = new ID("{8B0EFC2E-0525-446E-8352-42A42B2E072F}");

                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID WarningTitle = new ID("{9D8E6F2A-E3EF-4942-A52D-180B40873A9B}");

                /// <summary>
                /// 內容
                /// </summary>
		        public static readonly ID WarningContent = new ID("{8492D50C-E6FC-4565-871E-C38C9D32C546}");
            }
        }
    }
}
