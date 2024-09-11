using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.SignalStatus;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertNavFundToHis : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string fileName = "SYSJUST-NAV-FUND";
                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustNavFundToHis>(fileName);
                        _repository.BulkInsertToDatabaseForHIS(basic, "[Sysjust_FUNDNAV_HIS]", "FirstBankCode", "Date", fileName, startTime);
                        etlService.FinishJob(fileName, startTime);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, fileName, ex.Message, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, fileName, IsfilePath.Key, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                }
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            else
            {
                this.Logger.Warn($"Not Setting Any JobItems");
            }
        }
    }
}