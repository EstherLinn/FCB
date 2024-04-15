using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.ArticleContent
{
    public class ArticleContentModel
    {
        public Item DataSource { get; set; }
        public string MainTitle { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string Content1 { get; set; }
        public string Introduction { get; set; }
        public string Content2 { get; set; }
    }

    public struct Templates
    {

        public struct ArticleContentDatasource
        {
            public static readonly ID ID = new ID("{AF26C91D-04EE-4C0E-B6E8-078F0B4AEC70}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{4195A69D-4C10-4FDC-91DE-99BED559EFA0}");
                public static readonly ID Date = new ID("{56F4CB8D-471F-4753-9C38-A6CDDC210E13}");
                public static readonly ID Image = new ID("{3E2D392C-A38D-448D-8051-0D6547415921}");
                public static readonly ID Content1 = new ID("{0FBE2D0A-42B0-43A2-B10E-08BF8C66C600}");
                public static readonly ID Introduction = new ID("{21085A7D-D62D-4011-B4B0-E64941527DD6}");
                public static readonly ID Content2 = new ID("{70D5EC1A-5FE6-4290-B7C8-29E1E20761B4}");
            }
        }
    }
}
