using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertFundsizeEtf : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);
                string filename = "SYSJUST-FUNDSIZE-ETF";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustFundSizeEtf>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Fundsize_ETF]", filename);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Fundsize_ETF_History]", "ScaleDate", "FirstBankCode", filename);
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