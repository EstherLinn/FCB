using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundAnnunalRateOfReturn
    {
        public string FirstBankCode { get; set; }
        public int Year { get; set; }
        public decimal? AnnualReturnRateOriginalCurrency { get; set; }
        public decimal? AnnualReturnRateTWD { get; set; }
        public decimal? IndicatorIndexPriceChange { get; set; }
        public decimal? Difference { get; set; }
    }
}
