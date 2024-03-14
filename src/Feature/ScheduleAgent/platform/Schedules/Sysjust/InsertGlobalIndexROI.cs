using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    internal class InsertGlobalIndexRoi : SitecronAgentBase
    {
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                string endDestFolder = ConfigurationManager.AppSettings["destFolder"];
                string prefix = "SYSJUST-GLOBALINDEX_ROI_";
                string newnineFolder = Path.Combine(endDestFolder, "999");
                string[] files = Directory.GetFiles(newnineFolder);
                string filePath = files.FirstOrDefault(file => Path.GetFileName(file).Contains(prefix));
                string currentHash = ProcessRepository.CalculateHash(filePath);

                string preHash = string.Empty;
                string previousFilePath = Path.Combine(endDestFolder, DateTime.Today.AddDays(-1).ToString("yyyyMMdd"));
                if (Directory.Exists(previousFilePath))
                {
                    string[] prefiles = Directory.GetFiles(previousFilePath);
                    string prefilePath = prefiles.FirstOrDefault(file => Path.GetFileName(file).Contains(prefix));
                    preHash = ProcessRepository.CalculateHash(prefilePath);
                }

                if (File.Exists(filePath))
                {
                    if (currentHash.Equals(preHash))
                    {
                        Console.WriteLine("資料相同，不需要執行操作。");
                        ProcessRepository.LogChangeHistory(DateTime.Now, filePath, "資料相同，無執行操作。", "", 0);
                    }
                    else
                    {
                        try
                        {
                            var basic = ParseFileContent(filePath);

                            if (basic.Any())
                            {
                                ProcessRepository.BulkInsertToDatabase(basic, "[Sysjust_GlobalIndex_ROI]", "IndexID", "IndexID", filePath);
                                Console.WriteLine("資料匯入完成。");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            ProcessRepository.LogChangeHistory(DateTime.Now, filePath, ex.Message, "", 0);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: File not found");
                }
            });
        }

        private List<SysjustGlobalIndexRoi> ParseFileContent(string filePath)
        {
            var globalindex = new List<SysjustGlobalIndexRoi>();

            foreach (var basic in ChoCSVReader<SysjustGlobalIndexRoi>.LoadText(File.ReadAllText(filePath, Encoding.Default))
                         .WithDelimiter(";@")
                         .Configure(c => c.AutoDiscoverColumns = true))
            {
                globalindex.Add(basic);
            }

            return globalindex;
        }
    }
}