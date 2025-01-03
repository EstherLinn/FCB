using Feature.Wealth.ScheduleAgent.Models.ScheduleContext;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Repositories;
using Foundation.Wealth.Models;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertCif : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            this.Logger.Info($"Execution started at {startTime}");

            var context = new ScheduleContext
            {
                StartTime = startTime,
                ScheduleName = ScheduleName.InsertCif.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                TaskExecutionId = new ShortID(Guid.NewGuid()).ToString()
            };

            var _repository = new ProcessRepository(this.Logger);

            //CIF 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM WEA_DW_CIF_VIEW";
            var TrafficLight = NameofTrafficLight.CIF;

            try
            {
                string tableName = EnumUtil.GetEnumDescription(TrafficLight);

                var cifdata = _repository.Enumerate<Cif>(sql).ToList();
                if (cifdata.Any())
                {
                    bool Ischeck = _repository.CheckDataCount(tableName, "CIF", cifdata.Count, context);

                    if (!Ischeck)
                    {
                        await ProcessData(_repository, sql, tableName + "_Process", cifdata, context);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        await ProcessData(_repository, sql, tableName, cifdata, context);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                    }
                    else
                    {
                        _repository.LogChangeHistory("CIF", "資料量異常不執行匯入資料庫", string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                    }
                }
                else
                {
                    _repository.LogChangeHistory("CIF", "CIF No datas", "CIF", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                    this.Logger.Error($"{sql} No datas");
                }

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory("CIF", "CIF排程完成", "CIF", 0, duration.TotalSeconds, "Y", ModificationID.Done, context);
                this.Logger.Info($"ThreadId: {context.ThreadId}，取得CIF資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                _repository.LogChangeHistory("CIF", ex.Message, "CIF", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
            }
        }


        private async Task ProcessData(ProcessRepository _repository, string sql, string tableName, IEnumerable<Cif> cifdata, ScheduleContext context)
        {
            int totalInsertedCount = 0;
            try
            {
                List<Cif> batch = new List<Cif>();
                int batchSize = 10000;
                var isTrancate = false;

                foreach (var result in cifdata)
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
                _repository.LogChangeHistory("CIF", sql, tableName, totalInsertedCount, (DateTime.UtcNow - context.StartTime).TotalSeconds, "Y", ModificationID.OdbcDone, context, tableCount);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString(), ex);
                throw;
            }
        }
    }
}