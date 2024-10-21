using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            //TFTFU_STG 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM TFTFU_STG";
            var TrafficLight = NameofTrafficLight.TFTFU_STG;

            try
            {
                var data = _repository.Enumerate<TftfuStg>(sql);
                if (data != null && data.Any())
                {
                    await OdbcBulkInsert(_repository, sql, "[TFTFU_STG_Process]", data);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                    await OdbcBulkInsert(_repository, sql, "[TFTFU_STG]", data);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                }
                else
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, sql, "TFTFU_STG No datas", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                    this.Logger.Error($"{sql} No datas");
                }
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"取得TFTFU_STG資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
                _repository.LogChangeHistory(sql, "TFTFU_STG", string.Empty, 0, duration.TotalSeconds, "Y", ModificationID.OdbcDone);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory(sql, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
            }
        }

        private async Task OdbcBulkInsert(ProcessRepository _repository, string sql, string tableName, IEnumerable<TftfuStg> data)
        {
            try
            {
                List<TftfuStg> batch = new List<TftfuStg>();
                int batchSize = 1000;
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
                        await _repository.BulkInsertFromOracle(batch, tableName);
                        batch.Clear();
                    }
                }

                if (batch.Any())
                {
                    await _repository.BulkInsertFromOracle(batch, tableName);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory(sql, ex.Message, string.Empty, 0, 0, "N", ModificationID.Error);
            }
        }
    }
}