namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfRiskIndicator
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
        /// 六個月 Sharpe
        /// </summary>
        public string SixMonthSharpe { get; set; }

        /// <summary>
        /// 一年 Sharpe
        /// </summary>
        public string OneYearSharpe { get; set; }

        /// <summary>
        /// 三年 Sharpe
        /// </summary>
        public string ThreeYearSharpe { get; set; }

        /// <summary>
        /// 五年 Sharpe
        /// </summary>
        public string FiveYearSharpe { get; set; }

        /// <summary>
        /// 十年 Sharpe
        /// </summary>
        public string TenYearSharpe { get; set; }

        /// <summary>
        /// 六個月 Alpha
        /// </summary>
        public string SixMonthAlpha { get; set; }

        /// <summary>
        /// 一年 Alpha
        /// </summary>
        public string OneYearAlpha { get; set; }

        /// <summary>
        /// 三年 Alpha
        /// </summary>
        public string ThreeYearAlpha { get; set; }

        /// <summary>
        /// 五年 Alpha
        /// </summary>
        public string FiveYearAlpha { get; set; }

        /// <summary>
        /// 十年 Alpha
        /// </summary>
        public string TenYearAlpha { get; set; }

        /// <summary>
        /// 六個月 Beta
        /// </summary>
        public string SixMonthBeta { get; set; }

        /// <summary>
        /// 一年 Beta
        /// </summary>
        public string OneYearBeta { get; set; }

        /// <summary>
        /// 三年 Beta
        /// </summary>
        public string ThreeYearBeta { get; set; }

        /// <summary>
        /// 五年 Beta
        /// </summary>
        public string FiveYearBeta { get; set; }

        /// <summary>
        /// 十年 Beta
        /// </summary>
        public string TenYearBeta { get; set; }

        /// <summary>
        /// 六個月 R-squared
        /// </summary>
        public string SixMonthRsquared { get; set; }

        /// <summary>
        /// 一年 R-squared
        /// </summary>
        public string OneYearRsquared { get; set; }

        /// <summary>
        /// 三年 R-squared
        /// </summary>
        public string ThreeYearRsquared { get; set; }

        /// <summary>
        /// 五年 R-squared
        /// </summary>
        public string FiveYearRsquared { get; set; }

        /// <summary>
        /// 十年 R-squared
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
        /// 六個月 Tracking Error
        /// </summary>
        public string SixMonthTrackingError { get; set; }

        /// <summary>
        /// 一年 Tracking Error
        /// </summary>
        public string OneYearTrackingError { get; set; }

        /// <summary>
        /// 三年 Tracking Error
        /// </summary>
        public string ThreeYearTrackingError { get; set; }

        /// <summary>
        /// 五年 Tracking Error
        /// </summary>
        public string FiveYearTrackingError { get; set; }

        /// <summary>
        /// 十年 Tracking Error
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

        #region 樣式

        public string SixMonthStandardDeviationStyle { get; set; }
        public string OneYearStandardDeviationStyle { get; set; }
        public string ThreeYearStandardDeviationStyle { get; set; }
        public string FiveYearStandardDeviationStyle { get; set; }
        public string TenYearStandardDeviationStyle { get; set; }
        public string SixMonthSharpeStyle { get; set; }
        public string OneYearSharpeStyle { get; set; }
        public string ThreeYearSharpeStyle { get; set; }
        public string FiveYearSharpeStyle { get; set; }
        public string TenYearSharpeStyle { get; set; }
        public string SixMonthAlphaStyle { get; set; }
        public string OneYearAlphaStyle { get; set; }
        public string ThreeYearAlphaStyle { get; set; }
        public string FiveYearAlphaStyle { get; set; }
        public string TenYearAlphaStyle { get; set; }
        public string SixMonthBetaStyle { get; set; }
        public string OneYearBetaStyle { get; set; }
        public string ThreeYearBetaStyle { get; set; }
        public string FiveYearBetaStyle { get; set; }
        public string TenYearBetaStyle { get; set; }
        public string SixMonthRsquaredStyle { get; set; }
        public string OneYearRsquaredStyle { get; set; }
        public string ThreeYearRsquaredStyle { get; set; }
        public string FiveYearRsquaredStyle { get; set; }
        public string TenYearRsquaredStyle { get; set; }
        public string SixMonthCorrelationCoefficientIndexStyle { get; set; }
        public string OneYearCorrelationCoefficientIndexStyle { get; set; }
        public string ThreeYearCorrelationCoefficientIndexStyle { get; set; }
        public string FiveYearCorrelationCoefficientIndexStyle { get; set; }
        public string TenYearCorrelationCoefficientIndexStyle { get; set; }
        public string SixMonthTrackingErrorStyle { get; set; }
        public string OneYearTrackingErrorStyle { get; set; }
        public string ThreeYearTrackingErrorStyle { get; set; }
        public string FiveYearTrackingErrorStyle { get; set; }
        public string TenYearTrackingErrorStyle { get; set; }
        public string SixMonthVarianceStyle { get; set; }
        public string OneYearVarianceStyle { get; set; }
        public string ThreeYearVarianceStyle { get; set; }
        public string FiveYearVarianceStyle { get; set; }
        public string TenYearVarianceStyle { get; set; }

        #endregion 樣式
    }
}