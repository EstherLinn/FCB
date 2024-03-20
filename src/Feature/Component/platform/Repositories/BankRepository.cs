using Feature.Weakth.Component;
using Feature.Wealth.Component.Models.Footer;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class BankRepository
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

        public IList<FooterLinkGroup> CreateGroupLinkItem(Item item)
        {
            return item.GetChildren(Templates.FooterLinkGroup.ID).Select(child => new FooterLinkGroup { Item = child, Children = FindLinkItems(child) }).ToList();
        }
        public IList<FooterLinkItem> FindLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterLinkItem.ID).Select(x => new FooterLinkItem { Item = x }).ToList();
        }
        public IList<FooterSocialLinkItem> FindSocialLinkItems(Item item)
        {
            return item.GetChildren(Templates.FooterSocialLinkItem.ID).Select(x => new FooterSocialLinkItem { Item = x }).ToList();
        }
    }
}
