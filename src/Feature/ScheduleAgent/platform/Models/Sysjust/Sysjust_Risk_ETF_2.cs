using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金各期別績效評比，檔案名稱：SYSJUST-RISK-ETF-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskEtf2
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
        /// Date
        /// </summary>
        public string Date { get; set; }

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
        /// 六個月Sharpe
        /// </summary>
        public decimal? SixMonthSharpe { get; set; }

        /// <summary>
        /// 一年Sharpe
        /// </summary>
        public decimal? OneYearSharpe { get; set; }

        /// <summary>
        /// 三年Sharpe
        /// </summary>
        public decimal? ThreeYearSharpe { get; set; }

        /// <summary>
        /// 五年Sharpe
        /// </summary>
        public decimal? FiveYearSharpe { get; set; }

        /// <summary>
        /// 十年Sharpe
        /// </summary>
        public decimal? TenYearSharpe { get; set; }

        /// <summary>
        /// 六個月Alpha
        /// </summary>
        public decimal? SixMonthAlpha { get; set; }

        /// <summary>
        /// 一年Alpha
        /// </summary>
        public decimal? OneYearAlpha { get; set; }

        /// <summary>
        /// 三年Alpha
        /// </summary>
        public decimal? ThreeYearAlpha { get; set; }

        /// <summary>
        /// 五年Alpha
        /// </summary>
        public decimal? FiveYearAlpha { get; set; }

        /// <summary>
        /// 十年Alpha
        /// </summary>
        public decimal? TenYearAlpha { get; set; }

        /// <summary>
        /// 六個月Beta
        /// </summary>
        public decimal? SixMonthBeta { get; set; }

        /// <summary>
        /// 一年Beta
        /// </summary>
        public decimal? OneYearBeta { get; set; }

        /// <summary>
        /// 三年Beta
        /// </summary>
        public decimal? ThreeYearBeta { get; set; }

        /// <summary>
        /// 五年Beta
        /// </summary>
        public decimal? FiveYearBeta { get; set; }

        /// <summary>
        /// 十年Beta
        /// </summary>
        public decimal? TenYearBeta { get; set; }

        /// <summary>
        /// 六個月RSquared
        /// </summary>
        public decimal? SixMonthRsquared { get; set; }

        /// <summary>
        /// 一年RSquared
        /// </summary>
        public decimal? OneYearRsquared { get; set; }

        /// <summary>
        /// 三年RSquared
        /// </summary>
        public decimal? ThreeYearRsquared { get; set; }

        /// <summary>
        /// 五年RSquared
        /// </summary>
        public decimal? FiveYearRsquared { get; set; }

        /// <summary>
        /// 十年RSquared
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
        /// 六個月Variance
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

        /// <summary>
        /// 二年標準差
        /// </summary>
        public decimal? TwoYearStandardDeviation { get; set; }
    }
}