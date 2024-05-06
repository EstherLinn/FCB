using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Link
{
    public class LinkModel
    {
        public Item DataSource { get; set; }
        public IList<Link> Items { get; set; }

        public class Link
        {
            public Link(Item item)
            {
                Item = item;

                this.icon = "o-prefixLink o-prefixLink--insidebox t-bold ";
                if (this.LinkUrl.StartsWith("http") && !this.LinkUrl.EndsWith(".pdf"))
                {
                    this.icon += "o-prefixLink--external";
                }
                else if (this.LinkUrl.EndsWith(".pdf"))
                {
                    this.icon += "o-prefixLink--pdf";
                }
                else
                {
                    this.icon += "o-prefixLink--link";
                }
            }
            public Item Item { get; set; }
            public string Title => ItemUtils.GetFieldValue(Item, Templates.Link.Fields.Title);
            public string LinkUrl => ItemUtils.GeneralLink(Item, Templates.Link.Fields.Link)?.Url;
            public string icon { get; set; }
        }
    }

    public struct Templates
    {
        public struct LinkTitle
        {
            public static readonly ID Id = new ID("{9440F2CF-A724-4DA9-9F1B-F509CB3EF26C}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{126A1C22-0E8E-41CC-956C-FE411C49F548}");
            }
        }

        public struct Link
        {
            public static readonly ID Id = new ID("{B2B18294-1AEA-4402-9C81-B6DF989BF47E}");

            public struct Fields
            {
                /// <summary>
                /// 連結標題
                /// </summary>
                public static readonly ID Title = new ID("{6B5B38B5-56FA-44FF-A093-16CCBE9FE01F}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link = new ID("{69896A20-1850-4D83-ABE0-8B233301BF26}");
            }
        }
    }
}

