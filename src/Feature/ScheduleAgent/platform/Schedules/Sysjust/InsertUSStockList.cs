using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertUsStockList : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertUsStockList()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "SYSJUST-USSTOCKLIST";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustUsStockList>(filename);
                    _repository.BulkInsertDirectToDatabase(basic, "[Sysjust_USStockList_History]", filename);
                    _repository.BulkInsertToNewDatabase(basic, "[Sysjust_USStockList]", filename);
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