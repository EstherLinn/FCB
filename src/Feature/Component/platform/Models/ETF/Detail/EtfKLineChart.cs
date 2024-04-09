using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public enum EtfKLineChart
    {
        [Description("D")]
        day,

        [Description("W")]
        week,

        [Description("M")]
        month,

        [Description("Q")]
        season,
    }
}