﻿using Feature.Wealth.ScheduleAgent.Models.DefinedScheduleAgent;
using Feature.Wealth.ScheduleAgent.Models.ScheduleContext;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Services;
using Foundation.Wealth.Models;
using Sitecore.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertBasicEtf2 : DefinedScheduleAgent
    {
        protected override async Task ExecuteJobItems()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                var context = new ScheduleContext
                {
                    StartTime = startTime,
                    ScheduleName = ScheduleName.InsertBasicEtf2.ToString(),
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    TaskExecutionId = new ShortID(Guid.NewGuid()).ToString()
                };

                string fileName = "SYSJUST-BASIC-ETF-2";
                var TrafficLight = NameofTrafficLight.Sysjust_Basic_ETF_2;
                var isFilePath = await etlService.ExtractFile(fileName);

                if (isFilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = await etlService.ParseCsv<SysjustBasicEtf2>(fileName);
                        bool Ischeck = _repository.CheckDataCount(tableName, fileName, datas?.Count(), context);

                        if (!Ischeck)
                        {
                            _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, context);
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                            _repository.BulkInsertToNewDatabase(datas, tableName, fileName, context);
                            _repository.BulkInsertToDatabase(datas, tableName + "_History", "FirstBankCode", "FirstBankCode", fileName, context);
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                            etlService.FinishJob(fileName, context);
                        }
                        else
                        {
                            _repository.LogChangeHistory(fileName, "資料量異常不執行匯入資料庫", string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(fileName, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(fileName, isFilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
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