using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Features
{
    public class FeaturesModel
    {
        public Item DataSource { get; set; }
        public IList<Features> Items { get; set; }
        public class Features
        {
            public Features(Item item)
            {
                Item = item;
            }
            public Item Item { get; set; }
            public string ImageUrl => ItemUtils.ImageUrl(Item, Templates.Features.Fields.Image);
            public string ButtonText => ItemUtils.GetFieldValue(Item, Templates.Features.Fields.ButtonText);
            public string ButtonLink => ItemUtils.GeneralLink(Item, Templates.Features.Fields.ButtonLink)?.Url;
            public string LinkText1 => ItemUtils.GetFieldValue(Item, Templates.Features.Fields.LinkText1);
            public string LinkUrl1 => ItemUtils.GeneralLink(Item, Templates.Features.Fields.Link1)?.Url;
            public string LinkText2 => ItemUtils.GetFieldValue(Item, Templates.Features.Fields.LinkText2);
            public string LinkUrl2 => ItemUtils.GeneralLink(Item, Templates.Features.Fields.Link2)?.Url;
        }
    }

    public struct Templates
    {
        public struct FeaturesTitle
        {
            public static readonly ID Id = new ID("{56D391D9-F0F3-498A-A3B7-E87A9D17454F}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{3B15A8EA-EBD6-426E-A04B-C191C5290A11}");
            }
        }

        public struct Features
        {
            public static readonly ID Id = new ID("{FD0B871A-5D5D-4E12-9822-6646239C0777}");

            public struct Fields
            {
                /// <summary>
                /// 圖片
                /// </summary>
                public static readonly ID Image = new ID("{53A9C861-DB86-4A2B-BDBA-CDACD8D6D8C5}");

                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{2582A360-452F-41F3-BE59-356A1E506AB1}");

                /// <summary>
                /// 按鈕文字
                /// </summary>
                public static readonly ID ButtonText = new ID("{CA4D1690-D904-4B89-A78D-56BEC2C58E04}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{0916EFEA-77C4-4788-BE9E-2283AF182D87}");

                /// <summary>
                /// 連結文字
                /// </summary>
                public static readonly ID LinkText1 = new ID("{272B779C-0861-49E8-B6C3-DDE44B951D43}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link1 = new ID("{36A4E9D5-847E-430D-9013-0787E27390CB}");

                /// <summary>
                /// 連結文字
                /// </summary>
                public static readonly ID LinkText2 = new ID("{6E59A264-5D50-4BCA-9320-568C8F20497A}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link2 = new ID("{13D2809F-FA62-4B4D-B0C4-AC61968DEEE7}");
            }
        }
    }
}
