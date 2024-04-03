using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundDividendRecord
    {
        public string FirstBankCode { get; set; }
        public string ExDividendDate { get; set; }
        public string BaseDate { get; set; }
        public string ReleaseDate { get; set; }
        public decimal? Dividend { get; set; }
        public decimal? AnnualizedDividendRate { get; set; }
        public string Currency { get; set; }
    }
}
