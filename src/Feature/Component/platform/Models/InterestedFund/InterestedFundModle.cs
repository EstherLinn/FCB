using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.InterestedFund
{
    public class InterestedFundModle
    {
        public IList<Fund> Funds {  get; set; }
    }

    public class Fund
    {
        public string DetailUrl { get; set; }
        public string FundName { get; set; }
        public string FundId { get; set; }
        public string RateOfRetuen { get; set; }
        public string FocusButtonHtml { get; set; }
        public string CompareButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }

    }

    //從DB獲取資料
    public class FundModel
    {
        public string FundId { get; set; }
        public string FundName { get; set; }
        public decimal? OneMonthReturnOriginalCurrency { get; set; }
        public string AvailabilityStatus { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
    }


}
