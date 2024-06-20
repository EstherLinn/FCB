using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
    interface IMailInfo<T>
    {
        public void SendMail(IEnumerable<T> infos);
    }
}
