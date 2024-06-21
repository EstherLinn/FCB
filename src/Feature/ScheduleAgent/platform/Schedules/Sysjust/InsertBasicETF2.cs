using System;
using System.Linq;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertBasicEtf2 : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filename = "SYSJUST-BASIC-ETF-2";
                bool IsfilePath = etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustBasicEtf2>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Basic_ETF_2]", filename);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Basic_ETF_2_History]", "FirstBankCode", "FirstBankCode", filename);
                        etlService.FinishJob(filename);
                    }
                    catch (Exception ex)
                    {
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, "", 0);
                    }
                }
                else
                {
                     _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0);
                }

            }

        }
    }
}