using System;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Threading;
using System.Linq;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertBasicEtf : SitecronAgentBase
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

                string fileName = "SYSJUST-BASIC-ETF";
                var TrafficLight = NameofTrafficLight.Sysjust_Basic_ETF;
                var scheduleName = ScheduleName.InsertBasicEtf.ToString();
                var isFilePath = await etlService.ExtractFile(fileName);

                if (isFilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = await etlService.ParseCsv<SysjustBasicEtf>(fileName);
                        var count = datas?.Count();
                        var Ischeck = _repository.CheckDataCount(tableName, fileName, count, startTime, scheduleName, threadId);

                        if (!Ischeck)
                        {
                            _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, startTime, scheduleName, threadId);
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                            _repository.BulkInsertToNewDatabase(datas, tableName, fileName, startTime, scheduleName, threadId);
                            _repository.BulkInsertToDatabase(datas, tableName + "_History", "FirstBankCode", "FirstBankCode", fileName, startTime, scheduleName, threadId);
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
                        _repository.LogChangeHistory(fileName, ex.Message, string.Empty, 0, executionTime, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    var executionTime = (DateTime.UtcNow - startTime).TotalSeconds;
                    _repository.LogChangeHistory(fileName, isFilePath.Key, string.Empty, 0, executionTime, "N", ModificationID.Error, scheduleName, threadId);
                }
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                this.Logger.Info($"Execution finished at {endTime}. Total duration: {duration.TotalSeconds} seconds.");
            }
            else
            {
                this.Logger.Warn($"Not Setting Any JobItems");
            }
        }
    }
}