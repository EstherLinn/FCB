using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.IndexBanner
{
    public class IndexBannerModel
    {
        public Item DataSource { get; set; }

        public IList<Banner> Items { get; set; }

        public class Banner
        {
            public Banner(Item item)
            {
                Item = item;
            }

            public Item Item { get; set; }
            public bool IsDark { get; set; }
            public string ImageUrlPC { get; set; }
            public string ImageUrlMB { get; set; }
            public string ButtonLink { get; set; }
            public string ButtonText { get; set; }
        }
    }


    public struct Template
    {
        public struct IndexBanner
        {
            public static readonly ID Id = new ID("{42237DF0-FA9A-4CA5-A848-0D41E244EE3F}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID Title = new ID("{F2D070E9-73F1-4093-BA86-65F54FDA8F91}");

                /// <summary>
                /// 次標題
                /// </summary>
                public static readonly ID Subtitle = new ID("{1A558A9E-E7E7-414B-8051-CD440E7D68A2}");

                /// <summary>
                /// 是否為暗黑模式
                /// </summary>
                public static readonly ID DarkMode = new ID("{FDC136D5-0AD4-47E0-B483-24637AD2F982}");

                /// <summary>
                /// 圖片-電腦版
                /// </summary>
                public static readonly ID ImagePC = new ID("{1412E584-BB02-4848-92E0-3E68EC670E16}");

                /// <summary>
                /// 圖片-手機板
                /// </summary>
                public static readonly ID ImageMB = new ID("{D8422A07-DDCC-4589-985C-7595C110C478}");

                /// <summary>
                /// 按鈕連結文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{EC121B56-E4A5-471B-8288-9BB0E10D62CB}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{A876C9CA-B633-4794-BA01-6665D96E1627}");
            }
        }
    }
}