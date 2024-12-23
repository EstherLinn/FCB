using System;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Threading;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertTfjeNav : SitecronAgentBase
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

                var date = DateTime.Now.ToString("yyMMdd");
                string fileName = "TFJENAV." + date + ".1000.txt";
                var TrafficLight = NameofTrafficLight.ETF_NAV_TFJENAV;
                var filedate = etlService.GetFileDate(fileName);
                var scheduleName = ScheduleName.InsertETF_NAV_TFJENAV.ToString();
                if (etlService.ContainsDateFormat(filedate, out string extractedDate))
                {
                    fileName = "TFJENAV." + extractedDate + ".1000.txt";
                }

                var IsfilePath = await etlService.ExtractFile(fileName);
                
                if (IsfilePath.Value)
                {
                    try
                    {
                        string tableName = EnumUtil.GetEnumDescription(TrafficLight);
                        var datas = await etlService.ParseCsv<EtfNavTfjeNav>(fileName);
                        _repository.BulkInsertToDatabase(datas, tableName, "BankProductCode", "NetAssetValueDate", "DataDate", fileName, startTime , scheduleName, threadId);
                        etlService.FinishJob(fileName, startTime, scheduleName, threadId);
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
                    _repository.LogChangeHistory(fileName,IsfilePath.Key, string.Empty, 0, (DateTime.UtcNow - startTime).TotalSeconds, "N", ModificationID.Error, scheduleName, threadId);
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