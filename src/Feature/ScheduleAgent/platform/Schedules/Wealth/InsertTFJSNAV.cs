using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertTfjsNav : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                var date = DateTime.Now.ToString("yyMMdd");
                string filename = "TFJSNAV." + date + ".1000.txt";

                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<FundNavTfjsNav>(filename);
                        _repository.BulkInsertToDatabase(basic, "[FUND_NAV_TFJSNAV]", "DataDate", "BankProductCode", "NetAssetValueDate", filename);
                        etlService.FinishJob("TFJSNAV");
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