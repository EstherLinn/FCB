using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfDividendRecord
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 除息日
        /// </summary>
        public string ExDividendDate { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// 發放日
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 配息總額
        /// </summary>
        public string TotalDividendAmount { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendFrequency { get; set; }

        /// <summary>
        /// 短期資本利得
        /// </summary>
        public string ShortTermCapitalGains { get; set; }

        /// <summary>
        /// 長期資本利得
        /// </summary>
        public string LongTermCapitalGains { get; set; }
    }

    public class EtfDividend
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        public string ETFCode { get; set; }

        /// <summary>
        /// 除息日
        /// </summary>
        public DateTime? ExDividendDate { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// 發放日
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 配息總額
        /// </summary>
        public decimal? TotalDividendAmount { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendFrequency { get; set; }

        /// <summary>
        /// 短期資本利得
        /// </summary>
        public decimal? ShortTermCapitalGains { get; set; }

        /// <summary>
        /// 長期資本利得
        /// </summary>
        public decimal? LongTermCapitalGains { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }
    }
}