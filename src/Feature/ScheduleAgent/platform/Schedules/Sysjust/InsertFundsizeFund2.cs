using CsvHelper.Configuration;
using CsvHelper;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertFundSizeFund2 : SitecronAgentBase
    {
        private readonly ProcessRepository _repository = new ProcessRepository();
        protected override Task Execute()
        {
            string endDestFolder = Settings.GetSetting("destFolder");
            string prefix = "SYSJUST-FUNDSIZE-FUND-2";
            string newnineFolder = Path.Combine(endDestFolder, "999");
            string[] files = Directory.GetFiles(newnineFolder);
            string filePath = files.FirstOrDefault(file => Path.GetFileName(file).Contains(prefix));
            string currentHash = _repository.CalculateHash(filePath);

            string preHash = string.Empty;
            string previousFilePath = Path.Combine(endDestFolder, DateTime.Today.AddDays(-1).ToString("yyyyMMdd"));
            if (Directory.Exists(previousFilePath))
            {
                string[] prefiles = Directory.GetFiles(previousFilePath);
                string prefilePath = prefiles.FirstOrDefault(file => Path.GetFileName(file).Contains(prefix));
                preHash = _repository.CalculateHash(prefilePath);
            }

            string tableName = "[Sysjust_Fundsize_Fund_2]";

            if (File.Exists(filePath))
            {
                if (currentHash.Equals(preHash))
                {
                    this._repository.LogChangeHistory(DateTime.Now, filePath, "資料相同，無執行操作。", "", 0);
                }
                else
                {
                    try
                    {
                        var funds = ParseFileContent(filePath);

                        if (funds.Any())
                        {
                            this._repository.BulkInsertToDatabase(funds, tableName, "ScaleDate", "FirstBankCode", filePath);
                        }
                        
                    }

                    catch (Exception ex)
                    {
                        this._repository.LogChangeHistory(DateTime.UtcNow, filePath, ex.Message, "", 0);
                    }
                }
            }
            else
            {
                this._repository.LogChangeHistory(DateTime.Now, "ERROR: File not found", "找不到檔案", "", 0);
            }

            return Task.CompletedTask;
        }

        private List<SysjustFundSizeFund2> ParseFileContent(string filePath)
        {
            var funds = new List<SysjustFundSizeFund2>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";@", HasHeaderRecord = false };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<Fundsize2Map>();
                funds = csv.GetRecords<SysjustFundSizeFund2>().ToList();
            }

            return funds;
        }
    }
}