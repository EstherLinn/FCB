using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchFilterModel
    {
        /// <summary>
        /// 計價幣別
        /// </summary>
        public IEnumerable<string> PricingCurrencyList { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public IEnumerable<string> DividendDistributionFrequencyList { get; set; }

        /// <summary>
        /// 投資標的
        /// </summary>
        public IEnumerable<string> InvestmentTargetList { get; set; }

        /// <summary>
        /// 投資地區
        /// </summary>
        public IEnumerable<string> InvestmentRegionList { get; set; }

        /// <summary>
        /// 發行公司
        /// </summary>
        public IEnumerable<string> PublicLimitedCompanyList { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public IEnumerable<string> ExchangeList { get; set; }

        /// <summary>
        /// 投資風格
        /// </summary>
        public IEnumerable<string> InvestmentStyleList { get; set; }
    }
}