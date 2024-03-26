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
        public static readonly string Image = "{D17D168A-1950-426A-B8BC-21ACC3AC3B9B}";

        /// <summary>
        /// 標題
        /// </summary>
        public static readonly string Title = "{27B232AB-8B31-4E68-8E3D-D0F7B55D8ADB}";

    }
}

