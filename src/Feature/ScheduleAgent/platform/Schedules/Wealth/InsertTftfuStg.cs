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
    public class InsertTftfuStg : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            this.Logger.Info($"Execution started at {startTime}");

            var _repository = new ProcessRepository(this.Logger);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            //TFTFU_STG 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM TFTFU_STG";
            var TrafficLight = NameofTrafficLight.TFTFU_STG;
            var scheduleName = ScheduleName.InsertTFTFU_STG.ToString();
            try
            {
                string tableName = EnumUtil.GetEnumDescription(TrafficLight);

                var data = _repository.Enumerate<TftfuStg>(sql).ToList();
                if (data.Any())
                {
                    bool Ischeck = _repository.CheckDataCount(tableName, "TFTFU_STG", data.Count, startTime, scheduleName, threadId);

                    if (!Ischeck)
                    {
                        await ProcessData(_repository, sql, tableName + "_Process", data, startTime, threadId);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        await ProcessData(_repository, sql, tableName, data, startTime, threadId);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                    }
                    else
                    {
                        _repository.LogChangeHistory("TFTFU_STG", "資料量異常不執行匯入資料庫", string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    _repository.LogChangeHistory("TFTFU_STG", "TFTFU_STG No datas", "TFTFU_STG", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    this.Logger.Error($"{sql} No datas");
                }

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory("TFTFU_STG", "TFTFU_STG排程完成", "TFTFU_STG", 0, duration.TotalSeconds, "Y", ModificationID.Done, scheduleName, threadId);
                this.Logger.Info($"ThreadId: {threadId}，取得TFTFU_STG資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("TFTFU_STG", ex.Message, "TFTFU_STG", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
            }
        }

        private async Task ProcessData(ProcessRepository _repository, string sql, string tableName, IEnumerable<TftfuStg> data, DateTime startTime, int threadId)
        {
            int totalInsertedCount = 0;
            var scheduleName = ScheduleName.InsertTFTFU_STG.ToString();

            try
            {
                List<TftfuStg> batch = new List<TftfuStg>();
                int batchSize = 10000;
                var isTrancate = false;

                foreach (var result in data)
                {
                    batch.Add(result);

                    if (!isTrancate)
                    {
                        if (batch.Count > 0)
                        {
                            _repository.TrancateTable(tableName);
                            isTrancate = true;
                        }
                    }

                    if (batch.Count >= batchSize)
                    {
                        totalInsertedCount += batch.Count;
                        await _repository.BulkInsertFromOracle(batch, tableName);
                        batch.Clear();
                    }
                }

                if (batch.Any())
                {
                    totalInsertedCount += batch.Count;
                    await _repository.BulkInsertFromOracle(batch, tableName);
                }
                int tableCount = _repository.GetTableNumber(tableName);
                _repository.LogChangeHistory("TFTFU_STG", sql, tableName, totalInsertedCount, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.OdbcDone, scheduleName, threadId, tableCount);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                throw;
            }
        }
    }
}