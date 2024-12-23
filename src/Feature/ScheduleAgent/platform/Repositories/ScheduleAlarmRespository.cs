﻿using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Foundation.Wealth.Manager;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class ScheduleAlarmRespository
    {
        private readonly ILoggerService _logger;
        private readonly string _200done = ((int)ModificationID.Done).ToString();
        private readonly string _102 = ((int)ModificationID.OdbcDone).ToString();
        private readonly string _100 = ((int)ModificationID.最新資料).ToString();
        private readonly string _101 = ((int)ModificationID.資料差異更新).ToString();
        private readonly string _103 = ((int)ModificationID.最舊日期的那筆).ToString();

        public ScheduleAlarmRespository(ILoggerService logger)
        {
            this._logger = logger;
        }

        public List<ChangeHistory> GetChangeHistory()
        {
            string sql = @"SELECT *
                           FROM [ChangeHistory] WITH (NOLOCK)
                           WHERE ModificationDate >= DATEADD(HOUR, -1, GETUTCDATE())
                           ORDER BY [ModificationDate] DESC
                          ";

            var result = DbManager.Custom.ExecuteIList<ChangeHistory>(sql, null, commandType: CommandType.Text)?.ToList() ?? new List<ChangeHistory>();
            return result;
        }

        public void SendMail(IList<ChangeHistory> results, Item settings)
        {
            try
            {
                if (results == null || !results.Any())
                {
                    _logger.Info("資料庫查無資訊");
                    return;
                }

                bool hasFailure = results.Any(i => i.Success == "N");
                var mailServerOption = new ScheduleMailServerOption(settings);
                string mailBody = BuildMailBody(results, mailServerOption, hasFailure);

                if (string.IsNullOrEmpty(mailBody))
                {
                    _logger.Info("沒資料");
                    return;
                }

                SendEmail(mailBody, mailServerOption, hasFailure);
            }
            catch (Exception ex)
            {
                _logger.Error("發送郵件Error: " + ex.Message);
            }
        }

        private string BuildMailBody(IList<ChangeHistory> results, ScheduleMailServerOption mailServerOption, bool hasFailure)
        {
            var modificationLines = results
                .Where(i => i.Success == "Y" && (i.ModificationID == this._100 || i.ModificationID == this._101 || i.ModificationID == this._102))
                .GroupBy(i => Path.GetFileNameWithoutExtension(i.FileName))
                .ToDictionary(g => g.Key, g => g.ToList());

            var successData = GetSuccessData(results, modificationLines);
            var successDataTable = ConvertToDataTable(successData);

            var runningTasks = results
                .Where(i => i.Success == "Y" && i.ModificationID != this._200done
                            && !results.Any(s => Path.GetFileNameWithoutExtension(s.FileName) == Path.GetFileNameWithoutExtension(i.FileName)
                            && (s.ModificationID == this._200done && s.ThreadId == i.ThreadId))).ToList();

            var mailBody = new StringBuilder();

            if (hasFailure)
            {
                var failData = results.Where(i => i.Success == "N").ToList();
                var failDataTable = ConvertToDataTable(failData);
                mailBody.Append(BuildHtmlBody(failDataTable, mailServerOption.FailedTitle, failData.Count));
            }

            if (successData != null && successData.Count > 0)
            {
                mailBody.Append(BuildHtmlBody(successDataTable, mailServerOption.SuccessTitle, successData.Count));
            }

            if (runningTasks.Count > 0)
            {
                var tasksDataTable = ConvertToDataTable(runningTasks);
                mailBody.Append(BuildHtmlBody(tasksDataTable, "未完成的排程(尚在執行中)", runningTasks.Count, true));
            }

            return mailBody.ToString();
        }

        private void SendEmail(string mailBody, ScheduleMailServerOption mailServerOption, bool hasFailure)
        {
            using (var client = mailServerOption.ToSMTPClient())
            {
                var encoding = Encoding.UTF8;
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailServerOption.From, string.IsNullOrEmpty(mailServerOption.UserName) ? "第e理財網" : mailServerOption.UserName);
                    mail.Subject = hasFailure ? mailServerOption.FailedSubject : mailServerOption.SuccessSubject;
                    mail.IsBodyHtml = true;
                    mail.Body = mailBody + (hasFailure ? mailServerOption.FailedContent : mailServerOption.SuccessContent);
                    mail.HeadersEncoding = encoding;
                    mail.BodyEncoding = encoding;
                    mail.SubjectEncoding = encoding;

                    string[] emailAddresses = hasFailure ? mailServerOption.FailedTo : mailServerOption.SuccessTo;

                    if (emailAddresses != null && emailAddresses.Length > 0)
                    {
                        foreach (var email in emailAddresses)
                        {
                            if (!string.IsNullOrEmpty(email))
                            {
                                mail.To.Add(email.Trim());
                            }
                        }
                    }

                    try
                    {
                        client.Send(mail);
                        _logger.Info("郵件發送成功！");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("郵件發送失敗: " + ex.ToString());
                    }
                }
            }
        }


        private List<ChangeHistory> GetSuccessData(IList<ChangeHistory> results, Dictionary<string, List<ChangeHistory>> modificationLines)
        {
            var successData = results.Where(i => i.Success == "Y" && i.ModificationID == this._200done).ToList();

            var filteredSuccessData = new List<ChangeHistory>();

            var threadDataTableMapping = new Dictionary<int, Dictionary<string, List<string>>>();
            var threadDataCountMapping = new Dictionary<int, Dictionary<string, List<string>>>();

            var modificationID103Records = results.Where(i => i.Success == "Y" && i.ModificationID == this._103).ToList();

            foreach (var record in successData)
            {
                var matchingRecords = results
                    .Where(i => i.Success == "Y" &&
                                (i.ModificationID == this._100 || i.ModificationID == this._101 || i.ModificationID == this._102) &&
                                i.ThreadId == record.ThreadId && i.FileName == Path.GetFileNameWithoutExtension(record.FileName))
                    .ToList();

                if (!threadDataTableMapping.ContainsKey(record.ThreadId))
                {
                    threadDataTableMapping[record.ThreadId] = new Dictionary<string, List<string>>();
                }
                if (!threadDataCountMapping.ContainsKey(record.ThreadId))
                {
                    threadDataCountMapping[record.ThreadId] = new Dictionary<string, List<string>>();
                }

                foreach (var matchingRecord in matchingRecords)
                {
                    var childKey = Path.GetFileNameWithoutExtension(matchingRecord.FileName);

                    if (!threadDataTableMapping[record.ThreadId].ContainsKey(childKey))
                    {
                        threadDataTableMapping[record.ThreadId][childKey] = new List<string>();
                    }
                    if (!threadDataCountMapping[record.ThreadId].ContainsKey(childKey))
                    {
                        threadDataCountMapping[record.ThreadId][childKey] = new List<string>();
                    }

                    threadDataTableMapping[record.ThreadId][childKey].Add(matchingRecord.DataTable);

                    var matching103Record = modificationID103Records.FirstOrDefault(i => i.ScheduleName == matchingRecord.ScheduleName);
                    if (matching103Record != null)
                    {
                        matchingRecord.TableCount = matching103Record.TableCount;
                    }

                    threadDataCountMapping[record.ThreadId][childKey].Add(matchingRecord.TableCount.ToString());
                }
            }

            foreach (var record in successData)
            {
                var childKey = Path.GetFileNameWithoutExtension(record.FileName);

                if (threadDataTableMapping.TryGetValue(record.ThreadId, out var dataTableMappingForThread) &&
                    threadDataCountMapping.TryGetValue(record.ThreadId, out var dataCountMappingForThread))
                {
                    if (dataTableMappingForThread.TryGetValue(childKey, out var tables))
                    {
                        record.DataTable = string.Join("<br/>", tables);
                    }
                    if (dataCountMappingForThread.TryGetValue(childKey, out var counts))
                    {
                        record.TableCountConvert = string.Join("<br/>", counts);
                    }

                    var modificationLine = modificationLines
                        .Where(line => line.Key == childKey)
                        .SelectMany(line => line.Value)
                        .OrderBy(i => i.ModificationDate)
                        .FirstOrDefault(i => i.ModificationID == this._100 || i.ModificationID == this._101 || i.ModificationID == this._102)?.ModificationLine;

                    if (modificationLine.HasValue)
                    {
                        record.ModificationLine = modificationLine.Value;
                        filteredSuccessData.Add(record);
                    }
                }
            }

            return filteredSuccessData;
        }


        private string BuildHtmlBody(DataTable dataTable, string title, int line, bool isRunningTasks = false)
        {
            var htmlBody = new StringBuilder();

            bool hasFailure = dataTable.AsEnumerable().Any(row => row["Success"].ToString().StartsWith("N"));

            if (hasFailure)
            {
                htmlBody.Append($@"
                <h3>{title}：{line} 筆</h3>
                <table border='1' style='width:100%; text-align:center;'>
                    <thead>
                        <tr>
                            <th>編號</th>
                            <th>排程名稱</th>
                            <th>最近執行時間</th>
                            <th>執行結果</th>
                            <th>總執行時間(秒)</th>
                            <th>成功與否</th>
                        </tr>
                    </thead>
                    <tbody>");
            }
            else
            {
                htmlBody.Append($@"
                <h3>{title}：{line} 筆</h3>
                <table border='1' style='width:100%; text-align:center;'>
                    <thead>
                        <tr>
                           <th>編號</th>
                            <th>排程名稱</th>
                            <th>最近執行時間</th>
                            <th>執行結果</th>
                            <th>資料異動筆數</th>
                            <th>資料表</th>
                            <th>資料表說明</th>
                            <th>資料表總筆數</th>
                            <th>總執行時間(秒)</th>
                            <th>成功與否</th>
                        </tr>
                    </thead>
                    <tbody>");
            }

            int rowNumber = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                string success = row["Success"].ToString();
                string idColor = success.StartsWith("N") ? "style='color:red;'" : "";

                if (success.StartsWith("N"))
                {
                    htmlBody.Append($@"
                    <tr>
                        <td>{rowNumber++}</td>
                        <td>{row["ScheduleName"]}</td>
                        <td>{row["ModificationDate"]}</td>
                        <td {idColor}>{row["ModificationType"]}</td>
                        <td>{row["TotalSeconds"]}</td>
                        <td {idColor}>{success}</td>
                    </tr>");
                }
                else if (isRunningTasks)
                {
                    string tableDescription = string.Empty;
                    if (Enum.TryParse(row["ScheduleName"].ToString(), out ScheduleName scheduleEnum))
                    {
                        tableDescription = EnumUtil.GetEnumDescription(scheduleEnum);
                    }

                    htmlBody.Append($@"
                    <tr>
                        <td>{rowNumber++}</td>
                        <td>{row["ScheduleName"]}</td>
                        <td>{row["ModificationDate"]}</td>
                        <td {idColor}>{row["ModificationType"]}</td>
                        <td>{row["ModificationLine"]}</td>
                        <td>{row["DataTable"]}</td>
                        <td>{tableDescription}</td>
                        <td>{row["TableCount"]}</td>
                        <td>{row["TotalSeconds"]}</td>
                        <td {idColor}>{success}</td>
                    </tr>");
                }
                else
                {

                    // 取得資料表名稱並拆分
                    string dataTables = row["DataTable"].ToString();
                    string dataTablesCount = row["TableCountConvert"].ToString();
                    List<string> tableNames = dataTables.Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> tableCounts = dataTablesCount.Split(new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // 使用新的格式，合併資料表顯示
                    int totalTables = tableNames.Count;

                    for (int i = 0; i < totalTables; i++)
                    {
                        if (i >= tableNames.Count || i >= tableCounts.Count)
                        {
                            break;
                        }
                        string tableName = tableNames[i].Trim() ?? string.Empty;
                        string tableCount = tableCounts[i].Trim() ?? string.Empty;
                        string tableDescription = string.Empty;
                        if (Enum.TryParse(row["ScheduleName"].ToString(), out ScheduleName scheduleEnum))
                        {
                            tableDescription = EnumUtil.GetEnumDescription(scheduleEnum);
                        }

                        if (i == 0)
                        {
                            htmlBody.Append($@"
                            <tr>
                                <td rowspan=""{totalTables}"">{rowNumber++}</td>
                                <td rowspan=""{totalTables}"">{row["ScheduleName"]}</td>
                                <td rowspan=""{totalTables}"">{row["ModificationDate"]}</td>
                                <td rowspan=""{totalTables}"" {idColor}>{row["ModificationType"]}</td>
                                <td rowspan=""{totalTables}"">{row["ModificationLine"]}</td>
                                <td>{tableName}</td>
                                <td rowspan=""{totalTables}"">{tableDescription}</td>
                                <td>{tableCount}
                                <td rowspan=""{totalTables}"">{row["TotalSeconds"]}</td>
                                <td rowspan=""{totalTables}"" {idColor}>{success}</td>
                            </tr>");
                        }
                        else
                        {
                            htmlBody.Append($@"
                            <tr>
                                <td>{tableName}</td>
                                <td>{tableCount}</td>
                            </tr>");
                        }
                    }
                }
            }

            htmlBody.Append(@"
            </tbody>
            </table>");

            return htmlBody.ToString();
        }



        private DataTable ConvertToDataTable(IList<ChangeHistory> results)
        {
            if (results == null || !results.Any())
            {
                _logger.Info("無");
                return new DataTable();
            }
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("FileName");
            dataTable.Columns.Add("ModificationDate");
            dataTable.Columns.Add("ModificationType");
            dataTable.Columns.Add("DataTable");
            dataTable.Columns.Add("ModificationLine");
            dataTable.Columns.Add("TotalSeconds");
            dataTable.Columns.Add("Success");
            dataTable.Columns.Add("ScheduleName");
            dataTable.Columns.Add("TableCountConvert");
            dataTable.Columns.Add("TableCount");

            foreach (var result in results)
            {
                DataRow row = dataTable.NewRow();
                row["FileName"] = result.FileName;
                row["ModificationDate"] = result.ModificationDate.ToLocalTime();
                row["ModificationType"] = result.ModificationType;
                row["DataTable"] = result.DataTable;
                row["ModificationLine"] = result.ModificationLine;
                row["TotalSeconds"] = result.TotalSeconds;
                row["Success"] = result.Success;
                row["ScheduleName"] = result.ScheduleName;
                row["TableCountConvert"] = result.TableCountConvert;
                row["TableCount"] = result.TableCount;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}

