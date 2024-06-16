using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.SearchBar
{
    public class SearchBarModel
    {
        public Item DataSource { get; set; }
        public IEnumerable<Item> HotKeyWords { get; set; }
        public IEnumerable<ID> TemplatesID => new[] { Templates.SourceItem.Id, Templates.HotKeyWordSourceItem.Id };
        public bool IsValid { get; set; }

        public SearchBarModel(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.DataSource = item;

            this.HotKeyWords = item.GetMultiListValueItems(Templates.SearchBar.Fields.HotKeyWords).Where(i => this.TemplatesID.Contains(Templates.SourceItem.Id)
                && i.FieldHasValue(Templates.SourceItem.Fields.Datasource) && i.FieldHasValue(Templates.SourceItem.Fields.FieldTemplate));

            this.IsValid = item.TemplateID == Templates.SearchBar.Id;
        }
    }

    public struct Templates
    {
        public struct SearchBar
        {
            public static readonly ID Id = new ID("{924C9FBC-8059-422C-952F-59F4FA6E6F8D}");

            public struct Fields
            {
                public static readonly ID HotKeyWords = new ID("{3923A700-0566-4DD1-B183-5B1359725E84}");
                public static readonly ID Title = new ID("{068F6EBC-DF5F-47F8-A5D3-1A9413941181}");
            }
        }

        public struct SourceItem
        {
            public static readonly ID Id = new ID("{3A3CB02F-7818-4FA7-B44D-87CD84F4054F}");

            public struct Fields
            {
                public static readonly ID Datasource = new ID("{FDDC231A-A001-42E3-A2D0-38F9D0A7C905}");
                public static readonly ID FieldTemplate = new ID("{CD958BCD-C463-4588-A6E9-8BB6BC36BDA0}");
                public static readonly ID LinkItem = new ID("{FF197B22-5973-4584-82A5-3AD0B4BB6FD1}");
                public static readonly ID LinkText = new ID("{413A919B-B3F9-4A92-A824-E2D1A9B5161E}");
            }
        }

        public struct HotKeyWordSourceItem
        {
            public static readonly ID Id = new ID("{A5ABF17C-2888-43B1-B2E2-325E6A680178}");

            public struct Fields
            {
                public static readonly ID IsSiteProductSearch = new ID("{829DD7CE-8A1D-471C-AAC9-83B394B56DE8}");
                public static readonly ID SearchLink = new ID("{56C22917-83B9-4291-8F9E-EBE208FA58D5}");
            }
        }

    }
}