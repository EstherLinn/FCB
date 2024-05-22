﻿using System;
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
    public class InsertFundSize : SitecronAgentBase
    {
        private readonly ProcessRepository _repository = new ProcessRepository();
        protected override Task Execute()
        {
            string filePath = System.Configuration.ConfigurationManager.AppSettings["FundSize"];
            string tableName = "[FUND_SIZE]";

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

        private List<FundSize> ParseFileContent(string filePath)
        {
            List<FundSize> basicETF = new List<FundSize>();

            string fileContent = File.ReadAllText(filePath, Encoding.Default);
            var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<FundSize> itemsA = new FixedWidthLinesProvider<FundSize>().Parse(dataLinesA);

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