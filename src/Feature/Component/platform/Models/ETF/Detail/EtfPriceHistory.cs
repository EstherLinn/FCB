namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfPriceHistory
    {
        /// <summary>
        /// 淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public decimal? MarketPrice { get; set; }
    }
}
