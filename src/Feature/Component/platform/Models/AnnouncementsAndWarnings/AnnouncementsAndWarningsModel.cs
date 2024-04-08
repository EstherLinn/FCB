using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.AnnouncementsAndWarnings
{
    public class AnnouncementsAndWarningsModel
    {
        public Item Item { get; set; }
        public string ImageUrl { get; set; }
        public string Image3xUrl { get; set; }
        public string ButtonLink { get; set; }
    }

    public struct Templates
    {
        public struct Announcements
        {
            public static readonly ID Id = new ID("{D736CE92-7180-4579-B707-A133E61156A0}");

            public struct Fields
            {
                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{8C95E0D1-E889-4705-9D5C-67311B970DF3}");
            }
        }

        public struct MoreAnnouncement
        {
            public static readonly ID Id = new ID("{27779AA7-DA2F-485F-97E9-A8810CC65E28}");

            public struct Fields
            {
                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{165CF1CD-164B-4E21-88B3-DBF05EE58192}");

                /// <summary>
                /// 圖片3x
                /// </summary>
                public static readonly ID Image3x = new ID("{BC43F2D5-EB08-4404-8DE3-1938EC76D656}");

                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{CB6B7B75-4AB8-4F7A-9892-C4555C86DEE2}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Description = new ID("{02FB31B5-E46C-4AD4-9F07-174C6B1D95FB}");

                /// <summary>
                /// 按鈕文字 (預留欄位)
                /// </summary>
                public static readonly ID ButtonText = new ID("{429092D1-9938-4B19-8021-894E95F67F85}");

                /// <summary>
                /// 按鈕連結 (預留欄位)
                /// </summary>
                public static readonly ID ButtonLink = new ID("{8B0EFC2E-0525-446E-8352-42A42B2E072F}");
            }
        }
    }
}
