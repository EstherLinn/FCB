using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Schedules.Mail;
using Foundation.Wealth.Manager;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class ConsultMailRespository : IMailInfo<MemberConsultScheduleModel>
    {
        private readonly ILoggerService _logger;
        private readonly ID consultRelatedRoot = new ID("{3966B7A6-C804-4DA4-8B20-C08B57585BDA}");
        private readonly ID consultScheduleLink = new ID("{E10AE7C2-4C31-4A92-AB8D-DD82898DFCD1}");
        private readonly ID consultListLink = new ID("{E770630B-C8E6-443C-8387-1D3525B11151}");

        private readonly ID useTestEmail = new ID("{CF5A5BF5-B530-4ABB-8D88-AA54FC27FA35}");
        private readonly ID testEmail = new ID("{A1CC4A9D-B62D-4E87-BCB9-198646633A90}");

        public ConsultMailRespository(ILoggerService logger)
        {
            this._logger = logger;
        }

        public List<MemberConsultScheduleModel> GetAllMemebrConsults()
        {
            string sql = @"SELECT 
                            [ScheduleID]
                           ,[CustomerName]
                           ,[EmployeeName]
                           ,[EmployeeID]
						   ,[ScheduleDate]
                           ,[StartTime]
						   ,[EndTime]
						   ,[BranchName]
                           ,[Mail]
                           ,[Subject]
						   ,[Description]
                           ,b.VideoInfoOpen
                           FROM [ConsultSchedule] as a WITH (NOLOCK)
                           left join FCB_Member as b WITH (NOLOCK) on a.CustomerID = b.WebBankId
                           where [ScheduleDate]= CAST(GETDATE() AS DATE) and StartTime >= FORMAT(GETDATE(), 'HH:mm') and StatusCode = '1'
                           ORDER BY [CustomerName],[StartTime]";

            var result = DbManager.Custom.ExecuteIList<MemberConsultScheduleModel>(sql, null, commandType: CommandType.Text)?.ToList() ?? new List<MemberConsultScheduleModel>();
            return result;
        }
        public void SendMail(IEnumerable<MemberConsultScheduleModel> consults, Item settings)
        {
            if (consults == null || !consults.Any())
            {
                _logger.Info("無新的理財諮詢預約");
                return;
            }
            var homeUrl = $"https://";
            var cdHostName = Settings.GetSetting("CDHostName");
            homeUrl += cdHostName;
            string consultUrl;
            using (new LanguageSwitcher("zh-TW"))
            {
                Database db = Database.GetDatabase("web");
                consultUrl = ItemUtils.GeneralLink(db.GetItem(consultRelatedRoot), consultScheduleLink)?.Url;
                consultUrl = new Uri(consultUrl).AbsolutePath.ToString();
            }
            List<MailSchema> mails = new List<MailSchema>();
            foreach (var consultSchedule in consults)
            {
                //會員如關閉理財視訊通知，不發Mail
                if (!consultSchedule.VideoInfoOpen)
                {
                    continue;
                }
                var mailSchema = new MailSchema();
                string content = $@"親愛的客戶您好：<br><br>
第一銀行通知您，您於 {consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")} {consultSchedule.StartTime} 申請預約「遠距理財諮詢服務」即將開始，以下是您的預約資訊：<br><br>
分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br> 
為保障您的權益，若欲取消預約或更改預約時間，請於預定「遠距理財諮詢服務」開始前，登入第e理財網調整，感謝您的配合，謝謝。<br><br> 
第e理財網連結：{homeUrl + consultUrl + "?id=" + consultSchedule.ScheduleID.ToString()} <br><br>
※請勿直接回覆此信，若有疑問請洽第一銀行{consultSchedule.BranchName}理財業務人員，謝謝。
";
                mailSchema.Topic = "「第一銀行第e理財網」遠距理財諮詢服務 - 預約即將開始提醒";
                mailSchema.Content = content;
                mailSchema.MailTo = consultSchedule.Mail;
                mails.Add(mailSchema);

                // 給理財顧問的郵件內容
                var advisorMailSchema = new MailSchema()
                {
                    Topic = "「第一銀行第e理財網」遠距理財諮詢服務 - 今日預約即將開始提醒",
                    Content = $@"親愛的理顧您好：<br><br>
第一銀行通知您， {consultSchedule.CustomerName}客戶預約之「遠距理財諮詢服務」即將開始，以下是您的客戶預約資訊：<br><br>
預約日期：{consultSchedule.ScheduleDate.ToString("yyyy/MM/dd")}<br>
預約時段：{consultSchedule.StartTime}~{consultSchedule.EndTime}<br>
主要往來分行：{consultSchedule.BranchName}<br>
理財顧問：{consultSchedule.EmployeeName}先生/小姐<br>
諮詢主題：{consultSchedule.Subject}<br>
其他諮詢內容：{consultSchedule.Description}<br><br>
為維護良好之客我關係及服務品質，請於預定「遠距理財諮詢服務」時間開始前，登入第e理財網提供諮詢服務。<br><br>
第e理財網連結：{homeUrl + consultUrl + "?id=" + consultSchedule.ScheduleID.ToString()} <br><br>
※此為系統主動發送信函，請勿直接回覆此封信件，謝謝。"
                };

                using (new LanguageSwitcher("zh-TW"))
                {
                    Database db = Database.GetDatabase("web");
                    if (Settings.GetSetting("ConsultUseTestEmail") == "1" && ItemUtils.GetFieldValue(db.GetItem(consultRelatedRoot), useTestEmail) == "1")
                    {
                        advisorMailSchema.MailTo = ItemUtils.GetFieldValue(db.GetItem(consultRelatedRoot), testEmail);
                    }
                }

                if (string.IsNullOrEmpty(advisorMailSchema.MailTo))
                {
                    advisorMailSchema.MailTo = "i" + Regex.Replace(consultSchedule.EmployeeID.TrimStart('0'), ".$", string.Empty).PadLeft(5, '0') + "@firstbank.com.tw";
                }

                mails.Add(advisorMailSchema);
            }

            if (mails.Any())
            {
                MailServerOption mailServerOption = new MailServerOption(settings);
                using (var client = mailServerOption.ToSMTPClient())
                {
                    foreach (var item in mails)
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
                                message.To.Add(item.MailTo);
                                message.Subject = item.Topic;
                                message.Body = item.Content;
                                client.Send(message);
                                _logger.Info($"預約諮詢Mail發送To:{item.MailTo}");
                            }
                            catch (Exception ex)
                            {
                                _logger.Error($"理財諮詢預約發送失敗To {item.MailTo} Exception:{ex}");
                            }
                        }
                    }
                }
            }
        }
    }
}

