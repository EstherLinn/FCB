﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertFundSizeFund2 : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string fileName = "SYSJUST-FUNDSIZE-FUND-2";
                var TrafficLight = NameofTrafficLight.Sysjust_Fundsize_Fund_2;
                var scheduleName = ScheduleName.InsertFundsizeFund2.ToString();
                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = await etlService.ParseCsv<SysjustFundSizeFund2>(fileName);
                        _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, startTime, scheduleName);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        _repository.BulkInsertToNewDatabase(datas, tableName, fileName, startTime, scheduleName);

                        var thirtyDaysAgo = DateTime.Today.AddDays(-30);
                        var thirtyDaysData = datas?
                            .Where(n => DateTime.TryParse(n.ScaleDate, out var date) && date > thirtyDaysAgo)
                            .ToList();

                        _repository.BulkInsertToDatabaseForHIS(thirtyDaysData, tableName + "_History", "FirstBankCode", "ScaleDate", fileName, startTime, scheduleName);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                        etlService.FinishJob(fileName, startTime, scheduleName);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.ToString(), ex);
                         _repository.LogChangeHistory(fileName, ex.Message, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName);
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(fileName,IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N",  ModificationID.Error, scheduleName);
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