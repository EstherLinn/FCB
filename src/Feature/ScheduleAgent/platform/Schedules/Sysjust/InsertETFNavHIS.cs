using Feature.Wealth.ScheduleAgent.Models.DefinedScheduleAgent;
using Feature.Wealth.ScheduleAgent.Models.ScheduleContext;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Services;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertEtfNavHis : DefinedScheduleAgent
    {
        protected override async Task ExecuteJobItems()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                var context = new ScheduleContext
                {
                    StartTime = startTime,
                    ScheduleName = ScheduleName.InsertEtfNavHIS.ToString(),
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    TaskExecutionId = new ShortID(Guid.NewGuid()).ToString()
                };

                string filename = "SYSJUST-ETFNAV-HIS";
                var IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<SysjustEtfNavHis>)await etlService.ParseCsv<SysjustEtfNavHis>(filename);
                        _repository.BulkInsertToEncryptedDatabase(basic, "[Sysjust_ETFNAV_HIS]", filename, context);
                        etlService.FinishJob(filename, context);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(filename, ex.Message, IsfilePath.Key, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(filename, "找不到檔案或檔案相同不執行", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                }
            }
        }
    }
}