using Feature.Wealth.Account.Models.ReachInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
   public class MemebrReachInfo : ReachInfo
    {
        public string MemberEmail { get; set; }
        public string ProductName { get; set; }

        public decimal? NewestValue { get; set; }

        public string NewestDate{ get; set; }

    }
}
