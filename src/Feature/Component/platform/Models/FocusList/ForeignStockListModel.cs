using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FocusList
{
   public class ForeignStockListModel : FocusListBaseModel
    {
        public string FundCode { get; set; }
        public decimal? ClosingPrice { get; set; }
        public string ClosingPriceDate { get; set; }
        public decimal? ChangePercentage { get; set; }
        public decimal? MonthlyReturn { get; set; }
    }
}
