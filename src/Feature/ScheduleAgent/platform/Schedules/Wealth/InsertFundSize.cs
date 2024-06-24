using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Services;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertFundSize : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filePath = "FUND_SIZE";
                string tableName = "[FUND_SIZE]";

                bool IsfilePath = await etlService.ExtractFile("FUND_SIZE");

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseFixedLength<FundSize>(filePath);
                        _repository.BulkInsertToNewDatabase(basic, tableName, filePath);

                        etlService.FinishJob(filePath);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, " ", 0);
                    }
                }
                else
                {
                    this.Logger.Error("ERROR: File not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filePath, "找不到檔案", " ", 0);
                }
            }
        }
    }
}