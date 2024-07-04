using Feature.Wealth.ScheduleAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
    public class WeekMonthlyReportMail : SitecronAgentBase
    {
        private readonly WeekMonthlyRespository _weekMonthlyRepository ;

        public WeekMonthlyReportMail()
        {
            this._weekMonthlyRepository = new WeekMonthlyRespository(this.Logger);
        }

        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                var data = _weekMonthlyRepository.GetWeekMonthlyReports();
                _weekMonthlyRepository.SendMail(data);
            });
        }
    }
}
