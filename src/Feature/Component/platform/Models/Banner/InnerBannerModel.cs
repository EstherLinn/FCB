using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.Banner
{
    public class InnerBannerModel
    {
        public Item Item { get; set; }
        public string ImageUrl { get; set; }
    }

    public struct Templates
    {
        public struct InnerBanner
        {
            public static readonly ID Id = new ID("{28345B4F-F889-42D5-9FDF-41306097414B}");

            public struct Fields
            {
                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{D17D168A-1950-426A-B8BC-21ACC3AC3B9B}");

                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{27B232AB-8B31-4E68-8E3D-D0F7B55D8ADB}");
            }
        }
    }
}
