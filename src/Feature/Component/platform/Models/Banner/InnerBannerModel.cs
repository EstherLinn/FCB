using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.Banner
{
    public class InnerBannerModel
    {
        public Item Item { get; set; }
        public string ImageUrl { get; set; }
    }

    public struct Template
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public static readonly string Image = "{8C374ADE-0694-4757-B8ED-D3984F2C947E}";

        /// <summary>
        /// 標題
        /// </summary>
        public static readonly string Title = "{397AF03E-2E2C-4AD1-A2BD-327C1CF047C4}";

    }
}

