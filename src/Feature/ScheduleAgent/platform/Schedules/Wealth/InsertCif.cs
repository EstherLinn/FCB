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

namespace Feature.Wealth.ScheduleAgent.Schedules.Wealth
{
    public class InsertCif : SitecronAgentBase
    {
        private readonly ProcessRepository _repository = new();

        protected override async Task Execute()
        {
            //CIF 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM WEA_DW_CIF_VIEW";

            var results = DbManager.Cif.ExecuteIList<Cif>(sql, null, CommandType.Text);


            if (results != null && results.Any())
            {
                try
                {
                    //使用BulkInsert寫入資料庫
                    await _repository.BulkInsertFromOracle(results, "[CIF]");
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message, ex);
                    _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0);
                }
            }
            else
            {
                this.Logger.Error("ERROR: File not found");
                _repository.LogChangeHistory(DateTime.UtcNow, sql, "沒有資料", " ", 0);

            }
        }
    }
}