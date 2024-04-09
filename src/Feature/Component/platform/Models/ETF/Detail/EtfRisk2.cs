using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfRisk2
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
        /// 日期
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 六個月標準差
        /// </summary>
        public decimal? SixMonthStandardDeviation { get; set; }

        /// <summary>
        /// 一年標準差
        /// </summary>
        public decimal? OneYearStandardDeviation { get; set; }

        /// <summary>
        /// 三年標準差
        /// </summary>
        public decimal? ThreeYearStandardDeviation { get; set; }

        /// <summary>
        /// 五年標準差
        /// </summary>
        public decimal? FiveYearStandardDeviation { get; set; }

        /// <summary>
        /// 十年標準差
        /// </summary>
        public decimal? TenYearStandardDeviation { get; set; }

        /// <summary>
        /// 六個月 Sharpe
        /// </summary>
        public decimal? SixMonthSharpe { get; set; }

        /// <summary>
        /// 一年 Sharpe
        /// </summary>
        public decimal? OneYearSharpe { get; set; }

        /// <summary>
        /// 三年 Sharpe
        /// </summary>
        public decimal? ThreeYearSharpe { get; set; }

        /// <summary>
        /// 五年 Sharpe
        /// </summary>
        public decimal? FiveYearSharpe { get; set; }

        /// <summary>
        /// 十年 Sharpe
        /// </summary>
        public decimal? TenYearSharpe { get; set; }

        /// <summary>
        /// 六個月 Alpha
        /// </summary>
        public decimal? SixMonthAlpha { get; set; }

        /// <summary>
        /// 一年 Alpha
        /// </summary>
        public decimal? OneYearAlpha { get; set; }

        /// <summary>
        /// 三年 Alpha
        /// </summary>
        public decimal? ThreeYearAlpha { get; set; }

        /// <summary>
        /// 五年 Alpha
        /// </summary>
        public decimal? FiveYearAlpha { get; set; }

        /// <summary>
        /// 十年 Alpha
        /// </summary>
        public decimal? TenYearAlpha { get; set; }

        /// <summary>
        /// 六個月 Beta
        /// </summary>
        public decimal? SixMonthBeta { get; set; }

        /// <summary>
        /// 一年 Beta
        /// </summary>
        public decimal? OneYearBeta { get; set; }

        /// <summary>
        /// 三年 Beta
        /// </summary>
        public decimal? ThreeYearBeta { get; set; }

        /// <summary>
        /// 五年 Beta
        /// </summary>
        public decimal? FiveYearBeta { get; set; }

        /// <summary>
        /// 十年 Beta
        /// </summary>
        public decimal? TenYearBeta { get; set; }

        /// <summary>
        /// 六個月 R-squared
        /// </summary>
        public decimal? SixMonthRsquared { get; set; }

        /// <summary>
        /// 一年 R-squared
        /// </summary>
        public decimal? OneYearRsquared { get; set; }

        /// <summary>
        /// 三年 R-squared
        /// </summary>
        public decimal? ThreeYearRsquared { get; set; }

        /// <summary>
        /// 五年 R-squared
        /// </summary>
        public decimal? FiveYearRsquared { get; set; }

        /// <summary>
        /// 十年 R-squared
        /// </summary>
        public decimal? TenYearRsquared { get; set; }

        /// <summary>
        /// 六個月與指數相關係數
        /// </summary>
        public decimal? SixMonthCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 一年與指數相關係數
        /// </summary>
        public decimal? OneYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 三年與指數相關係數
        /// </summary>
        public decimal? ThreeYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 五年與指數相關係數
        /// </summary>
        public decimal? FiveYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 十年與指數相關係數
        /// </summary>
        public decimal? TenYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 六個月 Tracking Error
        /// </summary>
        public decimal? SixMonthTrackingError { get; set; }

        /// <summary>
        /// 一年 Tracking Error
        /// </summary>
        public decimal? OneYearTrackingError { get; set; }

        /// <summary>
        /// 三年 Tracking Error
        /// </summary>
        public decimal? ThreeYearTrackingError { get; set; }

        /// <summary>
        /// 五年 Tracking Error
        /// </summary>
        public decimal? FiveYearTrackingError { get; set; }

        /// <summary>
        /// 十年 Tracking Error
        /// </summary>
        public decimal? TenYearTrackingError { get; set; }
        /// <summary>
        /// 六個月 Variance
        /// </summary>
        public decimal? SixMonthVariance { get; set; }
        /// <summary>
        /// 一年 Variance
        /// </summary>
        public decimal? OneYearVariance { get; set; }
        /// <summary>
        /// 三年 Variance
        /// </summary>
        public decimal? ThreeYearVariance { get; set; }
        /// <summary>
        /// 五年 Variance
        /// </summary>
        public decimal? FiveYearVariance { get; set; }
        /// <summary>
        /// 十年 Variance
        /// </summary>
        public decimal? TenYearVariance { get; set; }
    }
}