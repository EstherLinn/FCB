using CsvHelper;
using CsvHelper.Configuration;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using FixedWidthParserWriter;
using FluentFTP;
using FluentFTP.Helpers;
using Sitecore.Data.Items;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Services
{
    public class EtlService
    {
        private readonly ILoggerService _logger;
        private readonly Item _settings;

        public EtlService(ILoggerService logger, Item settings)
        {
            this._logger = logger;
            this._settings = settings;

            if (settings != null)
            {
                this.WorkingDirectory = settings["WorkingDirectory"].EnsurePrefix("/");
                this.LocalDirectory = Path.ChangeExtension(this.WorkingDirectory, this.LocalDirectory);
                this.BackUpDirectory = Path.ChangeExtension(this.WorkingDirectory, this.BackUpDirectory);

                EnsureDirectoryExists(this.LocalDirectory);
                EnsureDirectoryExists(this.BackUpDirectory);
            }
        }

        private string LocalDirectory { get; } = @"/temp";
        private string BackUpDirectory { get; } = @"/App_Data/packages";
        private string WorkingDirectory { get; }

        /// <summary>
        /// 提取檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> ExtractFile(string fileName)
        {
            if (this._settings != null)
            {
                try
                {
                    using (var ftpClient = new AsyncFtpClient(this._settings["Ip"], this._settings["UserName"], this._settings["Password"], this._settings.GetInteger("Port") ?? 21))
                    {
                        await ftpClient.SetWorkingDirectory(this.WorkingDirectory);
                        await ftpClient.Connect();
                        if (!await ftpClient.FileExists(fileName))
                        {
                            // TODO: 無檔案可下載
                            return false;
                        }

                        // TODO: 實作組合路徑名稱
                        string localFilePath = Path.Combine(this.LocalDirectory, fileName);
                        if (File.Exists(localFilePath) && await ftpClient.CompareFile(localFilePath, fileName, FtpCompareOption.Checksum) == FtpCompareResult.Equal)
                        {
                            // TODO: 相同檔案所以 skip
                            return false;
                        }

                        if (await ftpClient.DownloadFile(localFilePath, fileName, FtpLocalExists.Overwrite) == FtpStatus.Success)
                        {

                            return true;
                        }

                        // TODO: 下載失敗
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // TODO: 發生錯誤
                }
            }

            return false;
        }

        /// <summary>
        /// 檢查預設資料夾是否存在
        /// </summary>
        private void EnsureDirectoryExists(string path)
        {
            if (Directory.Exists(FileUtil.MapPath(path)))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(FileUtil.MapPath(path));
            }
            catch (IOException ex)
            {
                this._logger.Error($"Error : {ex}", ex);
            }
            catch (Exception ex)
            {
                this._logger.Error($"Error : {ex}", ex);
            }
        }

        public async Task<IEnumerable<T>> ParseCsv<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            string localFilePath = Path.Combine(this.LocalDirectory, fileName);

            using (var reader = new StreamReader(localFilePath, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }

        public IEnumerable<T> ParseFixedLength<T>(string filePath) where T : class, new()
        {
            string fileContent = File.ReadAllText(filePath, Encoding.Default);
            var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<T> dataLines = new FixedWidthLinesProvider<T>().Parse(dataLinesA);

            return dataLines;
        }


        ///copyDirectory 備份檔案"yyyyMMdd HH"

    }
}