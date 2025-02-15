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
    public class InsertHoldingEtf3 : DefinedScheduleAgent
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
                    ScheduleName = ScheduleName.InsertHoldingEtf3.ToString(),
                    ThreadId = Thread.CurrentThread.ManagedThreadId,
                    TaskExecutionId = new ShortID(Guid.NewGuid()).ToString()
                };

                string fileName = "SYSJUST-HOLDING-ETF-3";
                var TrafficLight = NameofTrafficLight.Sysjust_Holding_ETF_3;
                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        //取得資料表名稱
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        //透過CsvHelper解析資料
                        var datas = await etlService.ParseCsv<SysjustHoldingEtf3>(fileName);
                        bool Ischeck = _repository.CheckDataCount(tableName, fileName, datas?.Count(), context);

                        if (!Ischeck)
                        {
                            //執行匯入temp資料表
                            _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, context);
                            //匯入完成後開起紅燈
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                            //執行匯入主資料表
                            _repository.BulkInsertToNewDatabase(datas, tableName, fileName, context);

                            var thirtyDaysAgo = DateTime.Today.AddDays(-30);
                            var thirtyDaysData = datas
                                .Where(n => DateTime.TryParse(n.Date, out var date) && date > thirtyDaysAgo)
                                .ToList();

                            //執行匯入歷史資料表(History)
                            _repository.BulkInsertToDatabaseFor30Days(thirtyDaysData, tableName + "_History", "FirstBankCode", "StockName", "Percentage", fileName, "Date", context);
                            //匯入完成後轉為綠燈
                            _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                            //完成匯入更改檔案名稱_done
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
                    _repository.LogChangeHistory(fileName, IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, context);
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