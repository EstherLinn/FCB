using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Footer
{
    public class FooterModel
    {
        public Item DataSource { get; set; }
        public IList<FooterSocialLinkItem> LeftLinkItems { get; set; }
        public IList<FooterLinkGroup> RightLinkItems { get; set; }

        public static FooterModel GetFooter()
        {
            string dataSourceId = RenderingContext.CurrentOrNull?.Rendering?.DataSource;
            if (string.IsNullOrEmpty(dataSourceId))
            {
                return null;
            }

            var dataSource = RenderingContext.CurrentOrNull?.Rendering?.Item;
            if (dataSource == null)
            {
                return null;
            }

            var footer = new FooterModel
            {
                DataSource = dataSource
            };

            var leftLink = dataSource.TargetItem("Footer Left Source");
            if (leftLink != null)
            {
                footer.LeftLinkItems = FindSocialLinkItems(leftLink);
            }

            var rightLink = dataSource.TargetItem("Footer Right Source");
            if (rightLink != null)
            {
                footer.RightLinkItems = CreateGroupLinkItem(rightLink);
            }

            return footer;
        }

        public static IList<FooterLinkGroup> CreateGroupLinkItem(Item item)
        {
            return item.GetChildren(Templates.FooterLinkGroupID).Select(child => new FooterLinkGroup { Item = child, Children = FindLinkItems(child) }).ToList();
        }

        public static IList<FooterLinkItem> FindLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterLinkItemID).Select(x => new FooterLinkItem { Item = x }).ToList();
        }

        public static IList<FooterSocialLinkItem> FindSocialLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterSocialLinkItemID).Select(x => new FooterSocialLinkItem { Item = x }).ToList();
        }
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

    public static class Templates
    {
        public static readonly ID FooterLinkGroupID = new("{B7193281-61E8-481C-8C4B-D316AD12E05D}");

        public static readonly ID FooterLinkItemID = new("{5D3B3B0E-03CB-4048-9620-D1AC2373CC11}");

        public static readonly ID FooterSocialLinkItemID = new("{56F8C908-BC88-4CDC-9E49-118BDFB18082}");
    }
}
