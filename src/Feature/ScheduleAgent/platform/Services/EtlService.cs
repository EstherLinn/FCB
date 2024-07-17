using CsvHelper;
using CsvHelper.Configuration;
using FixedWidthParserWriter;
using FluentFTP;
using FluentFTP.Helpers;
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
        private readonly Item _Supplementsettings;

        public EtlService(ILoggerService logger, IEnumerable<Item> jobItems)
        {
            this._logger = logger;

            if (jobItems == null)
            {
                return;
            }

            this._Supplementsettings = jobItems.FirstOrDefault(j => j.TemplateID == Templates.SupplementSetting.Id);

            if (this._Supplementsettings != null && this._Supplementsettings.IsChecked("Do Supplement"))
            {
                this.LocalDirectory = this._Supplementsettings["LocalDirectory"];
            }
            else
            {
                this._settings = jobItems.FirstOrDefault(j => j.TemplateID == Templates.FtpsSettings.Id);
                if (this._settings != null)
                {
                    SetDirectory();
                    EnsureDirectoryExists(this.LocalDirectory);
                }
            }
        }

        public string LocalDirectory { get; set; }
        private string BackUpDirectory { get; set; } = Settings.GetSetting("BackUpDirectory");
        private string WorkingDirectory { get; set; }

        /// <summary>
        /// 設定FTPs目錄
        /// </summary>
        private void SetDirectory()
        {
            // 取得工作目錄
            this.WorkingDirectory = this._settings["WorkingDirectory"].EnsurePrefix("/");

            // 取得本機檔案目錄
            if (!string.IsNullOrEmpty(this._settings["LocalDirectory"]))
            {
                this.LocalDirectory = this._settings["LocalDirectory"];
            }
            else
            {
                this.LocalDirectory = Settings.GetSetting("LocalDirectory");
            }

            string parentDirectory = Path.GetDirectoryName(this.WorkingDirectory);

            // 在上一層目錄的基礎上建立目錄路徑
            this.LocalDirectory = Path.Combine(parentDirectory, this.LocalDirectory);
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
            //CSV檔案資料完成後，檔案改名加_done
            if (filename.Equals("Fundlist") || filename.ToLower().Contains("bond"))
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
                this._logger.Info(filename + " 執行完成");
            }
            //補檔案完成，修改後台checkbox設定，改成false
            else if (this._Supplementsettings != null && this._Supplementsettings.IsChecked("Do Supplement"))
            {
                bool newValue = false;
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    this._Supplementsettings.Editing.BeginEdit();
                    this._Supplementsettings["Do Supplement"] = newValue.ToString();
                    this._Supplementsettings.Editing.EndEdit();
                }
                this._logger.Info(filename + " 完成補檔執行");
            }
            //TXT檔案資料完成後，檔案改名加_done
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
                this._logger.Info(filename + " 執行完成");
            }

        }

        /// <summary>
        /// 檢查Ftps檔案存不存在，以及是否要做補檔
        /// </summary>
        public async Task<bool> ExtractFile(string fileName)
        {
            if (this._Supplementsettings != null && this._Supplementsettings.IsChecked("Do Supplement"))
            {
                return true;
            }
            else if (this._settings != null)
            {
                try
                {
                    // 建立FTPs連線
                    using (var ftpClient = new AsyncFtpClient(this._settings["Ip"], this._settings["UserName"], this._settings["Password"], this._settings.GetInteger("Port") ?? 21))
                    {
                        await ftpClient.SetWorkingDirectory(this.WorkingDirectory);
                        await ftpClient.Connect();

                        //建立本機檔案目錄路徑
                        string localFiledonePath = "";
                        if (fileName.Equals("fundlist") || fileName.ToLower().Contains("bond"))
                        {
                            localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.csv");
                            fileName = Path.ChangeExtension(fileName, "csv");
                        }
                        else
                        {
                            localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.txt");
                            fileName = Path.ChangeExtension(fileName, "txt");
                        }

                        //建立ftps上檔案目錄路徑
                        var filePath = Path.Combine(this.WorkingDirectory, fileName);

                        //確認檔案是否存在
                        if (!await ftpClient.FileExists(filePath))
                        {
                            this._logger.Error($"File {fileName} not found.");
                            return false;
                        }
                        string localFilePath = Path.Combine(this.LocalDirectory, fileName);

                        //確認檔案是否相同
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
                        //下載檔案
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

    }
}