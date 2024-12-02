using Dapper;
using Feature.Wealth.Component.Models.Consult;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using log4net;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class ConsultRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        private readonly List<string> _dnis = Settings.GetSetting("OctonApiDNISList").Split(',').ToList();

        public IList<ConsultSchedule> GetConsultScheduleList()
        {
            string sql = @"SELECT
                           [ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description],[AdvistorAdvice]
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
                           ,[Subject],[Description],[AdvistorAdvice]
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

            var result = this._dbConnection.Query<ConsultSchedule>(sql, new { ScheduleID = scheduleID })?.FirstOrDefault();

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
                           ,AdvistorAdvice = @AdvistorAdvice
                           ,StatusCode = @StatusCode
                           ,ModifiedOn = GETDATE()
                           WHERE ScheduleID = @ScheduleID";

            this._dbConnection.Execute(sql, consultSchedule);
        }

        public IList<IMVP_HOLIDAY> GetHoliday()
        {
            string IMVP_HOLIDAY = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.IMVP_HOLIDAY);

            string sql = $@"SELECT [CALENDAR_DATE]
                            ,CONVERT(char(10), CONVERT(date, [CALENDAR_DATE]), 126) [RealDate]
                            FROM {IMVP_HOLIDAY} WITH (NOLOCK)
                            WHERE [CALENDAR_DATE] > CONVERT(varchar, GETDATE(), 112)
                            AND [CALENDAR_DATE] < CONVERT(varchar, DATEADD(month, 1, GETDATE()), 112)";

            var result = this._dbConnection.Query<IMVP_HOLIDAY>(sql)?.ToList() ?? new List<IMVP_HOLIDAY>();
            return result;
        }

        public Branch GetBranch(string employeeCode)
        {
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            string Branch_Data = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Branch_Data);

            string sql = $@"SELECT
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
                           FROM {HRIS} A WITH (NOLOCK)
                           LEFT JOIN {Branch_Data} B WITH (NOLOCK) ON SUBSTRING(A.OfficeOrBranchCode, 2, 3) = B.BranchCode
                           WHERE A.EmployeeCode = @EmployeeCode";

            var result = this._dbConnection.Query<Branch>(sql, new { EmployeeCode = employeeCode })?.FirstOrDefault() ?? new Branch();

            if (!string.IsNullOrEmpty(result.BranchName))
            {
                result.BranchName = result.BranchName.Trim().Replace("\t", string.Empty).Replace("　", string.Empty);
            }

            return result;
        }

        public string GetDNIS(ConsultSchedule consultSchedule)
        {
            var usedDNIS = GetConsultScheduleList()
                .Where(c => c.ScheduleDate == consultSchedule.ScheduleDate
                && c.StartTime == consultSchedule.StartTime
                && c.StatusCode != "3").Select(c => c.DNIS).ToList();

            foreach (var d in this._dnis)
            {
                if (usedDNIS == null || !usedDNIS.Any())
                {
                    return d;
                }
                else if (!usedDNIS.Contains(d))
                {
                    return d;
                }
            }

            return null;
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
        //預約確認中-客戶
        public string GetWaitMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 – 確認中";
        }

        public string GetWaitMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 已預約「遠距理財諮詢服務」，本次預約待理財顧問確認後，系統將寄送預約完成通知信，感謝您耐心等候，謝謝。<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }
        //預約成功-客戶
        public string GetSuccessMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 - 預約成功";
        }

        public string GetSuccessMailContent(ConsultSchedule consultSchedule, string url)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 申請「遠距理財諮詢服務」已預約成功，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
為保障您的權益，若欲取消預約或更改預約時間，請於預定「遠距理財諮詢服務」開始前，登入第e理財網調整，感謝您的配合，謝謝。<br><br> 
第e理財網連結：{url} <br><br>
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
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
為保障您的權益，若欲取消預約或更改預約時間，請於預定「遠距理財諮詢服務」開始前，登入第e理財網調整，感謝您的配合，謝謝。<br><br> 
第e理財網連結：{url} <br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }
        //預約失敗-客戶
        public string GetRejectMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務 - 預約失敗";
        }
        public string GetRejectMailContent(ConsultSchedule consultSchedule, string msg, string url)
        {
            string result = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 已預約「遠距理財諮詢服務」，惟理財顧問因{msg}，無法於您預約的時段為您提供「遠距理財視訊諮詢服務」，您可再選擇其他時段進行預約，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br>
{(string.IsNullOrEmpty(consultSchedule.AdvistorAdvice) ? "<br>" : $"理顧意見：{consultSchedule.AdvistorAdvice}<br><br>")}
第e理財網連結：{url}<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }
        //取消預約-客戶
        public string GetClientCancelMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務預約 – 取消預約";
        }

        public string GetClientCancelMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的客戶您好：<br><br> 
第一銀行通知您，您已成功取消於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 預約的「遠距理財諮詢服務」，為保障您的權益，若對此次預約取消有疑問，可致電您的理財顧問再次確認，謝謝。<br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";

            return result;
        }
        //主管協助取消預約-客戶
        public string GetCancelManagerMailTopic()
        {
            return "「第一銀行第e理財網」遠距理財諮詢服務預約 – 預約取消";
        }

        public string GetCancelManagerMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的客戶您好：<br><br> 
第一銀行通知您，您已取消完成於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 的「遠距理財諮詢服務」。<br>
為保障您的權益，若對此次預約取消有任何問題，可致電您的理財顧問聯繫，謝謝。<br><br>
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
                        this._log.Error($"預約諮詢Mail發送To:{mail.MailTo}");
                    }
                    catch (Exception ex)
                    {
                        this._log.Error($"預約諮詢Mail發送失敗:{ex.ToString}");
                    }
                }
            }
        }

        public IList<CustomerInfo> GetCustomerInfos(string employeeCode)
        {
            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);

            string sql = $@"SELECT DISTINCT
                            A.CIF_ID AS CIF_ID,
                            A.CIF_CUST_NAME AS CustomerName,
                            A.CIF_E_MAIL_ADDRESS,
                            A.CIF_TEL_NO1,
                            A.CIF_TEL_NO3,
                            C.WebBankId AS WebBankId,
                            C.MemberEmail,
                            D.OfficeOrBranchName AS BranchName
                            FROM {CIF} AS A WITH (NOLOCK)
                            LEFT JOIN {CFMBSEL} AS B WITH (NOLOCK) ON A.CIF_ID = B.CUST_ID
                            LEFT JOIN FCB_Member AS C WITH (NOLOCK) ON B.PROMOTION_CODE COLLATE Latin1_General_CS_AS = C.WebBankId
                            LEFT JOIN {HRIS} AS D WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(A.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = D.EmployeeCode
                            WHERE C.WebBankId IS NOT NULL AND D.EmployeeCode IS NOT NULL AND D.EmployeeCode = @EmployeeCode";

            var result = this._dbConnection.Query<CustomerInfo>(sql, new { EmployeeCode = employeeCode })?.ToList() ?? new List<CustomerInfo>();

            if (result != null && result.Any())
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].CIF_ID.Length >= 10)
                    {
                        result[i].CIF_ID = result[i].CIF_ID.Substring(0, 3) + "XXX" + result[i].CIF_ID.Substring(6, 4);
                    }

                    if (!string.IsNullOrEmpty(result[i].CustomerName))
                    {
                        result[i].CustomerName = result[i].CustomerName.Trim().Replace("    ", string.Empty).Replace("　", string.Empty);
                    }

                    if (!string.IsNullOrEmpty(result[i].BranchName))
                    {
                        result[i].BranchName = result[i].BranchName.Trim().Replace("    ", string.Empty).Replace("　", string.Empty);
                    }
                }
            }

            return result;
        }

        public string GetCIF_ID(string id)
        {
            var sql = $@"SELECT TOP 1 [CUST_ID]
                         FROM [CFMBSEL] WITH (NOLOCK)
                         WHERE PROMOTION_CODE = @PROMOTION_CODE";

            var result = this._dbConnection.Query<string>(sql, new { PROMOTION_CODE = id })?.FirstOrDefault() ?? string.Empty;

            return result;
        }

        public void InsertMailRecords(List<MailRecord> mailRecords)
        {
            var sql = $"INSERT INTO MailRecord (PlatFormId, MailInfoType,InfoContent,InfoLink,InfoDateTime,HaveRead) " +
                       $"VALUES (@PlatFormId, @MailInfoType,@InfoContent,@InfoLink,@InfoDateTime,@HaveRead)";
            DbManager.Custom.ExecuteNonQuery(sql, mailRecords, CommandType.Text);
        }

        public MailRecord GetMailRecord(ConsultSchedule consultSchedule, string url)
        {
            var mailRecord = new MailRecord
            {
                PlatFormId = consultSchedule.CustomerID,
                InfoDateTime = DateTime.Now,
                MailInfoType = MailInfoTypeEnum.理顧預約.ToString(),
                InfoContent = $@"您已成功預約 {DateTime.Parse(consultSchedule.ScheduleDate.ToString("yyyy/MM/dd") + " " + consultSchedule.StartTime).ToString("yyyy/MM/dd tt HH:mm")} 的理顧諮詢，請查看會議連結",
                InfoLink = url
            };

            return mailRecord;
        }
        //預約確認中-理顧
        public string GetAdvisorConfirmationMailTopic()
        {
            return "【第e理財網】遠距理財諮詢服務 -- 預約待確認";
        }
        public string GetAdvisorConfirmationMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的理顧您好：<br><br>
通知您， {consultSchedule.CustomerName}客戶於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 已預約「遠距理財諮詢服務」，請至智慧行銷全通路平台(IMVP)系統-理顧行事曆功能，確認並回覆客戶預約時間，系統將您回覆之內容寄通知信給予客戶。<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
客戶姓名:{consultSchedule.CustomerName}先生/小姐<br>
主要往來分行：{consultSchedule.BranchName}<br>
專責理顧：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。";
            return result;
        }
        //確認成功-理顧
        public string GetAdvisorSuccessMailTopic()
        {
            return "【第e理財網】遠距理財諮詢服務 -- 預約確認完成";
        }
        public string GetAdvisorSuccessMailContent(ConsultSchedule consultSchedule, string url)
        {
            string result = $@"親愛的理顧您好：<br><br>
通知您，您已完成確認 {consultSchedule.CustomerName}客戶於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 申請「遠距理財諮詢服務」之預約，以下是本次的預約資訊：<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
客戶姓名:{consultSchedule.CustomerName}先生/小姐<br>
主要往來分行：{consultSchedule.BranchName}<br>
專責理顧：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br>
為維護良好之客我關係及服務品質，請於預定「遠距理財諮詢服務」時間開始前，登入第e理財網提供諮詢服務。<br><br>
第e理財網連結：{url} <br><br>
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。";
            return result;
        }
        //取消預約-理顧
        public string GetAdvisorCancelMailTopic()
        {
            return "【第e理財網】遠距理財諮詢服務 -- 客戶線上取消預約";
        }

        public string GetAdvisorCancellationMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的理顧您好：<br><br>
通知您，{consultSchedule.CustomerName}客戶取消「遠距理財諮詢服務」預約，以下是您的客戶預約資訊：<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
主要往來分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br>
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。";
            return result;
        }
        //預約取消-理顧
        public string GetEmployeeCancelMailTopic()
        {
            return "【第e理財網】遠距理財諮詢服務 -- 取消預約(理財主管線上取消)";
        }

        public string GetEmployeeCancellationMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的理顧您好：<br><br>
通知您，{consultSchedule.CustomerName}客戶取消「遠距理財諮詢服務」預約，以下是您的客戶預約資訊：<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
主要往來分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br>
若對此次預約取消有任何問題，請致電客戶聯繫，謝謝。<br>
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。";
            return result;
        }

        public string GetEmployeeEmail(ConsultSchedule consultSchedule)
        {
            var email = string.Empty;

            if (ConsultRelatedLinkSetting.GetUseTestEmail())
            {
                email = ConsultRelatedLinkSetting.GetTestEmail();
            }

            if (string.IsNullOrEmpty(email))
            {
                var mailCode = Regex.Replace(consultSchedule.EmployeeID.TrimStart('0'), ".$", string.Empty);
                email = "i" + mailCode.PadLeft(5, '0') + "@firstbank.com.tw";
            }

            return email;
        }

        //預約取消-主管
        public string GetManagerCancelMailTopic()
        {
            return "【第e理財網】遠距理財諮詢服務 -- 取消預約(理財主管線上取消)";
        }

        public string GetManagerCancellationMailContent(ConsultSchedule consultSchedule)
        {
            string result = $@"親愛的主管您好：<br><br>
通知您，您已協助{consultSchedule.CustomerName}客戶取消「遠距理財諮詢服務」預約，以下是該筆預約資訊：<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
主要往來分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br>
若對此次預約取消有任何問題，請致電客戶聯繫，謝謝。<br>
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。";
            return result;
        }
    }
}
