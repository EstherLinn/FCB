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
    }
}