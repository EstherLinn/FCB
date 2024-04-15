using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.RelatedArticles
{
    public class RelatedArticlesModel
    {
        public Item DataSource { get; set; }
        public string MainTitle { get; set; }
        public IEnumerable<RelatedArticleCardItemModel> CardList { get; set; }
    }

    public class RelatedArticleCardItemModel
    {
        public Item Item { get; set; }
        public string PCImage { get; set; }
        public string MobileImage { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
    }

    public struct Templates
    {
        public struct RelatedArticleDatasource
        {
            public static readonly ID Id = new ID("{C0857ED2-C0F8-4CA2-8F06-26B236DFD85D}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{38CBADD2-5387-4212-A059-7EB0A107B1BC}");
                public static readonly ID SettingList = new ID("{1198B7A8-1C62-4173-9BD5-9E79979CBC58}");
            }
        }
    }
}
