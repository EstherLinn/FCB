using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;
using System.IO;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertHoldingFund4 : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "SYSJUST-HOLDING-FUND-4";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustHoldingFund4>(filename);

                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Holding_Fund_4_History]", "StockName", "FirstBankCode", "Date", filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Holding_Fund_4]", filename);
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