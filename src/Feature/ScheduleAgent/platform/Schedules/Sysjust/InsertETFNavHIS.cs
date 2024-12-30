using System;
using System.Linq;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Collections.Generic;
using System.Threading;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertEtfNavHis : SitecronAgentBase
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

                var scheduleName = ScheduleName.InsertEtfNavHIS.ToString();
                string filename = "SYSJUST-ETFNAV-HIS";
                var IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<SysjustEtfNavHis>)await etlService.ParseCsv<SysjustEtfNavHis>(filename);
                        _repository.BulkInsertToEncryptedDatabase(basic, "[Sysjust_ETFNAV_HIS]", filename, startTime, scheduleName, threadId);
                        etlService.FinishJob(filename, startTime, scheduleName, threadId);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(filename, ex.Message, IsfilePath.Key, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(filename, "找不到檔案或檔案相同不執行", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                }
            }
        }
    }
}