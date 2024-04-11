using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.HorizontalGraphics
{
    public class HorizontalGraphicsModel
    {
        public Item DataSource { get; set; }
        public Item Direction { get; set; }
        public IList<HorizontalGraphics> Items { get; set; }

        public class HorizontalGraphics
        {
            public HorizontalGraphics(Item item)
            {
                Item = item;
            }
            public Item Item { get; set; }
            public string ImageUrl { get; set; }
        }
    }

    public struct Templates
    {
        public struct HorizontalGraphicsTitle
        {
            public static readonly ID Id = new ID("{9BC33BE6-586F-49B2-A53F-47FE9277CDB9}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{4EA0DEA8-2A7E-4A27-8C4C-124C283B234C}");

                /// <summary>
                /// 描述
                /// </summary>
                public static readonly ID Description = new ID("{2DE8D708-D763-4B37-A8FE-8A2BE0491605}");

                /// <summary>
                /// 圖片方向
                /// </summary>
                public static readonly ID FadeDirection = new ID("{3BAAB9CB-12E6-413D-9374-3591D204865E}");
            }
        }

        public struct HorizontalGraphics
        {
            public static readonly ID Id = new ID("{759CA276-16B9-4325-BD65-EE852B331231}");

            public struct Fields
            {
                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{20008C0F-AA09-478D-9B23-447BC947971D}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{A8B64596-06EB-4EBF-945B-C7759023C509}");

                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{5C02B39E-764B-448D-9803-4091EBC764BE}");
            }
        }
    }
}
