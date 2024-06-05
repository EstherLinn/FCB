using CsvHelper;
using CsvHelper.Configuration;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using FixedWidthParserWriter;
using FluentFTP;
using FluentFTP.Helpers;
using Renci.SshNet;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

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
                string parentDirectory = Path.GetDirectoryName(this.WorkingDirectory);

                // 在上一層目錄的基礎上建立目錄路徑
                this.LocalDirectory = Path.Combine(parentDirectory, this.LocalDirectory);
                this.BackUpDirectory = Path.Combine(parentDirectory, this.BackUpDirectory);

                EnsureDirectoryExists(this.LocalDirectory);
                EnsureDirectoryExists(this.BackUpDirectory);
            }
        }

        public string LocalDirectory { get; } = Settings.GetSetting("LocalDirectory");
        private string BackUpDirectory { get; } = Settings.GetSetting("BackUpDirectory");
        private string WorkingDirectory { get; }

        public bool Extract(string fileName)
        {

            if (this._settings != null)
            {
                using (var sftpClient = new SftpClient(this._settings["Ip"], this._settings.GetInteger("Port") ?? 21, this._settings["UserName"], this._settings["Password"]))
                {
                    try
                    {
                        sftpClient.Connect();

                        if (!sftpClient.IsConnected)
                        {
                            this._logger.Error("SFTP connection failed.");
                            return false;
                        }
                        sftpClient.ChangeDirectory(this._settings["WorkingDirectory"]);
                        string localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.txt");

                        fileName = Path.ChangeExtension(fileName, "txt");

                        if (!sftpClient.Exists(fileName))
                        {
                            // 文件不存在
                            return false;
                        }

                        string localFilePath = Path.Combine(this.LocalDirectory, fileName);
                        string backupFilePath = Path.Combine(this.BackUpDirectory, fileName);
                        

                        // 下载文件
                        using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            sftpClient.DownloadFile(fileName, fileStream);
                        }

                        if (File.Exists(localFilePath))
                        {
                            string localFileHash = CalculateHash(localFilePath);

                            if(File.Exists(localFiledonePath))
                            {
                                string localFiledoneHash = CalculateHash(localFiledonePath);
                                if (localFileHash.Equals(localFiledoneHash))
                                {
                                    // 文件内容相同，跳過下载
                                    return false;
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error($"Error while connecting to SFTP server: {ex.Message}", ex);
                        throw; 
                    }
                }
            }
            return false;
        }



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
                    using (var sftpClient = new SftpClient(this._settings["Ip"], this._settings.GetInteger("Port") ?? 21, this._settings["UserName"], this._settings["Password"]))
                    {
                        sftpClient.Connect();

                        if (!sftpClient.IsConnected)
                        {
                            throw new Exception("Unable to connect to SFTP server.");
                        }


                        if (!sftpClient.Exists(fileName))
                        {
                            // TODO: 無檔案可下載
                            return false;
                        }


                        // TODO: 實作組合路徑名稱
                        string localFilePath = Path.Combine(this.LocalDirectory, fileName);
                        string backupFilePath = Path.Combine(this.BackUpDirectory, fileName);

                        string currentHash = CalculateHash(localFilePath);

                        
                        //if (File.Exists(localFilePath) )
                        //{
                        //    // TODO: 相同檔案所以 skip
                        //    return false;
                        //}



                        if (sftpClient.Exists(fileName))
                        {
                            using (var file = new FileStream(localFilePath, FileMode.Create))
                            {
                                await Task.Run(() => sftpClient.DownloadFile(fileName, file));
                            }

                            File.Copy(localFilePath, backupFilePath, true);
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
        

        public async Task<IEnumerable<T>> ParseCsv<T>(string fileName)
        {
            var config = CsvConfiguration.FromAttributes<T>(CultureInfo.InvariantCulture);
            fileName = Path.ChangeExtension(fileName, "txt");
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
            filePath = Path.ChangeExtension(filePath, "txt");
            string fileContent = File.ReadAllText(filePath, Encoding.Default);
            var dataLinesA = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<T> dataLines = new FixedWidthLinesProvider<T>().Parse(dataLinesA);

            return dataLines;
        }


        /// <summary>
        /// copyDirectory 備份檔案"yyyyMMdd HH"
        /// </summary>
        private void BackupFiles(string path)
        {


        }
    }
}