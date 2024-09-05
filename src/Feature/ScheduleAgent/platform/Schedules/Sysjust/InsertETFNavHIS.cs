using System;
using System.Linq;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Collections.Generic;

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

                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "SYSJUST-ETFNAV-HIS";
                var IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<SysjustEtfNavHis>)await etlService.ParseCsv<SysjustEtfNavHis>(filename);
                        _repository.BulkInsertToEncryptedDatabase(basic, "[Sysjust_ETFNAV_HIS]", filename, startTime);
                        etlService.FinishJob(filename, startTime);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, IsfilePath.Key, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                }
            }
        }
    }
}