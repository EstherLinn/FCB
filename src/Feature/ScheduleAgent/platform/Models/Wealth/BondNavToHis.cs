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
    public class BondNavToHis
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

        [Index(6)]
        public string BondName { get; set; }
    }
}
