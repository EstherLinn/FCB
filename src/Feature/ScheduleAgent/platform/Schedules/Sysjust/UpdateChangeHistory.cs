using System;
using System.Linq;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Foundation.Wealth.Manager;
using System.Data;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class UpdateChangeHistory : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            string sql = "SELECT * FROM [ChangeHistory] WITH (NOLOCK)";
            var results = await DbManager.Custom.ExecuteIListAsync<ChangeHistory>(sql, null, CommandType.Text);

            var newresults = results.Where(f => f.ModificationDate >= DateTime.Today.AddMonths(-1));

            try
            {
                _repository.BulkInsertToNewDatabase(newresults, "[ChangeHistory]", sql, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, "ChangeHistory", ex.Message, " ", 0, 0, "N");
            }

        }
    }
}