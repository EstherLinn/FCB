using Feature.Wealth.Component.Models.Footer;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class FooterRepository
    {
        public FooterModel GetFooter()
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

        public static IEnumerable<FooterLinkGroup> CreateGroupLinkItem(Item item)
        {
            return item.GetChildren(Templates.FooterLinkGroupID).Select(child => new FooterLinkGroup { Item = child, Children = FindLinkItems(child) });
        }

        public static IEnumerable<FooterLinkItem> FindLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterLinkItemID).Select(x => new FooterLinkItem { Item = x });
        }

        public static IEnumerable<FooterSocialLinkItem> FindSocialLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterSocialLinkItemID).Select(x => new FooterSocialLinkItem { Item = x });
        }
    }
}
