using CsvHelper;
using CsvHelper.Configuration;
using FixedWidthParserWriter;
using FluentFTP;
using FluentFTP.Helpers;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

                if (!string.IsNullOrEmpty(settings["LocalDirectory"]))
                {
                    this.LocalDirectory = settings["LocalDirectory"];
                }
                else
                {
                    this.LocalDirectory = Settings.GetSetting("LocalDirectory");
                }

                string parentDirectory = Path.GetDirectoryName(this.WorkingDirectory);

                // 在上一層目錄的基礎上建立目錄路徑
                this.LocalDirectory = Path.Combine(parentDirectory, this.LocalDirectory);
                this.BackUpDirectory = Path.Combine(parentDirectory, this.BackUpDirectory);

                EnsureDirectoryExists(this.LocalDirectory);
                EnsureDirectoryExists(this.BackUpDirectory);
            }
        }

        public string LocalDirectory { get; }
        private string BackUpDirectory { get; } = Settings.GetSetting("BackUpDirectory");
        private string WorkingDirectory { get; }

        /// <summary>
        /// 檢查預設資料夾是否存在
        /// </summary>
        private void EnsureDirectoryExists(string path)
        {
            try
            {
                if (Directory.Exists(FileUtil.MapPath(path)))
                {
                    return;
                }
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

        /// <summary>
        /// 檢查檔案是否一樣Hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string CalculateHash(string archiveFilePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(File.ReadAllBytes(archiveFilePath));
                return BitConverter.ToString(hashBytes);
            }
        }

        /// <summary>
        /// TXT 檔案解析，分隔符號解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseCsv<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            config.BadDataFound = null;
            fileName = Path.ChangeExtension(fileName, "txt");
            string localFilePath = Path.Combine(this.LocalDirectory, fileName);

            using (var reader = new StreamReader(localFilePath, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }

        /// <summary>
        /// TXT 檔案解析含日期的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseCsvContainsDate<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            config.BadDataFound = null;

            string[] files = Directory.GetFiles(this.LocalDirectory)
                          .Where(f => f.Contains(fileName))
                          .ToArray();
            string localFiledonePath = files.FirstOrDefault();

            using (var reader = new StreamReader(localFiledonePath, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }

        /// <summary>
        /// TXT 檔案解析，固定長度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseFixedLength<T>(string filePath) where T : class, new()
        {
            filePath = Path.ChangeExtension(filePath, "txt");
            string localFilePath = Path.Combine(this.LocalDirectory, filePath);

            using (var reader = new StreamReader(localFilePath, Encoding.Default))
            {
                string fileContent = await reader.ReadToEndAsync();
                var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var dataLines = new FixedWidthLinesProvider<T>().Parse(dataLinesA);
                return dataLines;
            }
        }

        /// <summary>
        /// CSV 檔案解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseCsvNotTXT<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            config.BadDataFound = null;
            fileName = Path.ChangeExtension(fileName, "csv");
            string localFilePath = Path.Combine(this.LocalDirectory, fileName);

            using (var reader = new StreamReader(localFilePath, Encoding.Default))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecordsAsync<T>().ToListAsync();
                return await records;
            }
        }

        /// <summary>
        /// copyDirectory 備份檔案"yyyyMMdd HH"
        /// </summary>
        private void BackupFiles(string path)
        {


        }

        /// <summary>
        /// 完成資料插入後，檔案改名加_done
        /// </summary>
        /// <param name="filename"></param>
        public void FinishJob(string filename)
        {
            if (filename.Equals("Fundlist"))
            {
                filename = Path.ChangeExtension(filename, "csv");
                string localFilePath = Path.Combine(LocalDirectory, filename);
                string doneFileName = $"{Path.GetFileNameWithoutExtension(filename)}_done.csv";
                string localDoneFilePath = Path.Combine(LocalDirectory, doneFileName);
                if (File.Exists(localDoneFilePath))
                {
                    File.Delete(localDoneFilePath);
                }
                File.Move(localFilePath, localDoneFilePath);
            }
            else
            {
                filename = Path.ChangeExtension(filename, "txt");
                string localFilePath = Path.Combine(LocalDirectory, filename);
                string doneFileName = $"{Path.GetFileNameWithoutExtension(filename)}_done.txt";
                string localDoneFilePath = Path.Combine(LocalDirectory, doneFileName);
                if (File.Exists(localDoneFilePath))
                {
                    File.Delete(localDoneFilePath);
                }
                File.Move(localFilePath, localDoneFilePath);
            }

        }

        /// <summary>
        /// 完成資料插入後，檔案改名加_done，包含日期的
        /// </summary>
        /// <param name="filename"></param>
        public void FinishJobContainsDate(string filename)
        {
            string[] files = Directory.GetFiles(this.LocalDirectory)
                          .Where(f => f.Contains(filename))
                          .ToArray();
            string localFiledonePath = files.FirstOrDefault();

            string doneFileName = $"{Path.GetFileNameWithoutExtension(filename)}_done.txt";
            string localDoneFilePath = Path.Combine(LocalDirectory, doneFileName);
            if (File.Exists(localDoneFilePath))
            {
                File.Delete(localDoneFilePath);
            }
            File.Move(localFiledonePath, localDoneFilePath);
        }


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

                        string localFiledonePath = "";
                        if (fileName.Equals("Fundlist"))
                        {
                            localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.csv");
                            fileName = Path.ChangeExtension(fileName, "csv");
                        }
                        else
                        {
                            localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.txt");
                            fileName = Path.ChangeExtension(fileName, "txt");
                        }

                        var filePath = Path.Combine(this.WorkingDirectory, fileName);

                        if (!await ftpClient.FileExists(filePath))
                        {
                            this._logger.Error($"File {fileName} not found.");
                            return false;
                        }
                        string localFilePath = Path.Combine(this.LocalDirectory, fileName);
                        string backupFilePath = Path.Combine(this.BackUpDirectory, fileName);

                        if (File.Exists(localFilePath) && await ftpClient.CompareFile(localFilePath, filePath, FtpCompareOption.Checksum) == FtpCompareResult.Equal)
                        {
                            string localFileHash = CalculateHash(localFilePath);

                            if (File.Exists(localFiledonePath))
                            {
                                string localFiledoneHash = CalculateHash(localFiledonePath);
                                if (localFileHash.Equals(localFiledoneHash))
                                {
                                    this._logger.Error("Same file content, skip download.");
                                    return false;
                                }
                            }
                            this._logger.Error("Same file content, skip download.");
                            return false;
                        }

                        if (await ftpClient.DownloadFile(localFilePath, filePath, FtpLocalExists.Overwrite) == FtpStatus.Success)
                        {
                            return true;
                        }

                        this._logger.Error($"File {fileName} not found on FTPS server.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error($"Error while downloading file from FTPS server: {ex.Message}", ex);
                }
            }

            return false;
        }

        public bool ExtractFileContainsDate(string fileName)
        {
            if (this._settings != null)
            {
                try
                {
                    using (var ftpClient = new FtpClient(this._settings["Ip"], this._settings["UserName"], this._settings["Password"], this._settings.GetInteger("Port") ?? 21))
                    {
                        ftpClient.SetWorkingDirectory(this.WorkingDirectory);
                        ftpClient.Connect();

                        var filePath = Path.Combine(this.WorkingDirectory, fileName);
                        string localFiledonePath = Directory.GetFiles(this.LocalDirectory, $"*{fileName}_done.txt").FirstOrDefault();

                        var files = ftpClient.GetListing(ftpClient.GetWorkingDirectory())
                                     .Where(file => file.Name.Contains(fileName))
                                     .OrderByDescending(file => file.Modified)
                                     .ToList();

                        if (files.Count == 0)
                        {
                            this._logger.Error($"No file exists.");
                            return false;
                        }
                        if (!ftpClient.FileExists(filePath))
                        {
                            this._logger.Error($"File {fileName} not found.");
                            return false;
                        }
                        var latestFile = files[0];

                        string localFilePath = Path.Combine(this.LocalDirectory, latestFile.Name);
                        localFilePath = Path.ChangeExtension(localFilePath, "txt");

                        ftpClient.DownloadFile(localFilePath, filePath, FtpLocalExists.Overwrite);

                        if (File.Exists(localFilePath))
                        {
                            string localFileHash = CalculateHash(localFilePath);
                            if (File.Exists(localFiledonePath))
                            {
                                string localFiledoneHash = CalculateHash(localFiledonePath);
                                if (localFileHash.Equals(localFiledoneHash))
                                {
                                    this._logger.Error("Same file content, skip download.");
                                    return false;
                                }
                            }
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error($"Error while connecting to SFTP server: {ex.Message}", ex);
                }
            }

            return false;
        }
    }
}