using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ArticleCardList
{
    public class ArticleCardListModel
    {
        public Item DataSource { get; set; }
        public string MainTitle { get; set; }
        public IEnumerable<ArticleCardItem> CardList { get; set; }
    }

    public class ArticleCardItem
    {
        public Item Item { get; set; }
        public string PCImage { get; set; }
        public string MobileImage { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string TargetItemID { get; set; }
    }

    public struct Templates
    {
        public struct ArticleCardListDatasource
        {
            public static readonly ID ID = new ID("{39FCC027-80A1-4221-9CB5-841DCFAADE82}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{4BD31DCA-E27F-4D5B-A77F-E297E62F3F64}");
            }
        }

        public struct ArticleCardItem
        {
            public static readonly ID ID = new ID("{465EA530-52DB-4AEF-B846-776B13B03160}");

            public struct Fields
            {
                public static readonly ID PCImage = new ID("{EE1B679A-272D-4978-8C3B-65A7C522AA65}");
                public static readonly ID MobileImage = new ID("{95968D28-6B25-4D39-A924-1DBF55803829}");
                public static readonly ID Title = new ID("{AC59587F-6CD2-4688-B421-14376556A6F2}");
                public static readonly ID Content = new ID("{29619FB9-0B48-4360-B655-D73C3056DC91}");
                public static readonly ID Date = new ID("{FFA45A01-8EFB-4120-997F-C61BE1E33057}");
                public static readonly ID Link = new ID("{275357AB-09C4-4057-ADE6-AD864ED5F2D8}");
            }
        }
    }
}
