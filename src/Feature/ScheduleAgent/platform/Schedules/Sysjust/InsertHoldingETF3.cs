using System;
using System.Threading.Tasks;
using Foundation.Wealth.Models;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertHoldingEtf3 : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var startTime = DateTime.UtcNow;
                this.Logger.Info($"Execution started at {startTime}");

                var _repository = new ProcessRepository(this.Logger, this.JobItems);
                var etlService = new EtlService(this.Logger, this.JobItems);

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
                        //執行匯入temp資料表
                        _repository.BulkInsertToNewDatabase(datas, tableName + "_Process", fileName, startTime);
                        //匯入完成後開起紅燈
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Red);
                        //執行匯入主資料表
                        _repository.BulkInsertToNewDatabase(datas, tableName, fileName, startTime);
                        //執行匯入歷史資料表(History)
                        _repository.BulkInsertToDatabaseFor30Days(datas, tableName + "_History", "FirstBankCode", "StockName", "Percentage", fileName, "Date", startTime);
                        //匯入完成後轉為綠燈
                        _repository.TurnTrafficLight(TrafficLight, TrafficLightStatus.Green);
                        //完成匯入更改檔案名稱_done
                        etlService.FinishJob(fileName, startTime);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, fileName, ex.Message, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
                    }
                }
                else
                {
                    this.Logger.Error($"{fileName} not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, fileName, IsfilePath.Key, " ", 0, (DateTime.UtcNow - startTime).TotalSeconds, "N");
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