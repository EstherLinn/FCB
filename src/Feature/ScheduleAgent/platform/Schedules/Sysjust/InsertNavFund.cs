using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Services;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

                string fileName = "SYSJUST-NAV-FUND";
                var TrafficLight = NameofTrafficLight.Sysjust_Nav_Fund;

                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var basic = await etlService.ParseCsv<SysjustNavFund>(fileName);
                        await Bulk30datas(basic, fileName, _repository, tableName + "_Process", startTime);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        await Bulk30datas(basic, fileName, _repository, tableName, startTime);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                        etlService.FinishJob(fileName, startTime);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        var executionTime = (DateTime.UtcNow - startTime).TotalSeconds;
                        _repository.LogChangeHistory(DateTime.UtcNow, fileName, ex.Message, " ", 0, executionTime, "N");
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    var executionTime = (DateTime.UtcNow - startTime).TotalSeconds;
                    _repository.LogChangeHistory(DateTime.UtcNow, fileName, IsfilePath.Key, " ", 0, executionTime, "N");
                }
            }
        }
        private async Task Bulk30datas(IEnumerable<SysjustNavFund> basic, string filename, ProcessRepository _repository,string tableName,DateTime startTime)
        {
            string sql = "SELECT * FROM [Sysjust_Nav_Fund] WITH (NOLOCK)";
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


            _repository.BulkInsertToNewDatabase(basic, tableName, filename, startTime);
            if (results != null)
            {
                _repository.BulkInsertDirectToDatabase(maxDatesQuery, tableName, "最舊日期的那筆", startTime);
            }
        }
    }
}