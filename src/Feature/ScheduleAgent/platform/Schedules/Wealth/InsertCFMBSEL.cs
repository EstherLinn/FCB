using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertCfmbsel : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            this.Logger.Info($"Execution started at {startTime}");

            var _repository = new ProcessRepository(this.Logger);

            //Cfmbsel 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM CFMBSEL_STG";
            var TrafficLight = NameofTrafficLight.CFMBSEL;

            try
            {
                string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                await OdbcBulkInsert(_repository, sql, tableName + "_Process", startTime);
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                await OdbcBulkInsert(_repository, sql, tableName, startTime);
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory("CFMBSEL", "CFMBSEL排程完成", "CFMBSEL", 0, duration.TotalSeconds, "Y", ModificationID.Done);
                this.Logger.Info($"取得CFMBSEL資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory(sql, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
            }
        }

        private async Task OdbcBulkInsert(ProcessRepository _repository, string sql, string tableName, DateTime startTime)
        {
            int _maxConcurrentTasks = 5;
            var _semaphore = new SemaphoreSlim(_maxConcurrentTasks);
            int _batchSize = 1000;
            bool isTruncate = false;

            try
            {
                var cfmbseldata = _repository.Enumerate<Cfmbsel>(sql).ToList();
                if (cfmbseldata.Any())
                {
                    var tasks = new List<Task>();
                    var totalCount = cfmbseldata.Count;

                    foreach (var batch in cfmbseldata.Chunk(_batchSize))
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            await _semaphore.WaitAsync();
                            try
                            {
                                if (!isTruncate && batch.Any())
                                {
                                    _repository.TrancateTable(tableName);
                                    isTruncate = true;
                                }

                                await InsertBatchAsync(_repository, batch.ToList(), tableName);
                            }
                            finally
                            {
                                _semaphore.Release();
                            }
                        }));
                    }

                    await Task.WhenAll(tasks);
                    _repository.LogChangeHistory("CFMBSEL", sql, tableName, totalCount, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.OdbcDone);
                }
                else
                {
                    this.Logger.Info($"No data found for {tableName}");
                    _repository.LogChangeHistory(tableName, $"No data found for {tableName}", tableName, 0, 0, "N", ModificationID.Error);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory(tableName, ex.Message, sql, 0, 0, "N", ModificationID.Error);
            }
        }

        private async Task InsertBatchAsync(ProcessRepository _repository, List<Cfmbsel> batch, string tableName)
        {
            int insertedCount = 0;
            try
            {
                foreach (var record in batch)
                {
                    await _repository.BulkInsertFromOracle(new List<Cfmbsel> { record }, tableName);
                    insertedCount++;
                }
                this.Logger.Info($"{tableName} 成功匯入 {insertedCount} 筆資料.");
            }
            catch (Exception ex)
            {
                this.Logger.Error($"Error {tableName}: {ex.Message}", ex);
                throw;
            }
        }
    }
}