using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.MultipleFileDownloadList
{
    public class MultipleFileDownloadListModel
    {
        public Item DataSource { get; set; }
        public IEnumerable<DownloadListItemModel> DownloadListItems { get; set; }
    }

    public class DownloadListItemModel
    {
        public Item item { get; set; }
        public string ItemTitle { get; set; }
        public IEnumerable<DownloadListSubItemModel> DownloadListSubitems { get; set; }
    }

    public class DownloadListSubItemModel
    {
        public Item item { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
    }

    public static class Templates
    {
        public struct MultipleFileDownloadListDatasource
        {
            public static readonly ID Id = new ID("{5B184CFE-CA83-45FE-9938-30DA2B297958}");
        }

        public struct MultipleFileDownloadListItem
        {
            public static readonly ID Id = new ID("{0934266B-DFE2-4F5E-A2A4-FC92E872C5F7}");

            public struct Fields
            {
                internal static readonly ID ItemTitle = new ID("{3893A333-FEEF-441F-95BB-EF7BA102BBE9}");
            }
        }

        public struct MultipleFileDownloadListSubItem
        {
            public static readonly ID Id = new ID("{D89325A3-6D40-4C8B-8AEA-12B60EE5084F}");

            public struct Fields
            {
                internal static readonly ID Title = new ID("{93DACEE1-8900-4F57-BF7B-95292EE029C9}");
                internal static readonly ID Date = new ID("{3F223052-1FF6-40A9-8AF7-889575B17283}");
                internal static readonly ID Link = new ID("{CD8C6328-CF0C-4814-A888-77A495E202B3}");
            }
        }
    }
}
