using Feature.Wealth.ScheduleAgent.Models.Mail;
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

        public ScheduleAlarmRespository(ILoggerService logger)
        {
            this._logger = logger;
        }

        public List<ChangeHistory> GetChangeHistory()
        {
            string sql = @"SELECT *
                           FROM [ChangeHistory] WITH (NOLOCK)
                           WHERE ModificationDate >= DATEADD(HOUR, -1, GETUTCDATE())
                           ORDER BY [ModificationDate] DESC";

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
                            && (s.ModificationID == this._200done))).ToList();

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
                mailBody.Append(BuildHtmlBody(tasksDataTable, "未完成的排程(尚在執行中)", runningTasks.Count));
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

            var dataTableMapping = new Dictionary<string, List<string>>();

            foreach (var record in results.Where(i => i.Success == "Y" && (i.ModificationID == this._100 || i.ModificationID == this._101 || i.ModificationID == this._102)))
            {
                var childKey = Path.GetFileNameWithoutExtension(record.FileName);
                if (!dataTableMapping.ContainsKey(childKey))
                {
                    dataTableMapping[childKey] = new List<string>();
                }
                dataTableMapping[childKey].Add(record.DataTable);
            }

            foreach (var record in successData)
            {
                var childKey = Path.GetFileNameWithoutExtension(record.FileName);

                if (dataTableMapping.TryGetValue(childKey, out var tables))
                {
                    record.DataTable = string.Join("<br/>", tables);
                }

                var modificationLine = modificationLines
                    .Where(line => line.Key == childKey)
                    .SelectMany(line => line.Value)
                    .FirstOrDefault(i => i.ModificationID == this._100 || i.ModificationID == this._101 || i.ModificationID == this._102)?.ModificationLine;

                if (modificationLine.HasValue)
                {
                    record.ModificationLine = modificationLine.Value;
                    filteredSuccessData.Add(record);
                }
            }

            return filteredSuccessData;
        }


        private string BuildHtmlBody(DataTable dataTable, string title, int line)
        {
            var htmlBody = new StringBuilder();
            htmlBody.Append($@"
                <h3>{title}：{line} 筆</h3>
                <table border='1' style='width:100%; text-align:center;'>
                    <tr>
                        <th>編號</th>
                        <th>檔案名稱</th>
                        <th>日期</th>
                        <th>執行類型</th>
                        <th>執行行數</th>
                        <th>資料表</th>
                        <th>執行時間</th>
                        <th>成功與否</th>
                    </tr>");

            int rowNumber = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                string success = row["Success"].ToString();
                string idColor = success.StartsWith("N") ? "style='color:red;'" : "";

                htmlBody.Append($@"
                <tr>
                    <td>{rowNumber++}</td>
                    <td>{row["FileName"]}</td>
                    <td>{row["ModificationDate"]}</td>
                    <td {idColor}>{row["ModificationType"]}</td>
                    <td>{row["ModificationLine"]}</td>
                    <td>{row["DataTable"]}</td>
                    <td>{row["TotalSeconds"]}</td>
                    <td {idColor}>{success}</td>
                </tr>");
            }

            htmlBody.Append(@"
                </table>
           ");

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
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}

