using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfIndustryHolding
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
        /// 產業名稱
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 百分比例
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal? Amount { get; set; }
    }

    public class EtfHolding
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
        /// 產業名稱
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 百分比例
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal? Amount { get; set; }
    }
}