using Dapper;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Foundation.Wealth.Manager;
using log4net;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class ConsultRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        public IList<ConsultSchedule> GetConsultScheduleList()
        {
            string sql = @"SELECT
                           [ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description]
                           ,CASE 
                            WHEN ([StatusCode] = '1' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]))
                            THEN '2'
                            WHEN ([StatusCode] = '0' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]))
                            THEN '3'
                            ELSE [StatusCode] 
                            END [StatusCode]
                           ,[CreatedOn],[ModifiedOn]
                           ,IIF([ScheduleDate] = CAST(GETDATE() AS DATE), 1, 0) [Comming]
                           ,IIF(GETDATE() BETWEEN (CAST([ScheduleDate] AS varchar) + ' ' + [StartTime]) AND (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]) , 1, 0) [Start]
                           FROM [ConsultSchedule] WITH (NOLOCK)
                           ORDER BY [ScheduleDate],[StartTime]";

            var result = this._dbConnection.Query<ConsultSchedule>(sql)?.ToList() ?? new List<ConsultSchedule>();

            if (result != null && result.Any())
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ScheduleDateString = result[i].ScheduleDate.ToString("yyyy-MM-dd");
                }
            }

            return result;
        }

        public ConsultSchedule GetConsultSchedule(string scheduleID)
        {
            string sql = @"SELECT
                           [ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description]
                           ,CASE 
                            WHEN ([StatusCode] = '1' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]))
                            THEN '2'
                            WHEN ([StatusCode] = '0' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]))
                            THEN '3'
                            ELSE [StatusCode] 
                            END [StatusCode]
                           ,[CreatedOn],[ModifiedOn]
                           ,IIF([ScheduleDate] = CAST(GETDATE() AS DATE), 1, 0) [Comming]
                           ,IIF(GETDATE() BETWEEN (CAST([ScheduleDate] AS varchar) + ' ' + [StartTime]) AND (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]) , 1, 0) [Start]
                           FROM [ConsultSchedule] WITH (NOLOCK)
                           WHERE ScheduleID = @ScheduleID";

            var result = this._dbConnection.Query<ConsultSchedule>(sql, new { ScheduleID = scheduleID })?.FirstOrDefault() ?? new ConsultSchedule();

            return result;
        }

        public void InsertConsultSchedule(ConsultSchedule consultSchedule)
        {
            string sql = @"INSERT INTO [ConsultSchedule]
                           ([ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description],[StatusCode]
                           ,[CreatedOn],[ModifiedOn])
                           VALUES
                           (@ScheduleID,@EmployeeID,@EmployeeName
                           ,@CustomerID,@CustomerName,@ScheduleDate
                           ,@StartTime,@EndTime,@DNIS
                           ,@EmployeeURL,@CustomerURL,@DepartmentCode
                           ,@BranchCode,@BranchName,@BranchPhone
                           ,@Type,@Phone,@Mail
                           ,@Subject,@Description,@StatusCode
                           ,GETDATE(),GETDATE())";

            this._dbConnection.Execute(sql, consultSchedule);
        }

        public void UpdateConsultSchedule(ConsultSchedule consultSchedule)
        {
            string sql = @"UPDATE [ConsultSchedule] SET
                           EmployeeID = @EmployeeID
                           ,EmployeeName = @EmployeeName
                           ,CustomerID = @CustomerID
                           ,CustomerName = @CustomerName
                           ,ScheduleDate = @ScheduleDate
                           ,StartTime = @StartTime
                           ,EndTime = @EndTime
                           ,DNIS = @DNIS
                           ,EmployeeURL = @EmployeeURL
                           ,CustomerURL = @CustomerURL
                           ,DepartmentCode = @DepartmentCode
                           ,BranchCode = @BranchCode
                           ,BranchName = @BranchName
                           ,BranchPhone = @BranchPhone
                           ,Type = @Type
                           ,Phone = @Phone
                           ,Mail = @Mail
                           ,Subject = @Subject
                           ,Description = @Description
                           ,StatusCode = @StatusCode
                           ,ModifiedOn = GETDATE()
                           WHERE ScheduleID = @ScheduleID";

            this._dbConnection.Execute(sql, consultSchedule);
        }

        public IList<Calendar> GetCalendar()
        {
            string sql = @"SELECT TOP 30
                           [Date]
                           ,[Week]
                           ,[IsHoliday]
                           ,[Description]
                           ,CONVERT(char(10), [RealDate], 126) [RealDate]
                           FROM [Calendar] WITH (NOLOCK) WHERE [RealDate] > GETDATE()";

            var result = this._dbConnection.Query<Calendar>(sql)?.ToList() ?? new List<Calendar>();
            return result;
        }

        public Branch GetBranch(string employeeCode)
        {
            string sql = @"SELECT
                           A.[OfficeOrBranchCode] [BranchCode],
                           A.[OfficeOrBranchName] [BranchName],
                           A.[DepartmentCode] [DepartmentCode],
                           CASE 
                           WHEN LEN(B.[PhoneNumber]) = 8
                           THEN '(' + TRIM(B.[PhoneAreaCode]) + ')' + SUBSTRING(B.[PhoneNumber], 1, 4) + '-' + SUBSTRING(B.[PhoneNumber], 5, 4)
                           WHEN LEN(B.[PhoneNumber]) = 7
                           THEN '(' + TRIM(B.[PhoneAreaCode]) + ')' + SUBSTRING(B.[PhoneNumber], 1, 3) + '-' + SUBSTRING(B.[PhoneNumber], 4, 4)
                           WHEN LEN(B.[PhoneNumber]) = 6
                           THEN '(' + TRIM(B.[PhoneAreaCode]) + ')' + SUBSTRING(B.[PhoneNumber], 1, 2) + '-' + SUBSTRING(B.[PhoneNumber], 3, 4)
                           ELSE '(' + TRIM(B.[PhoneAreaCode]) + ')' + B.[PhoneNumber]
                           END AS [BranchPhone]
                           FROM [HRIS] A WITH (NOLOCK)
                           LEFT JOIN [Branch_Data] B WITH (NOLOCK) ON SUBSTRING(A.OfficeOrBranchCode, 2, 3) = B.BranchCode
                           WHERE A.EmployeeCode = @EmployeeCode";

            var result = this._dbConnection.Query<Branch>(sql, new { EmployeeCode = employeeCode })?.FirstOrDefault() ?? new Branch();

            return result;
        }

        public string GetDNIS(ConsultSchedule consultSchedule)
        {
            var dnis = new List<string>
            {
                "1003",
                "1004",
                "1005",
                "1006",
                "1007"
            };

            var usedDNIS = GetConsultScheduleList()
                .Where(c => c.ScheduleDate == consultSchedule.ScheduleDate
                && c.StartTime == consultSchedule.StartTime
                && c.StatusCode != "3").Select(c => c.DNIS).ToList();

            if (usedDNIS != null && usedDNIS.Count > 0)
            {
                foreach (string d in usedDNIS)
                {
                    if (dnis.Contains(d))
                    {
                        dnis.Remove(d);
                    }
                }
            }

            return dnis.Count > 0 ? dnis[0] : null;
        }

        public bool CheckEmployeeSchedule(ConsultSchedule consultSchedule)
        {
            return GetConsultScheduleList()
                .Any(c => c.ScheduleDate == consultSchedule.ScheduleDate
                && c.StartTime == consultSchedule.StartTime
                && c.EmployeeID == consultSchedule.EmployeeID
                && c.StatusCode != "3");
        }

        internal void CancelConsultSchedule(Guid scheduleID)
        {
            string sql = @"UPDATE [ConsultSchedule] SET
                           StatusCode = '3'
                           ,ModifiedOn = GETDATE()
                           WHERE ScheduleID = @ScheduleID";

            this._dbConnection.Execute(sql, new { ScheduleID = scheduleID });
        }

        public string GetWaitMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務預約 – 確認中";
        }

        public string GetWaitMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 已預約「遠距理財諮詢服務」，本次預約待理財顧問確認後，系統將寄送預約完成通知信，感謝您耐心等候，謝謝。<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }

        public string GetSuccessMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 - 預約成功";
        }

        public string GetSuccessMailContent(ConsultSchedule consultSchedule, string url)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 申請「遠距理財諮詢服務」已預約成功，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
為保障您的權益，若欲取消預約或更改預約時間，請於預定「遠距理財諮詢服務」開始前，登入第e理財網調整，感謝您的配合，謝謝。<br><br> 
第e理財網連結：{url}?id={consultSchedule.ScheduleID.ToString()} <br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }

        public string GetNoticeMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 - 預約即將開始提醒";
        }

        public string GetNoticeMailContent(ConsultSchedule consultSchedule, string url)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 申請預約「遠距理財諮詢服務」即將開始，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
為保障您的權益，若欲取消預約或更改預約時間，請於預定「遠距理財諮詢服務」開始前，登入第e理財網調整，感謝您的配合，謝謝。<br><br> 
第e理財網連結：{url}?id={consultSchedule.ScheduleID.ToString()} <br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }

        public string GetRejectMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 - 預約失敗";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultSchedule"></param>
        /// <param name="msg"></param>
        /// <param name="url">預約列表頁</param>
        /// <returns></returns>
        public string GetRejectMailContent(ConsultSchedule consultSchedule, string msg, string url)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 已預約「遠距理財諮詢服務」，惟理財顧問因{msg}，無法於您預約的時段為您提供「遠距理財視訊諮詢服務」，您可再選擇其他時段進行預約，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
{url}<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }

        public string GetCancelMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務預約 – 取消預約";
        }

        public string GetCancelMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的客戶您好：<br><br> 
第一銀行通知您，您已成功取消於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 預約的「遠距理財諮詢服務」，為保障您的權益，若對此次預約取消有疑問，可致電您的理財顧問再次確認，謝謝。<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }

        private readonly ILog _log = Logger.General;

        public void SendMail(MailSchema mail, Item settings)
        {
            MailServerOption mailServerOption = new MailServerOption(settings);
            using (var client = mailServerOption.ToSMTPClient())
            {
                var encoding = Encoding.UTF8;
                using (MailMessage message = new MailMessage()
                {
                    From = new MailAddress(mailServerOption.From, string.IsNullOrEmpty(mailServerOption.UserName) ? "第e理財網" : mailServerOption.UserName),
                    IsBodyHtml = true,
                    HeadersEncoding = encoding,
                    BodyEncoding = encoding,
                    SubjectEncoding = encoding
                })
                {
                    try
                    {
                        message.To.Add(mail.MailTo);
                        message.Subject = mail.Topic;
                        message.Body = mail.Content;
                        client.Send(message);
                        this._log.Info($"預約諮詢Mail發送To:{mail.MailTo}");
                    }
                    catch (Exception ex)
                    {
                        this._log.Error($"預約諮詢Mail發送失敗:{ex.ToString}");
                    }
                }
            }
        }
    }
}
