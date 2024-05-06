using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.LongCards
{
    public class LongCardsModel
    {
        public Item Item { get; set; }
        public string ImageUrl { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
    }

    public struct Templates
    {
        public struct LongCards
        {
            public static readonly ID Id = new ID("{A8DD590A-7CC9-47DB-841C-EDFD310338E0}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{ED40AB60-8AEA-4F3F-82A0-D9C3139DDE3C}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Description = new ID("{B67F64A0-D4BB-40DE-B07A-BB6CF97BE11A}");

                /// <summary>
                /// 按鈕文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{AB44AB26-0A58-43D6-9B3A-D30B260F5915}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{301B2C16-B9F1-4DFC-9959-1540EE7FD051}");

                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{550CBA5B-1228-437A-A54D-952F7740B1C5}");
            }
        }
    }
}
