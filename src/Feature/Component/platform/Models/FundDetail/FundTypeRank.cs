using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
   public class FundTypeRank
    {
        public string FirstBankCode { get; set; }
        public string FundName { get; set; }
        public string FundEnglishName { get; set; }
        public decimal? SixMonthReturnOriginalCurrency { get; set; }
        public decimal? NetAssetValue { get; set; }
    }
}
