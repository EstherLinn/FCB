﻿using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Collections.Generic;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Threading;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertBondHistoryPrice : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                var threadId = Thread.CurrentThread.ManagedThreadId;

                var scheduleName = ScheduleName.InsertBondHistoryPrice.ToString();
                string fileName = "BondHistoryPrice";
                var IsfilePath = await etlService.ExtractFile(fileName, "csv");

                if (IsfilePath.Value)
                {
                    try
                    {
                        var basic = (IList<BondHistoryPrice>)await etlService.ParseCsvNotTXT<BondHistoryPrice>(fileName);
                        bool Ischeck = _repository.CheckDataCount("BondHistoryPrice", fileName, basic?.Count, startTime, scheduleName, threadId);

                        if (!Ischeck)
                        {
                            _repository.BulkInsertToEncryptedDatabase(basic, "BondHistoryPrice", fileName, startTime, scheduleName, threadId);
                            etlService.FinishJob(fileName, startTime, scheduleName, threadId, "csv");
                        }
                        else
                        {
                            _repository.LogChangeHistory(fileName, "資料量異常不執行匯入資料庫", string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                        _repository.LogChangeHistory(fileName, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(fileName, IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
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