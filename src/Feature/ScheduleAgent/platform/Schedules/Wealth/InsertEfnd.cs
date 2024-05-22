using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using FixedWidthParserWriter;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertEfnd : SitecronAgentBase
    {
        private readonly ProcessRepository _repository = new ProcessRepository();
        protected override Task Execute()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["EFND"];
            string tableName = "[EFND]";

            if (File.Exists(filePath))
            {
                try
                {
                    var basic = ParseFileContent(filePath);

                    if (basic.Any())
                    {
                        _repository.BulkInsertToNewDatabase(basic, tableName, filePath);
                    }
                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, "", 0);
                }

            }
            else
            {
                _repository.LogChangeHistory(DateTime.UtcNow, "ERROR: File not found", "找不到檔案", "", 0);
            }

            return Task.CompletedTask;
        }

        private List<Efnd> ParseFileContent(string filePath)
        {
            List<Efnd> basicETF = new List<Efnd>();

            string fileContent = File.ReadAllText(filePath, Encoding.Default);
            var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<Efnd> itemsA = new FixedWidthLinesProvider<Efnd>().Parse(dataLinesA);

            foreach (var basic in itemsA)
            {
                if (basic.DIVIDEND != null)
                {
                    basic.DIVIDEND = (decimal.Parse(basic.DIVIDEND) / 1000000000).ToString("0.000000000");
                }
                basic.UpdateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                basicETF.Add(basic);
            }

            return basicETF;
        }
    }
}