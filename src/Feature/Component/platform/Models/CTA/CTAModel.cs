using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.CTA
{
    public class CTAModel
    {
        public Item Item { get; set; }
        public string ImageUrl { get; set; }
        public string ButtonLink { get; set; }
        public bool ShowIcon { get; set; }
    }

    public struct Template
    {
        /// <summary>
        /// 插圖
        /// </summary>
        public static readonly string Image = "{7D47B613-D632-432B-A7AB-DBD4FDDEB2B5}";

        /// <summary>
        /// 主標題
        /// </summary>
        public static readonly string MainTitle = "{AEC62FCD-5B88-4398-825F-D23D1313C9E3}";

        /// <summary>
        /// 次標題
        /// </summary>
        public static readonly string Subtitle = "{DFCBF0EF-D034-404F-A301-BBB0974F00A3}";

        /// <summary>
        /// 顯示ICON
        /// </summary>
        public static readonly string ShowIcon = "{6ACFB5CB-5DB8-44A0-BF69-E911DA99572B}";

        /// <summary>
        /// 按鈕連結文字
        /// </summary>
        public static readonly string ButtonLinkText = "{BF3930F3-67FC-4BA6-9DC2-26A855B0D9F1}";

        /// <summary>
        /// 按鈕連結
        /// </summary>
        public static readonly string ButtonLink = "{BAFBF0D5-F544-4AEE-9FD8-2615F3B79E8E}";

    }
}
