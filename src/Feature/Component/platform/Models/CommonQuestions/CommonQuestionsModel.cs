using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.CommonQuestions
{
    public class CommonQuestionsModel
    {
        public Item DataSource { get; set; }
        public IList<SubItem> SubItems { get; set; }
        public bool CanRender => this.SubItems?.Any() ?? false;

        public CommonQuestionsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.DataSource = item;
            this.SubItems = ItemUtils.GetChildren(item)?.Select(x => new SubItem(x)).ToList();
        }

        public class SubItem
        {
            public Item Item { get; set; }
            public SubItem(Item item)
            {
                this.Item = item;
            }
        }
    }
    public struct Templates
    {
        public struct CommonQuestionsDataSource
        {
            public static readonly ID Id = new ID("{22DB42A2-BF73-4A08-8167-D134DBE62C09}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{17E6184B-389A-4031-9120-449347396ADC}");
            }
        }
        public struct CommonQuestionsItem
        {
            public static readonly ID Id = new ID("{27F0BD05-222D-4ED3-AB75-BF0947B879D9}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{D51B80C2-778D-4D61-817A-3589BB26A032}");
                public static readonly ID Content = new ID("{81E9C6ED-ED07-464E-8786-009360024719}");
            }
        }
    }

}
