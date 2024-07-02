using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;
using System.IO;
using FixedWidthParserWriter;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertEfnd : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "EFND";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseFixedLength<Efnd>(filename);
                        _repository.BulkInsertToDatabase(basic, "[EFND]", "INVEST_FUND_NO", "DEPOSIT_DAY", "BASE_DAY", filename);
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