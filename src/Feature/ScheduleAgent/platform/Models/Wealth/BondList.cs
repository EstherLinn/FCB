using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(",")]
    [HasHeaderRecord(false)]
    public class BondList
    {
        [Index(0)]
        public string BondCode { get; set; }

        [Index(1)]
        public string ISINCode { get; set; }

        [Index(2)]
        public string BondName { get; set; }

        [Index(3)]
        public string Currency { get; set; }

        [Index(4)]
        public string CurrencyCode { get; set; }

        [Index(5)]
        public decimal? InterestRate { get; set; }

        [Index(6)]
        public int? PaymentFrequency { get; set; }

        [Index(7)]
        public string RiskLevel { get; set; }

        [Index(8)]
        public string SalesTarget { get; set; }

        [Index(9)]
        public int? MinSubscriptionForeign { get; set; }

        [Index(10)]
        public int? MinSubscriptionNTD { get; set; }

        [Index(11)]
        public int? MinIncrementAmount { get; set; }

        [Index(12)]
        public string MaturityDate { get; set; }

        [Index(13)]
        public string StopSubscriptionDate { get; set; }

        [Index(14)]
        public string RedemptionDateByIssuer { get; set; }

        [Index(15)]
        public string Issuer { get; set; }

        [Index(16)]
        public string OpenToPublic { get; set; }

        [Index(17)]
        public string Listed { get; set; }

        [Index(18)]
        public string ListingDate { get; set; }

        [Index(19)]
        public string DelistingDate { get; set; }
    }
}
