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
    public class InsertCfmbsel : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            this.Logger.Info($"Execution started at {startTime}");

            var _repository = new ProcessRepository(this.Logger);

            //Cfmbsel 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM CFMBSEL_STG";

            List<Cfmbsel> batch = new List<Cfmbsel>();
            int batchSize = 1000;
            var isTrancate = false;
            var TrafficLight = NameofTrafficLight.CFMBSEL;

            try
            {
                await OdbcBulkInsert(_repository, sql, batch, batchSize, isTrancate, "[CFMBSEL_Process]");
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                await OdbcBulkInsert(_repository, sql, batch, batchSize, isTrancate, "[CFMBSEL]");
                _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"取得CFMBSEL資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
                _repository.LogChangeHistory(DateTime.UtcNow, sql, "CFMBSEL", " ", 0, duration.TotalSeconds, "Y");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
            }


        }

        private async Task OdbcBulkInsert(ProcessRepository _repository, string sql, List<Cfmbsel> batch, int batchSize, bool isTrancate, string tableName)
        {
            try
            {
                foreach (var result in _repository.Enumerate<Cfmbsel>(sql))
                {
                    batch.Add(result);

                    if (isTrancate == false)
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
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0, 0, "N");
            }
        }
    }
}