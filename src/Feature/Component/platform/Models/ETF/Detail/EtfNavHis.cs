using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    internal class EtfNavHis
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }
    }
}