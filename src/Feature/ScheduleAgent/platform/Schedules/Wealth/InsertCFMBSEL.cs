using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using System.Linq;
using System.IO;
using FixedWidthParserWriter;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Foundation.Wealth.Manager;
using System.Data;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertCfmbsel : SitecronAgentBase
    {
        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            //CIF 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM CFMBSEL_STG";
            try
            {
                var results = await _repository.ConnectOdbc<Cfmbsel>(sql);

                foreach (var item in results)
                {
                    this.Logger.Info($"EXT_DATE: {item.EXT_DATE}" + $"CUST_ID: {item.CUST_ID}" + $"TELLER_CODE: {item.TELLER_CODE}" + $"PROMOTION_CODE: {item.PROMOTION_CODE}" + $"LOAD_DATE: {item.LOAD_DATE}");
                }

                if (!results.IsNullOrEmpty())
                {
                    //使用BulkInsert寫入sql資料庫
                    await _repository.BulkInsertFromOracle(results, "[CFMBSEL]");
                }
                else
                {
                    this.Logger.Warn("File not found");
                    _repository.LogChangeHistory(DateTime.UtcNow, sql, "沒有資料", " ", 0);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.ToString());
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0);

            }
        }
    }
}