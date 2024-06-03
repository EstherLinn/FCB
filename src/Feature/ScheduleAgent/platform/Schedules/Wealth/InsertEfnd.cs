using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertEfnd : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();
        public InsertEfnd()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "EFND";
            string tableName = "[EFND]";

            bool IsfilePath = await this._etlService.ExtractFile("EFND");

            if (IsfilePath)
            {
                try
                {
                    var basic = _etlService.ParseFixedLength<Efnd>(filename);
                    _repository.BulkInsertToNewDatabase(basic, tableName, filename);

                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, "", 0);
                }

            }
            else
            {
                _repository.LogChangeHistory(DateTime.UtcNow, "ERROR: File not found", "找不到檔案", "", 0);
            }
        }
    }
}