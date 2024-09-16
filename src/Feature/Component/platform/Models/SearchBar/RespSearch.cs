using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.SearchBar
{
    public class RespSearch
    {
        /// <summary>
        /// ETF
        /// </summary>
        public IEnumerable<Dictionary<string, object>> ETFProducts { get; set; }

        /// <summary>
        /// 基金
        /// </summary>
        public IEnumerable<Dictionary<string, object>> FundProducts { get; set; }

        /// <summary>
        /// 結構型商品
        /// </summary>
        public IEnumerable<Dictionary<string, string>> StructuredProducts { get; set; }

        /// <summary>
        /// 國外債券
        /// </summary>
        public IEnumerable<Dictionary<string, string>> ForeignBonds { get; set; }

        /// <summary>
        /// 國外股票
        /// </summary>
        public IEnumerable<Dictionary<string, object>> ForeignStocks { get; set; }
    }
}
