using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
    public class PriceInfoMail : SitecronAgentBase
    {
       private readonly  MailRepository mailRepository;
        public PriceInfoMail()
        {
            this.mailRepository = new MailRepository(this.Logger);
        }
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                var jobItem = this.JobItems.FirstOrDefault();
                var data = mailRepository.GetAllMemebrReachInfos();
                mailRepository.SendMail(data, jobItem);
            });
        }
    }
}
