using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfStrockHolding
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 個股代碼
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public string Percentage { get; set; }

        /// <summary>
        /// 持有股數
        /// </summary>
        public string NumberofSharesHeld { get; set; }
    }

    public class EtfHolding3
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 個股代碼
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 個股名稱
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// 持有股數
        /// </summary>
        public decimal? NumberofSharesHeld { get; set; }
    }
}