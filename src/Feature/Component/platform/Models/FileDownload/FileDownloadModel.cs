using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FileDownload
{
    public class FileDownloadModel
    {
        public Item DataSource { get; set; }
        public IList<File> Items { get; set; }
        public class File
        {
            public File(Item item)
            {
                Item = item;
            }
            public Item Item { get; set; }
            public string LinkUrl { get; set; }
            public string LinkTarget { get; set; }
        }
    }

    public struct Templates
    {
        public struct FileDescription
        {
            public static readonly ID Id = new ID("{15001D00-6FEE-40B5-A960-AABC781624FF}");

            public struct Fields
            {
                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{0E5244E9-3C1F-4817-8E94-318527728E76}");
            }
        }

        public struct FileDownload
        {
            public static readonly ID Id = new ID("{5CAE8294-B5EE-4771-92FD-C68940F6AA62}");

            public struct Fields
            {
                /// <summary>
                /// 連結文字
                /// </summary>
                public static readonly ID LinkText = new ID("{31C3E314-4CF8-41B0-8D91-F1B43A6B1602}");

                /// <summary>
                /// 連結
                /// </summary>
                public static readonly ID Link = new ID("{BEAAA08F-1EC4-47D6-A3B6-6E623781EC5A}");
            }
        }
    }
}
