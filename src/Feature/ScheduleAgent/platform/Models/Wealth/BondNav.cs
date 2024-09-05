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
    public class BondNav
    {
        [Index(0)]
        public string BondCode { get; set; }

        [Index(1)]
        public string Currency { get; set; }

        [Index(2)]
        public decimal? SubscriptionFee { get; set; }

        [Index(3)]
        public decimal? RedemptionFee { get; set; }

        [Index(4)]
        public string Date { get; set; }

        [Index(5)]
        public string ReservedColumn { get; set; }

        [Index(6)]
        public string BondName { get; set; }

        [Index(7)]
        public string Note { get; set; }

        [Index(8)]
        public decimal? PreviousInterest { get; set; }

        [Index(9)]
        public string SPCreditRating { get; set; }

        [Index(10)]
        public string MoodyCreditRating { get; set; }

        [Index(11)]
        public string FitchCreditRating { get; set; }

        [Index(12)]
        public decimal? YieldRateYTM { get; set; }
    }




}
