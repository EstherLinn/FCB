using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.SignalStatus;
using System.Collections.Generic;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertHris : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string fileName = "HRIS";
                var TrafficLight = NameofTrafficLight.HRIS;
                var IsfilePath = await etlService.ExtractFile(fileName);
                var scheduleName = ScheduleName.InsertHRIS.ToString();
                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = (IList<Hris>)await etlService.ParseCsv<Hris>(fileName);
                        _repository.BulkInsertToEncryptedDatabase(datas, tableName + "_Process", fileName, startTime, scheduleName);
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        _repository.BulkInsertToEncryptedDatabase(datas, tableName, fileName, startTime, scheduleName);
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