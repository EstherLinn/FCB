using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundRiskindicators
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public decimal? SixMonthStandardDeviation { get; set; }
        public decimal? OneYearStandardDeviation { get; set; }
        public decimal? ThreeYearStandardDeviation { get; set; }
        public decimal? FiveYearStandardDeviation { get; set; }
        public decimal? TenYearStandardDeviation { get; set; }
        public decimal? SixMonthSharpe { get; set; }
        public decimal? OneYearSharpe { get; set; }
        public decimal? ThreeYearSharpe { get; set; }
        public decimal? FiveYearSharpe { get; set; }
        public decimal? TenYearSharpe { get; set; }
        public decimal? SixMonthAlpha { get; set; }
        public decimal? OneYearAlpha { get; set; }
        public decimal? ThreeYearAlpha { get; set; }
        public decimal? FiveYearAlpha { get; set; }
        public decimal? TenYearAlpha { get; set; }
        public decimal? SixMonthBeta { get; set; }
        public decimal? OneYearBeta { get; set; }
        public decimal? ThreeYearBeta { get; set; }
        public decimal? FiveYearBeta { get; set; }
        public decimal? TenYearBeta { get; set; }
        public decimal? SixMonthRsquared { get; set; }
        public decimal? OneYearRsquared { get; set; }
        public decimal? ThreeYearRsquared { get; set; }
        public decimal? FiveYearRsquared { get; set; }
        public decimal? TenYearRsquared { get; set; }
        public decimal? SixMonthCorrelationCoefficientIndex { get; set; }
        public decimal? OneYearCorrelationCoefficientIndex { get; set; }
        public decimal? ThreeYearCorrelationCoefficientIndex { get; set; }
        public decimal? FiveYearCorrelationCoefficientIndex { get; set; }
        public decimal? TenYearCorrelationCoefficientIndex { get; set; }
        public decimal? SixMonthTrackingError { get; set; }
        public decimal? OneYearTrackingError { get; set; }
        public decimal? ThreeYearTrackingError { get; set; }
        public decimal? FiveYearTrackingError { get; set; }
        public decimal? TenYearTrackingError { get; set; }
        public decimal? SixMonthVariance { get; set; }
        public decimal? OneYearVariance { get; set; }
        public decimal? ThreeYearVariance { get; set; }
        public decimal? FiveYearVariance { get; set; }
        public decimal? TenYearVariance { get; set; }
    }
}
