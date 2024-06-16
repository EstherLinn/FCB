using Feature.Wealth.Component.Models.FundDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FocusList
{
   public class FocusListBaseModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CurrencyName { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
        public string AvailabilityStatus { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public Button Button { get; set; }

        public Info Info { get; set; }
    }

    public class Button
    {
        public string InfoButtonHtml { get; set; }
        public string FocusButtonHtml { get; set; }
        public string CompareButtonHtml { get; set; }
        public string SubscriptionButtonHtml { get; set; }
    }
    public class Info
    {
        public string InvestId { get; set; }
        public decimal? PriceValue { get; set; }
        public decimal? RiseFallPriceValue { get; set; }
        public decimal? ReachValue { get; set; }
        public decimal? RiseValue { get; set; }
        public decimal? FallValue { get; set; }
        public decimal? RisePercent { get; set; }
        public decimal? FallPercent { get; set; }
    }
}
