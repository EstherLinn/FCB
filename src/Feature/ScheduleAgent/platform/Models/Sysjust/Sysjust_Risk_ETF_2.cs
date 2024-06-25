using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System.Globalization;
using System;

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
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// ETF 代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ETFCode { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        [TypeConverter(typeof(DateConverteryyMdd))]
        public string Date { get; set; }

        /// <summary>
        /// 六個月標準差
        /// </summary>
        [Index(3)]
        public decimal? SixMonthStandardDeviation { get; set; }

        /// <summary>
        /// 一年標準差
        /// </summary>
        [Index(4)]
        public decimal? OneYearStandardDeviation { get; set; }

        /// <summary>
        /// 三年標準差
        /// </summary>
        [Index(5)]
        public decimal? ThreeYearStandardDeviation { get; set; }

        /// <summary>
        /// 五年標準差
        /// </summary>
        [Index(6)]
        public decimal? FiveYearStandardDeviation { get; set; }

        /// <summary>
        /// 十年標準差
        /// </summary>
        [Index(7)]
        public decimal? TenYearStandardDeviation { get; set; }

        /// <summary>
        /// 六個月Sharpe
        /// </summary>
        [Index(8)]
        public decimal? SixMonthSharpe { get; set; }

        /// <summary>
        /// 一年Sharpe
        /// </summary>
        [Index(9)]
        public decimal? OneYearSharpe { get; set; }

        /// <summary>
        /// 三年Sharpe
        /// </summary>
        [Index(10)]
        public decimal? ThreeYearSharpe { get; set; }

        /// <summary>
        /// 五年Sharpe
        /// </summary>
        [Index(11)]
        public decimal? FiveYearSharpe { get; set; }

        /// <summary>
        /// 十年Sharpe
        /// </summary>
        [Index(12)]
        public decimal? TenYearSharpe { get; set; }

        /// <summary>
        /// 六個月Alpha
        /// </summary>
        [Index(13)]
        public decimal? SixMonthAlpha { get; set; }

        /// <summary>
        /// 一年Alpha
        /// </summary>
        [Index(14)]
        public decimal? OneYearAlpha { get; set; }

        /// <summary>
        /// 三年Alpha
        /// </summary>
        [Index(15)]
        public decimal? ThreeYearAlpha { get; set; }

        /// <summary>
        /// 五年Alpha
        /// </summary>
        [Index(16)]
        public decimal? FiveYearAlpha { get; set; }

        /// <summary>
        /// 十年Alpha
        /// </summary>
        [Index(17)]
        public decimal? TenYearAlpha { get; set; }

        /// <summary>
        /// 六個月Beta
        /// </summary>
        [Index(18)]
        public decimal? SixMonthBeta { get; set; }

        /// <summary>
        /// 一年Beta
        /// </summary>
        [Index(19)]
        public decimal? OneYearBeta { get; set; }

        /// <summary>
        /// 三年Beta
        /// </summary>
        [Index(20)]
        public decimal? ThreeYearBeta { get; set; }

        /// <summary>
        /// 五年Beta
        /// </summary>
        [Index(21)]
        public decimal? FiveYearBeta { get; set; }

        /// <summary>
        /// 十年Beta
        /// </summary>
        [Index(22)]
        public decimal? TenYearBeta { get; set; }

        /// <summary>
        /// 六個月RSquared
        /// </summary>
        [Index(23)]
        public decimal? SixMonthRsquared { get; set; }

        /// <summary>
        /// 一年RSquared
        /// </summary>
        [Index(24)]
        public decimal? OneYearRsquared { get; set; }

        /// <summary>
        /// 三年RSquared
        /// </summary>
        [Index(25)]
        public decimal? ThreeYearRsquared { get; set; }

        /// <summary>
        /// 五年RSquared
        /// </summary>
        [Index(26)]
        public decimal? FiveYearRsquared { get; set; }

        /// <summary>
        /// 十年RSquared
        /// </summary>
        [Index(27)]
        public decimal? TenYearRsquared { get; set; }

        /// <summary>
        /// 六個月與指數相關係數
        /// </summary>
        [Index(28)]
        public decimal? SixMonthCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 一年與指數相關係數
        /// </summary>
        [Index(29)]
        public decimal? OneYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 三年與指數相關係數
        /// </summary>
        [Index(30)]
        public decimal? ThreeYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 五年與指數相關係數
        /// </summary>
        [Index(31)]
        public decimal? FiveYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 十年與指數相關係數
        /// </summary>
        [Index(32)]
        public decimal? TenYearCorrelationCoefficientIndex { get; set; }

        /// <summary>
        /// 六個月 Tracking Error
        /// </summary>
        [Index(33)]
        public decimal? SixMonthTrackingError { get; set; }

        /// <summary>
        /// 一年 Tracking Error
        /// </summary>
        [Index(34)]
        public decimal? OneYearTrackingError { get; set; }

        /// <summary>
        /// 三年 Tracking Error
        /// </summary>
        [Index(35)]
        public decimal? ThreeYearTrackingError { get; set; }

        /// <summary>
        /// 五年 Tracking Error
        /// </summary>
        [Index(36)]
        public decimal? FiveYearTrackingError { get; set; }

        /// <summary>
        /// 十年 Tracking Error
        /// </summary>
        [Index(37)]
        public decimal? TenYearTrackingError { get; set; }

        /// <summary>
        /// 六個月Variance
        /// </summary>
        [Index(38)]
        public decimal? SixMonthVariance { get; set; }

        /// <summary>
        /// 一年 Variance
        /// </summary>
        [Index(39)]
        public decimal? OneYearVariance { get; set; }

        /// <summary>
        /// 三年 Variance
        /// </summary>
        [Index(40)]
        public decimal? ThreeYearVariance { get; set; }

        /// <summary>
        /// 五年 Variance
        /// </summary>
        [Index(41)]
        public decimal? FiveYearVariance { get; set; }

        /// <summary>
        /// 十年 Variance
        /// </summary>
        [Index(42)]
        public decimal? TenYearVariance { get; set; }

        /// <summary>
        /// 二年標準差
        /// </summary>
        [Index(43)]
        public decimal? TwoYearStandardDeviation { get; set; }
    }

    public class DateConverteryyMdd : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                var cultureInfo = new CultureInfo("zh-TW");
                var formats = new[] { "yyyy/M/dd", "yyyy/MM/dd" };
                if (DateTime.TryParseExact(text, formats, cultureInfo, DateTimeStyles.None, out DateTime establishment))
                {
                    return establishment.ToString("yyyy/MM/dd");
                }
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}