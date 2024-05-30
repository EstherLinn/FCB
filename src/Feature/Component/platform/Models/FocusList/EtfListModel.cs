using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FocusList
{
   public class EtfListModel : FocusListBaseModel
    {
        public decimal? MarketPrice { get; set; }
        public string MarketPriceDate { get; set; }

        public decimal? SixMonthReturnMarketPriceOriginalCurrency { get; set; }
        public decimal? NetAssetValueChangePercentage { get; set; }
    }
}
