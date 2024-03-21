using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.Footer
{
    public class FooterModel
    {
        public Item DataSource { get; set; }
        public IEnumerable<FooterSocialLinkItem> LeftLinkItems { get; set; }
        public IEnumerable<FooterLinkGroup> RightLinkItems { get; set; }
    }

    public class FooterLinkGroup
    {
        public Item Item { get; set; }
        public IEnumerable<FooterLinkItem> Children { get; set; }
    }

    public class FooterLinkItem
    {
        public Item Item { get; set; }
    }

    public class FooterSocialLinkItem
    {
        public Item Item { get; set; }
    }

    public static class Templates
    {
        public static readonly ID FooterLinkGroupID = new("{B7193281-61E8-481C-8C4B-D316AD12E05D}");

        public static readonly ID FooterLinkItemID = new("{5D3B3B0E-03CB-4048-9620-D1AC2373CC11}");

        public static readonly ID FooterSocialLinkItemID = new("{56F8C908-BC88-4CDC-9E49-118BDFB18082}");
    }
}
