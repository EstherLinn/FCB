using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertTfjsNav : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();
        public InsertTfjsNav()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "FUND_NAV_TFJSNAV";
            bool IsfilePath = await this._etlService.ExtractFile(filename);
            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<FundNavTfjsNav>(filename);
                    _repository.BulkInsertToNewDatabase(basic, "[FUND_NAV_TFJSNAV]", filename);

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