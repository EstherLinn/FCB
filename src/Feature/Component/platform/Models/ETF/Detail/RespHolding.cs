using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class RespHolding
    {
        /// <summary>
        /// 產業持股狀況
        /// </summary>
        public List<EtfIndustryHolding> IndustryHoldings { get; set; }

        /// <summary>
        /// 區域持股狀況
        /// </summary>
        public List<EtfRegionHolding> RegionHoldings { get; set; }
    }
}
