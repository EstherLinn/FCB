using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundYearRateOfReturn
    {
        public string FirstBankCode { get; set; }
        public DateTime Date { get; set; }
        public decimal? AnnualizedStandardDeviation { get; set; }
        public decimal? Beta { get; set; }
        public decimal? Sharpe { get; set; }
        public decimal? InformationRatio { get; set; }
        public decimal? JensenIndex { get; set; }
        public decimal? TreynorIndex { get; set; }
    }
}
