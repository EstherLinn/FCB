using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertHoldingFund5 : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "SYSJUST-HOLDING-FUND-5";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustHoldingFund5>(filename);

                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Holding_Fund_5_History]", "StockCode", "FirstBankCode", "Date", filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Holding_Fund_5]", filename);
                        etlService.FinishJob(filename);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, " ", 0);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0);
                }
            }
        }
    }
}