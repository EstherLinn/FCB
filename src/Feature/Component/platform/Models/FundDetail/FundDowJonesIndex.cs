using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
   public class FundDowJonesIndex
    {
        public string FirstBankCode { get; set; }
        public DateTime Date { get; set; }
        public decimal? MonthlyReturnRate { get; set; }
        public decimal? IndicatorIndexPriceChange { get; set; }
        public decimal? Difference { get; set; }
    }
}
