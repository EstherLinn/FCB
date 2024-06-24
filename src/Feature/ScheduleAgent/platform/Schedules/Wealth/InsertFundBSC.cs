using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertFundBsc : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filename = "TFJSBSC";
                bool IsfilePath = etlService.ExtractFileContainsDate(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsvContainsDate<FundBsc>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[FUND_BSC]", filename);
                        etlService.FinishJobContainsDate(filename);
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