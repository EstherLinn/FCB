using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.CTA
{
    public class CTAModel
    {
        public Item Item { get; set; }
        public string ImagePcUrl { get; set; }
        public string ImageMbUrl { get; set; }
        public string ButtonLink { get; set; }
        public string ButtonText { get; set; }
        public bool ShowIcon { get; set; }
    }

    public struct Template
    {
        public struct CTA
        {
            public static readonly ID Id = new ID("{ECD826FE-7C27-4EBC-A3DF-9D2CF8A5F68D}");

            public struct Fields
            {
                /// <summary>
                /// 插圖-電腦版
                /// </summary>
                public static readonly ID ImagePc = new ID("{7D47B613-D632-432B-A7AB-DBD4FDDEB2B5}");

                /// <summary>
                /// 插圖-手機板
                /// </summary>
                public static readonly ID ImageMb = new ID("{EB97F51D-0B7D-44D5-A816-090493B98939}");

                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{AEC62FCD-5B88-4398-825F-D23D1313C9E3}");

                /// <summary>
                /// 次標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{DFCBF0EF-D034-404F-A301-BBB0974F00A3}");

                /// <summary>
                /// 顯示ICON
                /// </summary>
                public static readonly ID ShowIcon = new ID("{6ACFB5CB-5DB8-44A0-BF69-E911DA99572B}");

                /// <summary>
                /// 按鈕連結文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{BF3930F3-67FC-4BA6-9DC2-26A855B0D9F1}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{BAFBF0D5-F544-4AEE-9FD8-2615F3B79E8E}");
            }
        }
    }
}
