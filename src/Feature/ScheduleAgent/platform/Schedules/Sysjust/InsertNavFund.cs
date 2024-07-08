using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;
using Foundation.Wealth.Manager;
using System.Data;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertNavFund : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var _repository = new ProcessRepository(this.Logger);
                var etlService = new EtlService(this.Logger, this.JobItems);

                string filename = "SYSJUST-NAV-FUND";
                bool IsfilePath = await etlService.ExtractFile(filename);

                if (IsfilePath)
                {
                    try
                    {
                        string sql = @"WITH OldestDatesCTE AS (
                                            SELECT
                                                [FirstBankCode],
                                                MIN([NetAssetValueDate]) AS OldestDate
                                            FROM
                                                [Sysjust_Nav_Fund]
                                            GROUP BY
                                                [FirstBankCode]
                                        )

                                        SELECT 
                                            [a].[FirstBankCode],
                                            [a].[NetAssetValueDate],
                                            [a].[NetAssetValue],
                                            [a].[SysjustCode]
                                        FROM 
                                            [Sysjust_Nav_Fund] AS [a]
                                        JOIN 
                                            OldestDatesCTE AS [b] ON [a].[FirstBankCode] = [b].[FirstBankCode] 
                                                                  AND [a].[NetAssetValueDate] = [b].[OldestDate];
                                        ";
                        var results = await DbManager.Custom.ExecuteIListAsync<SysjustNavFund>(sql, null, CommandType.Text);
                        var basic = await etlService.ParseCsv<SysjustNavFund>(filename);
                        _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Nav_Fund]", filename);
                        if (results != null)
                        {
                            _repository.BulkInsertDirectToDatabase(results, "[Sysjust_Nav_Fund]", "最舊日期的那筆");
                        }
                        etlService.FinishJob(filename);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex.Message, ex);
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, " ", 0);
                    }
                }
                else
                {
                    this.Logger.Error($"{filename} not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, "找不到檔案或檔案相同不執行", " ", 0);
                }
            }
        }
    }
}