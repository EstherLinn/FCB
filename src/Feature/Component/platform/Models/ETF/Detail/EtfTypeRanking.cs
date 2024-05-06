namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfTypeRanking
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF名稱
        /// </summary>
        public string ETFName { get; set; }

        /// <summary>
        /// 六個月 報酬率 (淨值原幣)
        /// </summary>
        public string SixMonthReturnNetValueOriginalCurrency { get; set; }

        public string SixMonthReturnNetValueOriginalCurrencyStyle { get; set; }

        /// <summary>
        /// 價格(淨值)
        /// </summary>
        public string NetAssetValue { get; set; }
    }
}
