using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金各期別績效評比，檔案名稱：SYSJUST-RISK-FUND.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustRiskFund
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        public string SysjustCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 六個月標準差
        /// </summary>
        public string SixMonthStandardDeviation { get; set; }

        /// <summary>
        /// 一年標準差
        /// </summary>
        public string OneYearStandardDeviation { get; set; }

        /// <summary>
        /// 三年標準差
        /// </summary>
        public string ThreeYearStandardDeviation { get; set; }

        /// <summary>
        /// 五年標準差
        /// </summary>
        public string FiveYearStandardDeviation { get; set; }

        /// <summary>
        /// 十年標準差
        /// </summary>
        public string TenYearStandardDeviation { get; set; }

        /// <summary>
        /// 六個月Sharpe
        /// </summary>
        public string SixMonthSharpe { get; set; }

        /// <summary>
        /// 一年Sharpe
        /// </summary>
        public string OneYearSharpe { get; set; }

        /// <summary>
        /// 三年Sharpe
        /// </summary>
        public string ThreeYearSharpe { get; set; }

        /// <summary>
        /// 五年Sharpe
        /// </summary>
        public string FiveYearSharpe { get; set; }

        /// <summary>
        /// 十年Sharpe
        /// </summary>
        public string TenYearSharpe { get; set; }

        /// <summary>
        /// 六個月Alpha
        /// </summary>
        public string SixMonthAlpha { get; set; }

        /// <summary>
        /// 一年Alpha
        /// </summary>
        public string OneYearAlpha { get; set; }

        /// <summary>
        /// 三年Alpha
        /// </summary>
        public string ThreeYearAlpha { get; set; }

        /// <summary>
        /// 五年Alpha
        /// </summary>
        public string FiveYearAlpha { get; set; }

        /// <summary>
        /// 十年Alpha
        /// </summary>
        public string TenYearAlpha { get; set; }

        /// <summary>
        /// 六個月Beta
        /// </summary>
        public string SixMonthBeta { get; set; }

        /// <summary>
        /// 一年Beta
        /// </summary>
        public string OneYearBeta { get; set; }

        /// <summary>
        /// 三年Beta
        /// </summary>
        public string ThreeYearBeta { get; set; }

        /// <summary>
        /// 五年Beta
        /// </summary>
        public string FiveYearBeta { get; set; }

        /// <summary>
        /// 十年Beta
        /// </summary>
        public string TenYearBeta { get; set; }

        /// <summary>
        /// 六個月RSquared
        /// </summary>
        public string SixMonthRsquared { get; set; }

        /// <summary>
        /// 一年RSquared
        /// </summary>
        public string OneYearRsquared { get; set; }

        /// <summary>
        /// 三年RSquared
        /// </summary>
        public string ThreeYearRsquared { get; set; }

        /// <summary>
        /// 五年RSquared
        /// </summary>
        public string FiveYearRsquared { get; set; }

        /// <summary>
        /// 十年RSquared
        /// </summary>
        public string TenYearRsquared { get; set; }

        /// <summary>
        /// 六個月與指數相關係數
        /// </summary>
        public string SixMonthCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 一年與指數相關係數
        /// </summary>
        public string OneYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 三年與指數相關係數
        /// </summary>
        public string ThreeYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 五年與指數相關係數
        /// </summary>
        public string FiveYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 十年與指數相關係數
        /// </summary>
        public string TenYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 六個月Tracking Error
        /// </summary>
        public string SixMonthTrackingError { get; set; }

        /// <summary>
        /// 一年Tracking Error
        /// </summary>
        public string OneYearTrackingError { get; set; }

        /// <summary>
        /// 三年Tracking Error
        /// </summary>
        public string ThreeYearTrackingError { get; set; }

        /// <summary>
        /// 五年Tracking Error
        /// </summary>
        public string FiveYearTrackingError { get; set; }

        /// <summary>
        /// 十年Tracking Error
        /// </summary>
        public string TenYearTrackingError { get; set; }

        /// <summary>
        /// 六個月 Variance
        /// </summary>
        public string SixMonthVariance { get; set; }

        /// <summary>
        /// 一年 Variance 
        /// </summary>
        public string OneYearVariance { get; set; }

        /// <summary>
        /// 三年 Variance 
        /// </summary>
        public string ThreeYearVariance { get; set; }

        /// <summary>
        /// 五年 Variance 
        /// </summary>
        public string FiveYearVariance { get; set; }

        /// <summary>
        /// 十年 Variance 
        /// </summary>
        public string TenYearVariance { get; set; }
    }
}