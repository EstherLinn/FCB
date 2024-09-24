using Feature.Wealth.ScheduleAgent.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
    public class ConsultMail : SitecronAgentBase
    {
        private readonly ConsultMailRespository consultMailRespository;
        public ConsultMail()
        {
            this.consultMailRespository = new ConsultMailRespository(this.Logger);
        }
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                var jobItem = this.JobItems.FirstOrDefault();
                var data = consultMailRespository.GetAllMemebrConsults();
                consultMailRespository.SendMail(data, jobItem);
            });
        }
    }
}
