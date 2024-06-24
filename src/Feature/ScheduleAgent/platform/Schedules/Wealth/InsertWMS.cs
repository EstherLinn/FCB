using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertWms : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            string filePath = Sitecore.Configuration.Settings.GetSetting("WMS");

            if (File.Exists(filePath))
            {
                try
                {
                    var basic = await ParseCsv<WmsDocRecm>(filePath);

                    if (basic.Any())
                    {
                        _repository.BulkInsertToNewDatabase(basic, "[WMS_DOC_RECM]", filePath);
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

                string filename = "WMS_DOC_RECM";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseCsv<WmsDocRecm>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[WMS_DOC_RECM]", filename);
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

        private async Task<IEnumerable<T>> ParseCsv<T>(string filePath)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            config.BadDataFound = null;

            using (var reader = new StreamReader(filePath, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }
    }
}