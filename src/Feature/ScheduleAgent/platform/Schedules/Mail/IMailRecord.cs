using System.Collections.Generic;
namespace Feature.Wealth.ScheduleAgent.Schedules.Mail
{
    interface IMailRecord<T>
    {
        public void InsertMailRecords(IEnumerable<T> infos);
    }
}
