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
            public string imageUrl { get; set; }
            public string btnLink { get; set; }

            /// <summary>
            /// 插圖
            /// </summary>
            public static readonly string Image = "{1412E584-BB02-4848-92E0-3E68EC670E16}";

            /// <summary>
            /// 主標題
            /// </summary>
            public static readonly string MainTitle = "{F2D070E9-73F1-4093-BA86-65F54FDA8F91}";

            /// <summary>
            /// 次標題
            /// </summary>
            public static readonly string Subtitle = "{1A558A9E-E7E7-414B-8051-CD440E7D68A2}";

            /// <summary>
            /// 按鈕連結文字
            /// </summary>
            public static readonly string ButtonText = "{EC121B56-E4A5-471B-8288-9BB0E10D62CB}";

            /// <summary>
            /// 按鈕連結
            /// </summary>
            public static readonly string ButtonLink = "{A876C9CA-B633-4794-BA01-6665D96E1627}";
        }
    }
}
