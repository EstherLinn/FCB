using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.PageTitle
{
    public class PageTitleModel
    {
        public Item Item { get; set; }
    }

    public struct Templates
    {
        public struct PageTitle
        {
            public static readonly ID Id = new ID("{F0BE4BEC-F817-40BF-A23B-EE61E64B1857}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{0B6458BA-86FB-4591-AEDA-445182B1A75F}");
            }
        }
    }
}
