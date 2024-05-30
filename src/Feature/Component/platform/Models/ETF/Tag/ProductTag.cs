using System.Collections.Generic;
using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Tag
{
    public class ProductTag
    {
        /// <summary>
        /// 標籤 Key
        /// </summary>
        public string TagKey { get; set; }

        /// <summary>
        /// 產品代碼
        /// </summary>
        public List<string> ProductCodes { get; set; }

        /// <summary>
        /// 標籤類型
        /// </summary>
        public TagType TagType { get; set; }
    }

    public enum TagType
    {
        [Description("None")]
        None,

        /// <summary>
        /// 優惠標籤
        /// </summary>
        [Description("優惠標籤")]
        Discount,

        /// <summary>
        /// 分類標籤
        /// </summary>
        [Description("分類標籤")]
        Category,

        /// <summary>
        /// 熱門關鍵字
        /// </summary>
        [Description("熱門關鍵字")]
        Keywords
    }
}
