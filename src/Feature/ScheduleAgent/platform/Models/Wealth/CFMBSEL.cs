using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    public class Cfmbsel
    {
        public DateTime? EXT_DATE { get; set; }
        public string CUST_ID { get; set; }
        public string TELLER_CODE { get; set; }
        public string PROMOTION_CODE { get; set; }
        public DateTime? LOAD_DATE { get; set; }
    }
}
