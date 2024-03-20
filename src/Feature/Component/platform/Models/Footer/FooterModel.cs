using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.Footer
{
    public class FooterModel
    {
        public Item DataSource { get; set; }
        public IList<FooterSocialLinkItem> LeftLinkItems { get; set; }
        public IList<FooterLinkGroup> RightLinkItems { get; set; }   
    }

    public class FooterLinkGroup
    {
        public Item Item { get; set; }
        public IList<FooterLinkItem> Children { get; set; }
    }

    public class FooterLinkItem
    {
        public Item Item { get; set; }
    }

    public class FooterSocialLinkItem
    {
        public Item Item { get; set; }
    }
}
