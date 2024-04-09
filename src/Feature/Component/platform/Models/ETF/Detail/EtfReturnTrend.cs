using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public enum EtfReturnTrend
    {
        [Description("市價")]
        MarketPrice,

        [Description("淨值")]
        NetAssetValue,
    }
}