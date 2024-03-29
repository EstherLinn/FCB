using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.Notice
{
    public class NoticeModel
    {
        public Item Item { get; set; }
    }

    public struct Templates
    {
        public struct Notice
        {
            public static readonly ID Id = new ID("{92C6CB62-2192-434B-8713-C4C199AAEB86}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{C4A879F2-CFCF-4095-A0EB-57AF981E71FB}");

                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{59945339-D1E2-447B-ADAD-5D3E1BBB99C5}");
            }
        }
    }
}
