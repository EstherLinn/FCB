using Feature.Wealth.ScheduleAgent.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class ScheduledAlarm : SitecronAgentBase
    {
        private readonly ScheduleAlarmRespository consultMailRespository;
        public ScheduledAlarm()
        {
            this.consultMailRespository = new ScheduleAlarmRespository(this.Logger);
        }

        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var jobItem = this.JobItems.FirstOrDefault();
                var data = consultMailRespository.GetChangeHistory();
                consultMailRespository.SendMail(data, jobItem);
            }
            else
            {
                this.Logger.Warn($"Not Setting Any JobItems");
            }
        }
        
    }
}
