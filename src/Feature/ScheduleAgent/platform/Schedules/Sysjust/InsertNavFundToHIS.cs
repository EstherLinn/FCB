using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Threading;

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

                int threadId = Thread.CurrentThread.ManagedThreadId;

                string fileName = "SYSJUST-NAV-FUND";
                var IsfilePath = await etlService.ExtractFile(fileName);
                var scheduleName = ScheduleName.InsertNavFundToHis.ToString();
                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustNavFundToHis>(fileName);
                        _repository.BulkInsertToDatabaseForHIS(basic, "[Sysjust_FUNDNAV_HIS]", "FirstBankCode", "Date", fileName, startTime, scheduleName, threadId);
                        etlService.FinishJob(fileName, startTime, scheduleName, threadId);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(fileName, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(fileName, IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
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