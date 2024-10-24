using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;
using System.IO;
using FixedWidthParserWriter;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Foundation.Wealth.Manager;
using System.Data;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Foundation.Wealth.Models;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertCif : SitecronAgentBase
    {

        protected override async Task Execute()
        {
            var startTime = DateTime.UtcNow;
            this.Logger.Info($"Execution started at {startTime}");

            var _repository = new ProcessRepository(this.Logger);

            //CIF 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM WEA_DW_CIF_VIEW";
            var TrafficLight = NameofTrafficLight.CIF;

            try
            {
                var cifdata = _repository.Enumerate<Cif>(sql);
                if (cifdata != null && cifdata.Any())
                {
                    await ProcessData(_repository, sql, "[CIF_Process]", cifdata);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                    await ProcessData(_repository, sql, "[CIF]", cifdata);
                    _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                }
                else
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, sql, "CIF No datas", " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                    this.Logger.Error($"{sql} No datas");
                }
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"取得CIF資料完成：Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
                _repository.LogChangeHistory(DateTime.UtcNow, sql, "CIF", " ", 0, duration.TotalSeconds, "Y");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
            }
        }


        private async Task ProcessData(ProcessRepository _repository, string sql, string tableName, IEnumerable<Cif> cifdata)
        {
            try
            {
                List<Cif> batch = new List<Cif>();
                int batchSize = 1000;
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