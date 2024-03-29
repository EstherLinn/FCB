using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.KeyPoint
{
    public class KeyPointModel
    {
        public Item Item { get; set; }
    }

    public struct Templates
    {
        public struct KeyPoint
        {
            public static readonly ID Id = new ID("{DA693B2B-1E8B-42CC-9364-7E7B13734483}");

            public struct Fields
            {
                /// <summary>
                /// 內容
                /// </summary>
                public static readonly ID Content = new ID("{A515C19F-A9CC-40B9-A367-D8854D2521A3}");
            }
        }
    }
}
