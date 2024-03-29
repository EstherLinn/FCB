using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
   public class FundThiryDays
    {

        public string FirstBankCode { get; set; }
        public string NetAssetValueDate { get; set; }
        public decimal? NetAssetValue { get; set; }
        public decimal? PreviousNetAssetValue { get; set; }
        public decimal? FundPriceChange { get; set; }
        public decimal? PercentageChangeInFundPrice { get; set; }
    }
}
