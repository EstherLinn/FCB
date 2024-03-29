using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
  public class FundStockHolding
    {
        public string FirstBankCode { get; set; }
        public string Date { get; set; }
        public string FundName { get; set; }
        public string Category { get; set; }
        public decimal? Holding { get; set; }
        public string Currency { get; set; }
    }
}
