﻿using System;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertBranchData : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string fileName = "BRANCH_DATA";
                var TrafficLight = NameofTrafficLight.Branch_Data;
                var scheduleName = ScheduleName.InsertBranchData.ToString();
                var IsfilePath = await etlService.ExtractFile(fileName);

                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = await etlService.ParseFixedLength<BranchData>(fileName);
                        _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, startTime, scheduleName);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        _repository.BulkInsertToNewDatabase(datas, tableName, fileName, startTime, scheduleName);
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
                    _repository.LogChangeHistory(fileName,IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName);
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