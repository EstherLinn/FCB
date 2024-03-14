using Feature.Wealth.ScheduleAgent.Models.FCB;
using Feature.Wealth.ScheduleAgent.Repositories;
using FixedWidthParserWriter;
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
    public class InsertFundSize : SitecronAgentBase
    {
        protected override Task Execute()
        {
            return Task.Run(() =>
            {
                string filePath = Settings.GetSetting("FundSize");

                if (File.Exists(filePath))
                {
                    try
                    {
                        var basic = ParseFileContent(filePath);

                        if (basic.Any())
                        {
                            ProcessRepository.BulkInsertDirectToDatabase(basic, "[FUND_SIZE]", filePath);
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

        private List<FundSize> ParseFileContent(string filePath)
        {
            var basicETF = new List<FundSize>();

            string fileContent = File.ReadAllText(filePath, Encoding.Default);
            var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var itemsA = new FixedWidthLinesProvider<FundSize>().Parse(dataLinesA);

            foreach (var basic in itemsA)
            {
                if (basic.FundSizeMillionOriginalCurrency != null)
                {
                    basic.FundSizeMillionOriginalCurrency = (decimal.Parse(basic.FundSizeMillionOriginalCurrency) / 1000000).ToString();
                }

                if (basic.FundSizeMillionTWD != null)
                {
                    basic.FundSizeMillionTWD = (decimal.Parse(basic.FundSizeMillionTWD) / 1000000).ToString();
                }

                if (basic.ReferenceExchangeRate != null)
                {
                    basic.ReferenceExchangeRate = (decimal.Parse(basic.ReferenceExchangeRate) / 100000).ToString();
                }

                basicETF.Add(basic);
            }

            return basicETF;
        }
    }
}