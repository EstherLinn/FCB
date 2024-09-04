using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.MemberCard
{
    public class MemberCardViewModel
    {
        public DateTime? ScheduleDate { get; set; }
        public int ScheduleSpace { get; set; }
        public string ScheduleMessage { get; set; }

        public MemberCardModel MemberCardModel { get; set; }
    }
}
