using System.ComponentModel;

namespace Feature.Wealth.Component.Models
{
    public enum ProductTypeEnum
    {
        [Description("全部")]
        All,
        [Description("基金")]
        Fund,
        [Description("ETF")]
        ETF,
        [Description("國外股票")]
        ForeignStock,
        [Description("結構型商品")]
        StructuredProduct,
        [Description("市場新聞")]
        MarketNews,
    }
}