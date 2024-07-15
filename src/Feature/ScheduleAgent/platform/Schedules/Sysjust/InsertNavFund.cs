using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;
using Foundation.Wealth.Manager;
using System.Data;
using System.Collections.Generic;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertNavFund : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "SYSJUST-NAV-FUND";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        //30日txt淨值資料
                        var basic = await etlService.ParseCsv<SysjustNavFund>(filename);

                        //資料庫抓取原資料
                        string sql = "SELECT * FROM [Sysjust_Nav_Fund]";
                        var results = await DbManager.Custom.ExecuteIListAsync<SysjustNavFund>(sql, null, CommandType.Text);

                        var oldestDatesQuery = basic.GroupBy(fund => fund.FirstBankCode).Select(group => new { FirstBankCode = group.Key, OldestDate = group.Min(fund => fund.NetAssetValueDate) });

                        var filteredResults = new List<SysjustNavFund>();

                        foreach (var oldestDate in oldestDatesQuery)
                        {
                            var fundId = oldestDate.FirstBankCode;
                            var minDate = oldestDate.OldestDate;

                            var fundResults = results.Where(fund => fund.FirstBankCode == fundId && fund.NetAssetValueDate < minDate);
                            filteredResults.AddRange(fundResults);
                        }

                        var maxDatesQuery = filteredResults
                        .GroupBy(fund => fund.FirstBankCode)
                        .Select(group => group.OrderByDescending(fund => fund.NetAssetValueDate).First());
                        

                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Nav_Fund]", filename);
                        if (results != null)
                        {
                            _repository.BulkInsertDirectToDatabase(maxDatesQuery, "[Sysjust_Nav_Fund]", "最舊日期的那筆");
                        }
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
    }
}