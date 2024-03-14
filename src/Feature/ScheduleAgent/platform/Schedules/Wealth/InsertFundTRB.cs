using Feature.Wealth.ScheduleAgent.Models.FCB;
using Feature.Wealth.ScheduleAgent.Repositories;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertFundTrb : SitecronAgentBase
    {
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                string filePath = Settings.GetSetting("FundTRB");

                if (File.Exists(filePath))
                {
                    try
                    {
                        var basic = ParseFileContent(filePath);

                        if (basic.Any())
                        {
                            ProcessRepository.BulkInsertToDatabase(basic, "[FUNDTRB]", "ProductCode", "ProductCode", filePath);
                            Console.WriteLine("資料匯入完成。");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ProcessRepository.LogChangeHistory(DateTime.Now, filePath, ex.Message, "", 0);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: File not found");
                }
            });
        }

        private List<Fundtrb> ParseFileContent(string filePath)
        {
            var basicETF = new List<Fundtrb>();

            string fileContent = File.ReadAllText(filePath, Encoding.Default);

            foreach (var basic in ChoCSVReader<Fundtrb>.LoadText(fileContent)
                         .WithDelimiter(";")
                         .Configure(c => { c.AutoDiscoverColumns = true; }))
            {
                basic.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                basicETF.Add(basic);
            }

            return basicETF;
        }
    }
}