using System.ComponentModel;

namespace Feature.Wealth.Component.Models.StructuredProduct
{
    public enum StructuredProductTagEnum
    {
        [Description("優惠標籤")]
        DiscountTag,
        [Description("分類標籤")]
        SortTag,
        [Description("關鍵字標籤")]
        KeywordTag
    }
}