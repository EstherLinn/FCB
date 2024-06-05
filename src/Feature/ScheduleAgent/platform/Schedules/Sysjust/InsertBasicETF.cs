using System;
using System.Threading.Tasks;
using Feature.Wealth.ScheduleAgent.Services;
using Xcms.Sitecore.Foundation.QuartzSchedule;
using Feature.Wealth.ScheduleAgent.Repositories;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using System.Linq;
using Quartz;
using System.IO;

namespace Feature.Wealth.ScheduleAgent.Schedules.Sysjust
{
    public class InsertBasicEtf : SitecronAgentBase
    {
        private EtlService _etlService;
        private readonly ProcessRepository _repository = new();

        protected override async Task Execute()
        {
            if (this.JobItems != null)
            {
                var jobitem = this.JobItems.FirstOrDefault();
                this._etlService = new EtlService(this.Logger, jobitem);

                string filename = "SYSJUST-BASIC-ETF";
                bool IsfilePath = this._etlService.Extract(filename);

                if (IsfilePath)
                {
                    try
                    {
                        var basic = await this._etlService.ParseCsv<SysjustBasicEtf>(filename);
                        _repository.BulkInsertToDatabase(basic, "[Sysjust_Basic_ETF]", "FirstBankCode", "FirstBankCode", filename);

                        filename = Path.ChangeExtension(filename, "txt");
                        string localFilePath = Path.Combine(this._etlService.LocalDirectory, filename);
                        string doneFileName = $"{Path.GetFileNameWithoutExtension(filename)}_done.txt";
                        string localDoneFilePath = Path.Combine(this._etlService.LocalDirectory, doneFileName);
                        File.Move(localFilePath, localDoneFilePath);
                    }
                    catch (Exception ex)
                    {
                        _repository.LogChangeHistory(DateTime.UtcNow, filename, ex.Message, " ", 0);
                    }
                }
                else
                {
                    _repository.LogChangeHistory(DateTime.UtcNow, "ERROR: File not found", "找不到檔案", " ", 0);
                }
            }
        }
    }
}