using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertCompanyFund2 : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertCompanyFund2()
        {
            this._etlService = new EtlService(this.Logger, (Sitecore.Data.Items.Item)this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "SYSJUST-COMPANY-FUND-2";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustCompanyFund2>(filename);
                    _repository.BulkInsertToDatabase(basic, "[Sysjust_Company_fund_2]", "FundCompanyCode", "FundCompanyCode", filename);
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