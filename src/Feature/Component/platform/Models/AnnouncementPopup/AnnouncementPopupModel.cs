using Sitecore.Data;
using Sitecore.Data.Items;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.AnnouncementPopup
{
    public class AnnouncementPopupModel
    {
        public Item DataSource { get; set; }
        public bool IsShow { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public AnnouncementPopupModel()
        {
        }
        public AnnouncementPopupModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.DataSource = item;
            this.IsShow = ItemUtils.IsChecked(item, Templates.AnnouncementPopupDatasource.Fields.IsShow);
            this.Title = ItemUtils.GetFieldValue(item, Templates.AnnouncementPopupDatasource.Fields.Title);
            this.Content = ItemUtils.GetFieldValue(item, Templates.AnnouncementPopupDatasource.Fields.Content);
        }
    }
    public struct Templates
    {
        public struct AnnouncementPopupDatasource
        {
            public static readonly ID Id = new ID("{1F62ECCF-11B0-42E4-943E-568038CD8A12}");

            public struct Fields
            {
                public static readonly ID IsShow = new ID("{6443BC65-B416-4CD6-A7FD-33A673805AF6}");
                public static readonly ID Title = new ID("{1AD9CCE5-29D3-4226-8A1B-B4FA337735CA}");
                public static readonly ID Content = new ID("{F8E29051-9EC8-4AAB-A04E-C543DFCCD8AE}");
            }
        }
    }
}
