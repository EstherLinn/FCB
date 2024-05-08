using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.VideoCarousel
{
    public class VideoCarouselModel
    {
        public Item DataSource { get; set; }
        public bool CheckedShowIcon { get; set; }
        public string ImageUrl { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public IList<VideoCarousel> Items { get; set; }

        public class VideoCarousel
        {
            public VideoCarousel(Item item)
            {
                Item = item;
            }
            public Item Item { get; set; }
            public string ImageUrl { get; set; }
            public bool CheckedShowIcon { get; set; }
            public bool CheckedOpenVideoLinkinLightBox { get; set; }
            public string LinkUrl => ItemUtils.GeneralLink(Item, Templates.VideoCarouselVideos.Fields.Link)?.Url;
        }
    }

    public struct Templates
    {
        public struct VideoCarouselIndex
        {
            public static readonly ID Id = new ID("{E0846E96-532E-479B-9764-B2E904C18C78}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{909211D7-7E30-4791-83B9-7B2D7A23503E}");

                /// <summary>
                /// 是否顯示播放圖示
                /// </summary>
                public static readonly ID ShowIcon = new ID("{8F1B877F-FA55-40D0-BD27-6A06267C9BDC}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{EF923255-FC94-43BE-B225-8C7F75A2F27B}");

                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{ADCC523D-51DD-46AF-84EF-68C8AEA3C79A}");

                /// <summary>
                /// 連結文字
                /// </summary>
                public static readonly ID LinkText = new ID("{9649AA47-0E5F-4336-9292-71AB66E0A9E8}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link = new ID("{D924EFD0-25A0-4E7A-9C3A-372DEDE3BF2A}");
            }
        }

        public struct VideoCarouselVideos
        {
            public static readonly ID Id = new ID("{A255EA89-103D-4AD9-92DC-4036999F56F5}");

            public struct Fields
            {
                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{DAFE20A0-FCC7-4BD6-80B1-19263BBBEC43}");

                /// <summary>
                /// 以燈箱開啟影片連結
                /// </summary>
                public static readonly ID OpenVideoLinkinLightBox = new ID("{70B7A52D-7546-405D-8B0E-F368A8B83C71}");

                /// <summary>
                /// 是否顯示播放圖示
                /// </summary>
                public static readonly ID ShowIcon = new ID("{BD09DF2B-C946-4B6B-A47D-81468A0AA2BC}");

                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{1E41F781-6166-4441-996C-E9F7F74D3123}");

                /// <summary>
                /// 連結文字
                /// </summary>
                public static readonly ID LinkText = new ID("{81518A07-57EE-4E84-87A5-9326CBFA1720}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link = new ID("{7BC13B8F-726B-46C5-9EFA-67BC78D4474E}");
            }
        }
    }
}
