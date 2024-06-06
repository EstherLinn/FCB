using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertRiskFund2Domestic : SitecronAgentBase
    {
        private readonly ProcessRepository _repository = new();

        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filename = "SYSJUST-RISK-FUND-2-DOMESTIC";
                bool IsfilePath = etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustRiskFund2Domestic>(filename);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Risk_Fund_2_Domestic_History]", "FirstBankCode","Date", filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Risk_Fund_2_Domestic]", filename);
                        etlService.FinishJob(filename);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, " ", 0);
                    }
                }
                else
                {
                    this.Logger.Error("ERROR: File not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0);
                }
            }
        }
    }
}