using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.CommonTools
{
    public class CommonToolsModel
    {
        public Item DataSource { get; set; }
        public bool CanRenderTools { get; set; }
        public bool CanColneTools { get; set; }
        public bool IsValid { get; set; }

        public CommonToolsModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.DataSource = item;
            this.CanRenderTools = !ItemUtils.IsChecked(item, Templates.CommonTools.Fields.CloseTopTools);
            this.CanColneTools = !ItemUtils.IsChecked(item, Templates.CommonTools.Fields.CloseBottomCommonTools);

            this.IsValid = item.TemplateID == Templates.CommonTools.Id;
        }
    }

    public struct Templates
    {
        public struct CommonTools
        {
            public static readonly ID Id = new ID("{C4966863-4327-405F-9690-0ACD8C2CD512}");

            public struct Fields
            {
                public static readonly ID CloseTopTools = new ID("{BE75CD45-71E1-4735-B934-358FF1D20D4B}");
                public static readonly ID CloseBottomCommonTools = new ID("{9A9C07ED-9982-4C04-83E8-C6D31474817A}");
            }
        }
    }

}
