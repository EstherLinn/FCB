using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.Tab
{
    public class TabModel
    {
        public Item Datasource { get; private set; }

        public IList<Item> SubItems { get; private set; }

        public TabModel(Item item)
        {
            this.Datasource = item;
            this.SubItems = ItemUtils.GetChildren(this.Datasource)?.ToList() ?? new List<Item>();
        }
    }

    public static class DatasourceTemplates
    {
        public static readonly ID MainTitle = new ID("{94C4C08F-E9FD-43B5-8345-C67CAE820F85}");
    }

    public static class SubItemTemplates
    {
        public static readonly ID Title = new ID("{6BCC6916-B2DD-40C1-A956-D538AF611F49}");
    }
}