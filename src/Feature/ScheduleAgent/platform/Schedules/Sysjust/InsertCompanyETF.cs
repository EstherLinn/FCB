using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Globalization;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertCompanyEtf : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertCompanyEtf()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }
        protected override async Task Execute()
        {
            string filename = "SYSJUST-COMPANY-ETF";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {

                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustCompanyEtf>(filename);
                    _repository.BulkInsertToDatabase(basic, "[Sysjust_Company_ETF]", "IssueCompanyID", "IssueCompanyID", filename);

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