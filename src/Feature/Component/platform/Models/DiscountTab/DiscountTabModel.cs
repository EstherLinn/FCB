using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.DiscountTab
{
    public class DiscountTabModel
    {
        public Item DataSource { get; set; }

        public IEnumerable<DiscountTabItem> TabList { get; set; }
        public DiscountTabModel(Item item)
        {
            if (item == null || item.TemplateID != Templates.DiscountTabDatasource.ID)
            {
                return;
            }

            this.DataSource = item;
            this.TabList = item.GetChildren(Templates.DiscountTabItem.ID)
              .Where(children => item.GetChildren(Templates.DiscountTabItem.ID).Any())
              .Select(x => new DiscountTabItem { Item = x });
        }
    }

    public class DiscountTabItem
    {
        public Item Item { get; set; }
    }

    public class Templates
    {
        public struct DiscountTabDatasource
        {
            public static readonly ID ID = new ID("{4429B401-642B-4C87-86AA-62BAC14DC98C}");
        }

        public struct DiscountTabItem
        {
            public static readonly ID ID = new ID("{F4E1680A-AAA7-4DF7-A7EF-197EA911B882}");
        }
    }
}
