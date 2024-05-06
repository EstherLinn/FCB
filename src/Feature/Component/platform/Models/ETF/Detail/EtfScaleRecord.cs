namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfScaleRecord
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 基金代碼
        /// </summary>
        public string FundCode { get; set; }

        /// <summary>
        /// 規模(百萬)
        /// </summary>
        public decimal? ScaleMillions { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        public string ScaleDate { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }
    }
}