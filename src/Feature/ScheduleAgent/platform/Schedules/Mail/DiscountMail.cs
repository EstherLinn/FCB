using Feature.Wealth.ScheduleAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
     public class DiscountMail : SitecronAgentBase
    {
        private readonly DiscountActivityRespository _discountActivityRespository;

        public DiscountMail()
        {
            this._discountActivityRespository = new DiscountActivityRespository(this.Logger);
        }

        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                var data = _discountActivityRespository.GetDiscounts();
                _discountActivityRespository.SendMail(data);
            });
        }
    }
}
