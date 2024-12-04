using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Manager;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class UpdateChangeHistory : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            var _repository = new ProcessRepository(this.Logger);

            string sql = "SELECT * FROM [ChangeHistory] WITH (NOLOCK)";
            var results = await DbManager.Custom.ExecuteIListAsync<ChangeHistory>(sql, null, CommandType.Text);

            var newresults = results.Where(f => f.ModificationDate >= DateTime.Today.AddMonths(-2));
            var scheduleName = ScheduleName.UpdateChangeHistory.ToString();
            try
            {
                
                _repository.BulkInsertToNewDatabase(newresults, "ChangeHistory", "UpdateChangeHistory", DateTime.UtcNow, scheduleName);
                _repository.LogChangeHistory("UpdateChangeHistory", "ChangeHistory更新完成", "ChangeHistory", 0, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.Done, scheduleName);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("ChangeHistory", ex.Message, string.Empty, 0, 0, "N", ModificationID.Error, scheduleName);
            }

        }
    }
}