using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
namespace Feature.Wealth.Component.Models.FeaturedCards
{
    public class FeaturedCardsModel
    {
        public Item DataSource { get; set; }
        public IList<FeaturedCards> Items { get; set; }
        public FeaturedCardsModel(Item datasource, IList<FeaturedCards> items)
        {
            if (datasource?.TemplateID != Templates.FeaturedCardsTitle.Id)
            {
                return;
            }
            this.DataSource = datasource;
            this.Items = items;
        }
    }

    public class FeaturedCards
    {
        public FeaturedCards(Item item)
        {
            Item = item;
        }
        public Item Item { get; set; }
        public string Content { get; set; }
    }

    public struct Templates
    {
        public struct FeaturedCardsTitle
        {
            public static readonly ID Id = new ID("{CCB44DD0-94E0-4444-B5E7-3F3F2D3DB9D9}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{E6E7BD01-14A8-4F72-B395-02FE13CB258E}");

                /// <summary>
                /// 敘述
                /// </summary>
                public static readonly ID Description = new ID("{AA6DC1AB-FD91-4C09-A8BF-AF4E9D690C5C}");
            }
        }

        public struct FeaturedCards
        {
            public static readonly ID Id = new ID("{C84AC6D1-FDFE-4B04-B231-95F58851D973}");

            public struct Fields
            {
                /// <summary>
                /// 編號
                /// </summary>
                public static readonly ID Label = new ID("{61C873F0-6C9C-440C-A30D-2EDB82F293BC}");

                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{9885D4CA-DD8E-4E83-820D-ED5764980AF6}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{7D22F9E1-9B88-45AD-B048-7FB087AE8990}");
            }
        }
    }
}
