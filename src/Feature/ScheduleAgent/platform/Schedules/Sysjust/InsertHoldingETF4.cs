using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertHoldingEtf4 : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertHoldingEtf4()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "SYSJUST-HOLDING-ETF-4";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustHoldingEtf4>(filename);

                    _repository.BulkInsertDirectToDatabase(basic, "[Sysjust_Holding_ETF_4_History]", filename);
                    _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Holding_ETF_4]", filename);

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