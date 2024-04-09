namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfHistoryNetWorth
    {
        /// <summary>
        /// 日期
        /// </summary>
        /// <remarks>原欄位：DataDate</remarks>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 漲跌幅
        /// </summary>
        /// <remarks>原欄位：ChangePercentage</remarks>
        public decimal? NetAssetValue { get; set; }
    }
}
