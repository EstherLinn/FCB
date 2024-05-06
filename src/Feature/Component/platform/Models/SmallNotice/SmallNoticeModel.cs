using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.SmallNotice
{
    public class SmallNoticeModel
    {
        public Item DataSource { get; set; }
        public string MainTitle { get; set; }
        public string Content { get; set; }
    }

    public struct Templates
    {
        public struct SmallNoticeDatasource
        {
            public static readonly ID Id = new ID("{A46270FB-BFEE-4515-B791-A1C162A753F2}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{DA32971C-3BD9-46B2-89F5-BA05F9072F73}");
                public static readonly ID Content = new ID("{182086E3-BCD0-40A5-9559-855DF66D5E5B}");
            }
        }
    }
}
