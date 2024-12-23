using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;
using System.Threading;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertFundNavHis : SitecronAgentBase
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

                string filename = "SYSJUST-FUNDNAV-HIS";
                var scheduleName = ScheduleName.InsertFundNavHIS.ToString();
                var IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<SysjustFundNavHis>)await etlService.ParseCsv<SysjustFundNavHis>(filename);
                        int batchSize = 10000;
                        var isTrancate = false;
                        int totalInsertedCount = 0;
                        for (int i = 0; i < basic.Count; i += batchSize)
                        {
                            if (!isTrancate)
                            {
                                _repository.TrancateTable("[Sysjust_FUNDNAV_HIS]");
                                isTrancate = true;
                            }
                            var batch = basic.Skip(i).Take(batchSize).ToList();
                            await _repository.BulkInsertFromOracle(batch, "[Sysjust_FUNDNAV_HIS]");
                            totalInsertedCount += batch.Count;
                        }

                        int tableCount = _repository.GetTableNumber("[Sysjust_FUNDNAV_HIS]");
                        _repository.LogChangeHistory(filename, "最新資料", "Sysjust_FUNDNAV_HIS", totalInsertedCount, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.最新資料, scheduleName, threadId, tableCount);
                        etlService.FinishJob(filename, startTime, scheduleName, threadId);

                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(filename, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(filename, IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
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