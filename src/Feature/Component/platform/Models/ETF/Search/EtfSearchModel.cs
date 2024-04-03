﻿using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchModel
    {
        public EtfSearchResultModel SearchResultModel { get; set; }

        public EtfSearchFilterModel FilterModel { get; set; }
    }

    public enum RegionType
    {
        [Description("None")]
        None,

        /// <summary>
        /// 國內
        /// </summary>
        [Description("國內")]
        Domestic,

        /// <summary>
        /// 海外
        /// </summary>
        [Description("境外")]
        Overseas
    }

    public enum CurrencyCapsule
    {
        /// <summary>
        /// 原幣
        /// </summary>
        [Description("原幣")]
        Original,

        /// <summary>
        /// 台幣
        /// </summary>
        [Description("台幣")]
        TWD
    }
}