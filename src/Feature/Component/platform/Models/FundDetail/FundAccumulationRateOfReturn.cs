using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
  public  class FundAccumulationRateOfReturn
    {
        public string FirstBankCode { get; set; }
        public decimal? OneWeekReturnOriginalCurrency { get; set; }
        public decimal? MonthtoDateReturnOriginalCurrency { get; set; }
        public decimal? YeartoDateReturnOriginalCurrency { get; set; }
        public decimal? OneMonthReturnOriginalCurrency { get; set; }
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }
        public decimal? SixMonthReturnOriginalCurrency { get; set; }
        public decimal? OneYearReturnOriginalCurrency { get; set; }
        public decimal? TwoYearReturnOriginalCurrency { get; set; }
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }
        public decimal? FiveYearReturnOriginalCurrency { get; set; }
        public DateTime DataDate { get; set; }
    }
}
