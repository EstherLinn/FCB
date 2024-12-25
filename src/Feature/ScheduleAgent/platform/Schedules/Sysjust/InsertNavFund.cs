using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Services;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertNavFund : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                int threadId = Thread.CurrentThread.ManagedThreadId;

                string fileName = "SYSJUST-NAV-FUND";
                var TrafficLight = NameofTrafficLight.Sysjust_Nav_Fund;
                var scheduleName = ScheduleName.InsertNavFund.ToString();
                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var basic = await etlService.ParseCsv<SysjustNavFund>(fileName);
                        bool Ischeck = _repository.CheckDataCount(tableName, fileName, basic?.Count(), startTime, scheduleName, threadId);

                        if (!Ischeck)
                        {
                            await Bulk30datas(basic, fileName, _repository, tableName + "_Process", startTime, threadId);
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                            await Bulk30datas(basic, fileName, _repository, tableName, startTime, threadId);
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                            etlService.FinishJob(fileName, startTime, scheduleName, threadId);
                        }
                        else
                        {
                            _repository.LogChangeHistory(fileName, "資料量異常不執行匯入資料庫", string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        var executionTime = (DateTime.UtcNow - startTime).TotalSeconds;
                        _repository.LogChangeHistory(fileName, ex.Message, " ", 0, executionTime, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    var executionTime = (DateTime.UtcNow - startTime).TotalSeconds;
                    _repository.LogChangeHistory(fileName, IsfilePath.Key, " ", 0, executionTime, "N", ModificationID.Error, scheduleName, threadId);
                }
            }
        }
        private async Task Bulk30datas(IEnumerable<SysjustNavFund> basic, string filename, ProcessRepository _repository, string tableName, DateTime startTime, int threadId)
        {
            string sql = "SELECT * FROM [Sysjust_Nav_Fund] WITH (NOLOCK)";
            var scheduleName = ScheduleName.InsertNavFund.ToString();
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


            _repository.BulkInsertToNewDatabase(basic, tableName, filename, startTime, scheduleName, threadId);
            if (results != null)
            {
                _repository.BulkInsertDirectToDatabase(maxDatesQuery, tableName, filename, startTime, scheduleName, threadId);
            }
        }
    }
}