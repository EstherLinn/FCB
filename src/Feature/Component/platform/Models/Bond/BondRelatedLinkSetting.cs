using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Bond
{
    public static class BondRelatedLinkSetting
    {
        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link BondSearcPage()
        {
            Item item = ItemUtils.GetItem(Template.BondRelatedLink.Root);
            return item.GeneralLink(Template.BondRelatedLink.Fields.BondSearch);
        }

        private static Xcms.Sitecore.Foundation.Basic.FieldTypes.Link BondDetailPage()
        {
            Item item = ItemUtils.GetItem(Template.BondRelatedLink.Root);
            return item.GeneralLink(Template.BondRelatedLink.Fields.BondDetail);
        }

        public static string GetBondSearchUrl()
        {
            return BondSearcPage()?.Url;
        }

        public static string GetBondDetailUrl()
        {
            return BondDetailPage()?.Url;
        }

        public static Item GetBondSearchPageItem()
        {
            return BondSearcPage()?.TargetItem;
        }

        public static Item GetBondDetailPageItem()
        {
            return BondDetailPage()?.TargetItem;
        }

        public struct Template
        {
            public struct BondRelatedLink
            {
                public static readonly ID Root = new ID("{5AAE6EAC-F53D-4B57-8135-981CAFA0F435}");

                public struct Fields
                {
                    public static readonly ID BondSearch = new ID("{78E834D2-9E94-4FAC-A63B-536BB6EC05FB}");
                    public static readonly ID BondDetail = new ID("{634B5E93-7991-4F92-8A6A-A177AFFB6387}");
                }
            }

        }
    }
}
