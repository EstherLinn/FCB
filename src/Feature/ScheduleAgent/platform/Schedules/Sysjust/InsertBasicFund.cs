using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertBasicFund : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            string filePath = Sitecore.Configuration.Settings.GetSetting("BasicFund");

            if (File.Exists(filePath))
            {
                try
                {
                    var basic = await ParseCsv<SysjustBasicFund>(filePath);

                    if (basic.Any())
                    {
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Basic_Fund]", filePath);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Basic_Fund_History]", "FirstBankCode", "FirstBankCode", filePath);
                    }
                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, " ", 0);
                }
            }

            if (this.JobItems != null && string.IsNullOrEmpty(filePath))
            {
                var jobitem = this.JobItems.FirstOrDefault();
                var etlService = new EtlService(this.Logger, jobitem);

                string filename = "SYSJUST-BASIC-FUND";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<SysjustBasicFund>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Basic_Fund]", filename);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Basic_Fund_History]", "FirstBankCode", "FirstBankCode", filename);
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