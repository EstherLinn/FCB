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
            var _repository = new ProcessRepository(this.Logger);

            string filePath = Sitecore.Configuration.Settings.GetSetting("HoldingFund4");

            if (File.Exists(filePath))
            {
                try
                {
                    var basic = await ParseCsv<SysjustHoldingFund4>(filePath);

                    if (basic.Any())
                    {
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Holding_Fund_4_History]", "StockName", "FirstBankCode", "Date", filePath);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Holding_Fund_4]", filePath);
                    }
                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, " ", 0);
                }
            }

            if (this.JobItems != null && filePath == null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filename = "SYSJUST-HOLDING-FUND-4";
                bool IsfilePath = etlService.ExtractFile(filename);

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
                    this.Logger.Error("ERROR: File not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0);
                }
            }
        }

        public async Task<IEnumerable<T>> ParseCsv<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            config.BadDataFound = null;

            using (var reader = new StreamReader(fileName, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }
    }
}