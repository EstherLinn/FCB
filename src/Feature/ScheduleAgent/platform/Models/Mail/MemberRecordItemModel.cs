using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
    /// <summary>
    /// 週月報／優惠活動用
    /// </summary>
    public class MemberRecordItemModel
    {
        public string PlatFormId { get; set; }

        public string MemberEmail { get; set; }
        public string ItemId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

    }
}
