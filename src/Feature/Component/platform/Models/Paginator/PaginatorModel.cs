using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.Paginator
{
    public class PaginatorModel
    {
        public PaginatorItem PreviousPage { get; set; }

        public PaginatorItem NextPage { get; set; }
    }

    public class PaginatorItem
    {
        public Item Item { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }
    }

    public class SortingModel
    {
        /// <summary>
        /// 排序選項
        /// </summary>
        public SortingOptions Option { get; set; }

        /// <summary>
        /// 排序欄位
        /// </summary>
        public string Field { get; set; }
    }

    public enum SortingOptions
    {
        None,
        Ascending,
        Descending
    }

    public struct Templates
    {
        public struct PageNavigationTitle
        {
            public static readonly ID Id = new ID("{B83F10F1-6F84-49E2-A807-856B6FF6C10A}");

            public struct Fields
            {
                public static readonly ID NavigationTitle = new ID("{A8FAD4B5-F582-4B63-9031-8FA8EA8B6D06}");
            }
        }

        public struct PaginatorFields
        {
            public static readonly ID Id = new ID("{BC3C4C0A-CEED-4E75-9AEB-EA708CDA0B78}");

            public struct Fields
            {
                public static readonly ID SortingOption = new ID("{D8BF9BF9-493A-401D-8812-B0C253EB6DF0}");
                public static readonly ID SortingField = new ID("{B4E81FC9-3487-4547-BA21-55DA06A6E15F}");
            }
        }
    }
}