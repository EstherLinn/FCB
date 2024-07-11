using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Schedules.Mail;
using Foundation.Wealth.Manager;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    internal class WeekMonthlyRespository : IMailInfo<MemberRecordItemModel>, IMailRecord<MailRecord>
    {
        private readonly MailServerOption mailServerOption = new MailServerOption();
        private readonly ILoggerService _logger;
        public WeekMonthlyRespository(ILoggerService logger)
        {
            this._logger = logger;
        }
        public IEnumerable<MemberRecordItemModel> GetWeekMonthlyReports()
        {
            List<MemberRecordItemModel> WeekMonthlyReportsItems = new List<MemberRecordItemModel>();
            string sql = $"Select A.PlatFormId,A.MemberEmail,B.ItemId FROM FCB_Member as A ,[PublishItemRecord] AS B  WHERE A.MemberEmail is not null  AND B.IsSend = @isSend AND B.Type=@type";
            var para = new { isSend = false, type = MailInfoTypeEnum.週月報.ToString() };
            WeekMonthlyReportsItems = DbManager.Custom.ExecuteIList<MemberRecordItemModel>(sql, para, commandType: CommandType.Text)?.ToList();
            if (WeekMonthlyReportsItems.Any() && !string.IsNullOrEmpty(WeekMonthlyReportsItems.FirstOrDefault()?.ItemId))
            {
                var homeUrl = $"https://";
                var cdHostName = Settings.GetSetting("CDHostName");
                using (new LanguageSwitcher("zh-TW"))
                {
                    Database db = Database.GetDatabase("web");
                    foreach (var item in WeekMonthlyReportsItems)
                    {
                        var targetItem = db.GetItem(item.ItemId);
                        if (targetItem != null)
                        {

                            item.Title = ItemUtils.GetFieldValue(targetItem,"Title");
                            item.Url = ItemUtils.GeneralLink(targetItem, "Link").Url;
                            if (!string.IsNullOrEmpty(item.Url))
                            {
                                item.Url = homeUrl + cdHostName + item.Url;
                            }
                        }
                    }
                }
            }
            return WeekMonthlyReportsItems;
        }
        public void UpdatePublishRecordIsSend(IEnumerable<string> infos)
        {
            var sql = $"Update PublishItemRecord Set IsSend =@isSend where ItemId in @items";
            var para = new { isSend = true, items = infos };
            DbManager.Custom.ExecuteNonQuery(sql, para, CommandType.Text);
        }
        public void InsertMailRecords(IEnumerable<MailRecord> infos)
        {
            var sql = $"INSERT INTO MailRecord (PlatFormId, MailInfoType,InfoContent,InfoLink,InfoDateTime,HaveRead) " +
           $"VALUES (@PlatFormId, @MailInfoType,@InfoContent,@InfoLink,@InfoDateTime,@HaveRead)";
            DbManager.Custom.ExecuteNonQuery(sql, infos, CommandType.Text);
        }

        public void SendMail(IEnumerable<MemberRecordItemModel> infos)
        {
            if (infos == null || !infos.Any())
            {
                _logger.Info("empty");
                return;
            }
            var GroupByMember = infos.GroupBy(x => x.PlatFormId);
            List<MailSchema> Mails = new List<MailSchema>();

            foreach (var group in GroupByMember)
            {
                List<MailRecord> mailRecords = new List<MailRecord>();
                Mails.Clear();
                foreach (var item in group)
                {
                    var mail = new MailSchema();
                    mail.Content = item.Title;
                    mail.MailTo = item.MemberEmail;
                    mail.Topic = item.Title;
                    Mails.Add(mail);
                    mailRecords.Add(new MailRecord
                    {
                        PlatFormId = item.PlatFormId,
                        InfoDateTime = DateTime.Now,
                        InfoContent = string.Format("{0}", item.Title),
                        InfoLink = item.Url,
                        MailInfoType = MailInfoTypeEnum.週月報.ToString(),
                        HaveRead = false
                    });
                }
                if (Mails.Any())
                {
                    using (var client = mailServerOption.ToSMTPClient())
                    {
                        foreach (var item in Mails)
                        {
                            var encoding = Encoding.UTF8;
                            using (MailMessage message = new MailMessage()
                            {
                                From = new MailAddress(mailServerOption.UserName, "第一理財網"),
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
                                    //add EnableSsl
                                    client.EnableSsl = Sitecore.MainUtil.GetBool(Settings.GetSetting("MailServerUseSsl"), true);
                                    client.Send(message);
                                    _logger.Info($"週月報通知Mail發送To:{item.MailTo}");
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error($"週月報通知Mail發送失敗:{ex.ToString()}");
                                }
                            }
                        }
                    }
                }
                if (mailRecords.Any())
                {
                    InsertMailRecords(mailRecords);
                }
            }

            UpdatePublishRecordIsSend(infos.GroupBy(x => x.ItemId).Select(x => x.Key).ToList());
        }
    }
}
