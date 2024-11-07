using CsvHelper;
using CsvHelper.Configuration;
using Feature.Wealth.ScheduleAgent.Repositories;
using FixedWidthParserWriter;
using FluentFTP;
using FluentFTP.Helpers;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
                this.FileNameDate = this._Supplementsettings["FileNameDate"];
                DateTime parsedDate;

                if (DateTime.TryParseExact(this.FileNameDate, "yyyyMMdd'T'HHmmss'Z'", null, DateTimeStyles.AssumeUniversal, out parsedDate))
                {
                    DateTime taipeiDateTime = parsedDate.AddHours(8);
                    this.FileNameDate = taipeiDateTime.ToString("yyyyMMdd");
                    this._logger.Info($"FileNameDate: {this.FileNameDate}");
                }
                else
                {
                    this.FileNameDate = DateTime.Now.ToString("yyyyMMdd");
                    this._logger.Warn($"日期格式轉換有問題，原始值: {this._Supplementsettings["FileNameDate"]}");
                }
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
        public string FileNameDate { get; set; }

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
        /// <param name="fileName"></param>
        public void FinishJob(string fileName, DateTime startTime, string extension = "txt")
        {
            var _repository = new ProcessRepository(this._logger);

            //補檔案完成，修改後台checkbox設定，改成false
            if (this._Supplementsettings != null && this._Supplementsettings.IsChecked("Do Supplement"))
            {
                bool newValue = false;
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    this._Supplementsettings.Editing.BeginEdit();
                    this._Supplementsettings["Do Supplement"] = newValue.ToString();
                    this._Supplementsettings["FileNameDate"] = null;
                    this._Supplementsettings.Editing.EndEdit();
                }
                this._logger.Info(fileName + " 完成補檔執行");
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory(fileName, $"{fileName}排程補檔完成", string.Empty, 0, duration.TotalSeconds, "Y", Models.Sysjust.ModificationID.Done);
            }
            //帶日期的檔案改名加_done
            else if (fileName.Contains("1000"))
            {
                string localFilePath = Path.Combine(LocalDirectory, fileName);
                string doneFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_done.txt";
                string localDoneFilePath = Path.Combine(LocalDirectory, doneFileName);
                if (File.Exists(localDoneFilePath))
                {
                    File.Delete(localDoneFilePath);
                }
                File.Move(localFilePath, localDoneFilePath);
                this._logger.Info(fileName + " 執行完成");
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory(fileName, $"{fileName}排程完成", string.Empty, 0, duration.TotalSeconds, "Y", Models.Sysjust.ModificationID.Done);
            }
            //檔案資料完成後，檔案改名加_done
            else
            {
                fileName = Path.ChangeExtension(fileName, extension);
                string localFilePath = Path.Combine(LocalDirectory, fileName);
                string doneFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_done.{extension}";
                string localDoneFilePath = Path.Combine(LocalDirectory, doneFileName);
                if (File.Exists(localDoneFilePath))
                {
                    File.Delete(localDoneFilePath);
                }
                File.Move(localFilePath, localDoneFilePath);
                this._logger.Info(fileName + " 執行完成");
                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                _repository.LogChangeHistory(fileName, $"{fileName}排程完成", string.Empty, 0, duration.TotalSeconds, "Y", Models.Sysjust.ModificationID.Done);
            }
        }
        /// <summary>
        /// 檢查Ftps檔案存不存在，以及是否要做補檔
        /// </summary>
        public async Task<KeyValuePair<string, bool>> ExtractFile(string fileName, string extension = "txt")
        {
            if (this._Supplementsettings != null && this._Supplementsettings.IsChecked("Do Supplement"))
            {
                return new KeyValuePair<string, bool>("執行補檔" + this.FileNameDate, true);
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

                        localFiledonePath = Path.Combine(this.LocalDirectory, $"{fileName}_done.{extension}");
                        fileName = Path.ChangeExtension(fileName, extension);

                        //建立ftps上檔案目錄路徑
                        var filePath = Path.Combine(this.WorkingDirectory, fileName);

                        //確認檔案是否存在
                        if (!await ftpClient.FileExists(filePath))
                        {
                            this._logger.Error($"File {fileName} not found.");
                            return new KeyValuePair<string, bool>($"File {fileName} not found.", false);
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
                                    return new KeyValuePair<string, bool>("Same file content, skip download.", false);
                                }
                            }
                            this._logger.Error("Same file content, skip download.");
                            return new KeyValuePair<string, bool>("Same file content, skip download.", false);
                        }
                        //下載檔案
                        if (await ftpClient.DownloadFile(localFilePath, filePath, FtpLocalExists.Overwrite) == FtpStatus.Success)
                        {
                            return new KeyValuePair<string, bool>("從FTPS下載檔案", true);
                        }

                        this._logger.Error($"File {fileName} not found on FTPS server.");
                        return new KeyValuePair<string, bool>($"File {fileName} not found on FTPS server.", false);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error($"Error while downloading file from FTPS server: {ex.Message}", ex);
                    return new KeyValuePair<string, bool>($"Error while downloading file from FTPS server: {ex.Message}", false);
                }
            }

            return new KeyValuePair<string, bool>($"Error while ExtractFile", false);
        }


        public bool ContainsDateFormat(string input, out string extractedDate, string dateFormat = "yyMMdd")
        {
            string pattern = @"(\d{8})";

            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string dateValue = match.Groups[1].Value;

                if (dateValue.Length == 8)
                {
                    if (dateFormat == "yyMMdd")
                    {
                        extractedDate = dateValue.Substring(2);
                    }
                    else
                    {
                        extractedDate = dateValue;
                    }

                    _logger.Warn(extractedDate);
                    return true;
                }
            }

            extractedDate = null;
            return false;
        }

    }
}