using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.MemberLog
{
    public class MemberLog
    {
        public string PlatForm { get; set; }
        public string PlatFormId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime ActionTime { get; set; }

    }
}
