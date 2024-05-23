using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Services;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertFundSize : SitecronAgentBase
    {

        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();
        public InsertFundSize()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["FundSize"];
            string tableName = "[FUND_SIZE]";

            bool IsfilePath = await this._etlService.ExtractFile("FUND_SIZE");

            if (IsfilePath)
            {
                try
                {
                    var basic = _etlService.ParseFixedLength<FundSize>(filePath);
                    _repository.BulkInsertToNewDatabase(basic, tableName, filePath);

                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, "", 0);
                }

            }
            else
            {
                _repository.LogChangeHistory(DateTime.UtcNow, "ERROR: File not found", "找不到檔案", "", 0);
            }
        }
    }
}