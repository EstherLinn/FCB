using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertFundsizeEtf : SitecronAgentBase
    {
        private readonly EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        public InsertFundsizeEtf()
        {
            this._etlService = new EtlService(this.Logger, this.JobItems);
        }

        protected override async Task Execute()
        {
            string filename = "SYSJUST-FUNDSIZE-ETF";
            bool IsfilePath = await this._etlService.ExtractFile(filename);

            if (IsfilePath)
            {
                try
                {
                    var basic = await this._etlService.ParseCsv<SysjustFundSizeFund>(filename);

                    _repository.BulkInsertToDatabase(basic, "[Sysjust_Fundsize_ETF]", "ScaleDate", "FirstBankCode", filename);
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