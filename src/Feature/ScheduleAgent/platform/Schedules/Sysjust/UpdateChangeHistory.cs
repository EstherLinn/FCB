using Feature.Wealth.ScheduleAgent.Models.DefinedScheduleAgent;
using Feature.Wealth.ScheduleAgent.Models.ScheduleContext;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class UpdateChangeHistory : DefinedScheduleAgent
    {
        protected override async Task ExecuteJobItems()
        {
            var startTime = DateTime.UtcNow;
            var _repository = new ProcessRepository(this.Logger);

            var context = new ScheduleContext
            {
                StartTime = startTime,
                ScheduleName = ScheduleName.UpdateChangeHistory.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                TaskExecutionId = new ShortID(Guid.NewGuid()).ToString()
            };

            string sql = "SELECT * FROM [ChangeHistory] WITH (NOLOCK)";
            var results = await DbManager.Custom.ExecuteIListAsync<ChangeHistory>(sql, null, CommandType.Text);

            var newresults = results.Where(f => f.ModificationDate >= DateTime.Today.AddMonths(-3));

            try
            {

                _repository.BulkInsertToNewDatabase(newresults, "ChangeHistory", "UpdateChangeHistory", context);
                _repository.LogChangeHistory("UpdateChangeHistory", "ChangeHistory更新完成", "ChangeHistory", 0, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.Done, context);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("ChangeHistory", ex.Message, string.Empty, 0, 0, "N", ModificationID.Error, context);
            }

        }
    }
}