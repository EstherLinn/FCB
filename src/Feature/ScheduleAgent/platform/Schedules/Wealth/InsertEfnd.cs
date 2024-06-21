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
            var _repository = new ProcessRepository(this.Logger);
            string filePath = Sitecore.Configuration.Settings.GetSetting("EFND");

            if(File.Exists(filePath))
            {
                try
                {
                    var basic = await ParseFixedLengthFullPath<Efnd>(filePath);

                    if (basic.Any())
                    {
                        _repository.BulkInsertToNewDatabase(basic, "[EFND]", filePath);
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

                string filename = "EFND";
                bool IsfilePath = etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await etlService.ParseFixedLength<Efnd>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[EFND]", filename);
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
        private async Task<IEnumerable<T>> ParseFixedLengthFullPath<T>(string filePath) where T : class, new()
        {
            using (var reader = new StreamReader(filePath, Encoding.Default))
            {
                string fileContent = await reader.ReadToEndAsync();
                var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var dataLines = new FixedWidthLinesProvider<T>().Parse(dataLinesA);
                return dataLines;
            }
        }
    }
}