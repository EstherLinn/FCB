using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
   public class FundRiskGraph
    {
        public string FirstBankCode { get; set; }
        public DateTime Date { get; set; }
        public string FundName { get; set; }
        public decimal? OneYearStandardDeviation { get; set; }
        public decimal? TwoYearStandardDeviation { get; set; }
        public decimal? ThreeYearStandardDeviation { get; set; }
        public decimal? OneYearReturnOriginalCurrency { get; set; }
        public decimal? TwoYearReturnOriginalCurrency { get; set; }
        public decimal? ThreeYearReturnOriginalCurrency { get; set; }
    }
}
