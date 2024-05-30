using Feature.Wealth.Component.Models.FundDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Models.FocusList
{
     public  class FundListModel : FocusListBaseModel
    {
        public decimal? NetAssetValue { get; set; }
        public string NetAssetValueDate { get; set; }
        public decimal? OneMonthReturnOriginalCurrency { get; set; }
        public decimal? SixMonthReturnOriginalCurrency { get; set; }
        public decimal? OneYearReturnOriginalCurrency { get; set; }

    }
}
