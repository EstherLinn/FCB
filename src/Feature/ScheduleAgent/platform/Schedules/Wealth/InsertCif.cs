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
    public class InsertCif : SitecronAgentBase
    {

        protected override async Task Execute()
        {
            var _repository = new ProcessRepository(this.Logger);

            //CIF 一次性排程 去連線orcale 資料庫查詢之後結果放物件再塞回去sql，使用bulkInsert
            string sql = "SELECT * FROM WEA_ODS_CIF_VIEW";
            try
            {
                var results = await _repository.ConnectOdbc<Cif>(sql);

                foreach (var item in results)
                {
                    this.Logger.Info($"CIF_ID: {item.CIF_ID}" + $"CIF_CUST_NAME: {item.CIF_CUST_NAME}" + $"CIF_ESTABL_BIRTH_DATE: {item.CIF_ESTABL_BIRTH_DATE}" + $"CIF_CUST_ATTR: {item.CIF_CUST_ATTR}" + $"CIF_TEL_NO1: {item.CIF_TEL_NO1}"
                        + $"CIF_TEL_NO3: {item.CIF_TEL_NO3}" + $"CIF_E_MAIL_ADDRESS: {item.CIF_E_MAIL_ADDRESS}" + $"CIF_CHN_BU: {item.CIF_CHN_BU}" + $"CIF_CHN_CR: {item.CIF_CHN_CR}"
                        + $"CIF_AO_EMPNO: {item.CIF_AO_EMPNO}" + $"CIF_MAIN_BRANCH: {item.CIF_MAIN_BRANCH}" + $"CIF_EMP_RISK: {item.CIF_EMP_RISK}" + $"CIF_EMP_PI_RISK_ATTR: {item.CIF_EMP_PI_RISK_ATTR}"
                        + $"KYC_EXPIR_DATE: {item.CIF_KYC_EXPIR_DATE}" + $"CIF_VIP_CODE: {item.CIF_VIP_CODE}" + $"CIF_RECCONSENT_TYPE: {item.CIF_RECCONSENT_TYPE}" + $"CIF_UNHEALTH_TYPE: {item.CIF_UNHEALTH_TYPE}"
                        + $"CIF_SAL_FLAG: {item.CIF_SAL_FLAG}" + $"CIF_HIGH_ASSET_FLAG: {item.CIF_HIGH_ASSET_FLAG}"
                        + $"CIF_EXT_DATE: {item.CIF_EXT_DATE}");
                }

                if (results.Any())
                {
                    //使用BulkInsert寫入資料庫
                    await _repository.BulkInsertFromOracle(results, "[CIF]");
                }
                else
                {
                    this.Logger.Warn($"{sql} no data.");
                    _repository.LogChangeHistory(DateTime.UtcNow, sql, "沒有資料", " ", 0);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message, ex);
                _repository.LogChangeHistory(DateTime.UtcNow, sql, ex.Message, " ", 0);
            }
        }
    }
}