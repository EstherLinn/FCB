using System;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Collections.Generic;
using System.Linq;

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

            List<TftfuStg> batch = new List<TftfuStg>();
            int batchSize = 1000;
            var isTrancate = false;

            try
            {
                await OdbcBulkInsert(_repository, sql, batch, batchSize, isTrancate, "[TFTFU_STG_Process]");
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                await OdbcBulkInsert(_repository,sql,batch, batchSize, isTrancate, "[TFTFU_STG]");
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"取得TFTFU_STG資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
                _repository.LogChangeHistory(DateTime.UtcNow, sql, "TFTFU_STG", " ", 0, duration.TotalSeconds, "Y");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
            }
        }

        private async Task OdbcBulkInsert(ProcessRepository _repository,string sql, List<TftfuStg> batch, int batchSize, bool isTrancate,string tableNamw)
        {
            foreach (var result in _repository.Enumerate<TftfuStg>(sql))
            {
                batch.Add(result);

                if (isTrancate == false)
                {
                    if (batch.Count > 0)
                    {
                        _repository.TrancateTable(tableNamw);
                        isTrancate = true;
                    }
                }

                if (batch.Count >= batchSize)
                {
                    await _repository.BulkInsertFromOracle(batch, tableNamw);
                    batch.Clear();
                }
            }

            if (batch.Any())
            {
                await _repository.BulkInsertFromOracle(batch, tableNamw);
            }
        }
    }
}