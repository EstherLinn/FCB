using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.USStock
{
    public static class USStockRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link USStockSearcPage()
        {
            Item item = ItemUtils.GetItem(Template.USStockRelatedLink.Root);
            return item.GeneralLink(Template.USStockRelatedLink.Fields.USStockSearch);
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link USStockDetailPage()
        {
            Item item = ItemUtils.GetItem(Template.USStockRelatedLink.Root);
            return item.GeneralLink(Template.USStockRelatedLink.Fields.USStockDetail);
        }

        public static string GetUSStockSearchUrl()
        {
            return USStockSearcPage()?.Url;
        }

        public static string GetUSStockDetailUrl()
        {
            return USStockDetailPage()?.Url;
        }

        public static Item GetUSStockSearchPageItem()
        {
            return USStockSearcPage()?.TargetItem;
        }

        public static Item GetUSStockDetailsPageItem()
        {
            return USStockDetailPage()?.TargetItem;
        }

        public struct Template
        {
            public struct USStockRelatedLink
            {
                public static readonly ID Root = new ID("{B6913A88-4FC4-48C2-8BAF-1350C93E8A5A}");

                public struct Fields
                {
                    public static readonly ID USStockSearch = new ID("{0478EE30-BAEA-44C2-89F8-91C746BB0C1D}");
                    public static readonly ID USStockDetail = new ID("{93D8E15F-8DD2-49D5-9858-990673212E5A}");
                }
            }

        }
    }
}
