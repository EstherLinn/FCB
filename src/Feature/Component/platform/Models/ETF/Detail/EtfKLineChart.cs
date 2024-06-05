using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public enum EtfKLineChart
    {
        /// <summary>
        /// 日
        /// </summary>
        [Description("D")]
        day,

        /// <summary>
        /// 週
        /// </summary>
        [Description("W")]
        week,

        /// <summary>
        /// 月
        /// </summary>
        [Description("M")]
        month,

        /// <summary>
        /// 季
        /// </summary>
        [Description("Q")]
        season,
    }
}