using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;

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

                string filename = "SYSJUST-FUNDNAV-HIS";
                var IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<SysjustFundNavHis>)await etlService.ParseCsv<SysjustFundNavHis>(filename);
                        int batchSize = 10000;
                        var isTrancate = false;
                        for (int i = 0; i < basic.Count; i += batchSize)
                        {
                            if (!isTrancate)
                            {
                                _repository.TrancateTable("[Sysjust_FUNDNAV_HIS]");
                                isTrancate = true;
                            }
                            var batch = basic.Skip(i).Take(batchSize).ToList();
                            await _repository.BulkInsertFromOracle(batch, "[Sysjust_FUNDNAV_HIS]");
                        }
                        etlService.FinishJob(filename, startTime);

                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(filename, ex.Message, IsfilePath.Key, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(filename, "找不到檔案或檔案相同不執行", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
                }
            }
        }
    }
}