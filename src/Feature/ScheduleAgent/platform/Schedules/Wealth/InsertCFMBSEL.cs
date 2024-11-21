using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

                var data = _repository.Enumerate<Cfmbsel>(sql).ToList();
                if (data.Any())
                {
                    await ProcessData(_repository, sql, tableName + "_Process", data, startTime);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                    await ProcessData(_repository, sql, tableName, data, startTime);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                }
                else
                {
                    _repository.LogChangeHistory("CFMBSEL", "CFMBSEL No datas", "CFMBSEL", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
                    this.Logger.Error($"{sql} No datas");
                }

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory("CFMBSEL", "CFMBSEL排程完成", "CFMBSEL", 0, duration.TotalSeconds, "Y", ModificationID.Done);
                this.Logger.Info($"取得CFMBSEL資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("CFMBSEL", ex.Message, "CFMBSEL", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error);
            }
        }

        private async Task ProcessData(ProcessRepository _repository, string sql, string tableName, IEnumerable<Cfmbsel> data, DateTime startTime)
        {
            int totalInsertedCount = 0;

            try
            {
                List<Cfmbsel> batch = new List<Cfmbsel>();
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
                _repository.LogChangeHistory("CFMBSEL", sql, tableName, totalInsertedCount, (DateTime.UtcNow - startTime).TotalSeconds, "Y", ModificationID.OdbcDone);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("CFMBSEL", ex.Message, "CFMBSEL", 0, 0, "N", ModificationID.Error);
            }
        }
    }
}