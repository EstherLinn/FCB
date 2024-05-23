using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertHoldingFund5 : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertHoldingFund5()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "SYSJUST-HOLDING-FUND-5";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustHoldingEtf>(filename);

                    _repository.BulkInsertToDatabase(basic, "[Sysjust_Holding_Fund_5_History]", "StockCode", "FirstBankCode", "Date", filename);
                    _repository.BulkInsertToNewDatabase(basic, "[Sysjust_Holding_Fund_5]", filename);
                }
                catch (Exception ex)
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, "", 0);
                }
            }
            else
            {
                _repository.LogChangeHistory(DateTime.UtcNow, "ERROR: File not found", "找不到檔案", "", 0);
            }
        }
    }
}