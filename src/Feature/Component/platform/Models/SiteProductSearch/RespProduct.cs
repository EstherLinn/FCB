using Feature.Wealth.Component.Models.SiteProductSearch.Product;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SiteProductSearch
{
    public class RespProduct
    {
        /// <summary>
        /// ETF
        /// </summary>
        public List<EtfProductResult> ETFProducts { get; set; }

        /// <summary>
        /// 基金
        /// </summary>
        public List<FundProductResult> FundProducts { get; set; }

        /// <summary>
        /// 結構型商品
        /// </summary>
        public List<StructuredProductResult> StructuredProducts { get; set; }

        /// <summary>
        /// 國外債券
        /// </summary>
        public List<ForeignBondsResult> ForeignBonds { get; set; }

        /// <summary>
        /// 國外股票
        /// </summary>
        public List<ForeignStockResult> ForeignStocks { get; set; }

        /// <summary>
        /// 國外債券
        /// </summary>
        public List<ForeignBondResult> ForeignBonds { get; set; }
    }
}