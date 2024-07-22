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
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Data.Items;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    internal class WeekMonthlyRespository : IMailInfo<MemberRecordItemModel>, IMailRecord<MailRecord>
    {
        private readonly ILoggerService _logger;
        private readonly ID titleField = new ID("{D5501C04-3956-4A2F-AE0D-00DEB77FA4B5}");
        private readonly ID linkField = new ID("{C2E751CD-1E7D-4EA7-889B-344117133A09}");

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
                using (new LanguageSwitcher("zh-TW"))
                {
                    Database db = Database.GetDatabase("web");
                    foreach (var item in WeekMonthlyReportsItems)
                    {
                        var targetItem = db.GetItem(item.ItemId);
                        if (targetItem != null && !string.IsNullOrEmpty(ItemUtils.GetFieldValue(targetItem, titleField)))
                        {
                            item.Title = ItemUtils.GetFieldValue(targetItem, titleField);
                            item.Url = ItemUtils.GeneralLink(targetItem, linkField).Url;
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

        public void SendMail(IEnumerable<MemberRecordItemModel> infos, Item settings)
        {
            if (infos == null || !infos.Any())
            {
                _logger.Info("無新的周月報");
                return;
            }
            var GroupByMember = infos.GroupBy(x => x.PlatFormId);
            List<MailSchema> Mails = new List<MailSchema>();
            var homeUrl = $"https://";
            var cdHostName = Settings.GetSetting("CDHostName");
            homeUrl += cdHostName;
            foreach (var group in GroupByMember)
            {
                if (string.IsNullOrEmpty(group.First().MemberEmail) || string.IsNullOrEmpty(group.First().Title))
                {
                    continue;
                }
                List<MailRecord> mailRecords = new List<MailRecord>();
                Mails.Clear();
                foreach (var item in group)
                {
                    var mail = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<p>親愛的客戶您好：</p>");
                    sb.Append(string.Format("<p>第一銀行提醒您，「 <span style='color:red;'>{0}</span> 」來囉，詳情請於第e理財網查看，感謝您的配合，謝謝。</p>", item.Title));
                    sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", homeUrl));
                    mail.Content = sb.ToString();
                    mail.MailTo = item.MemberEmail;
                    mail.Topic = string.Format("「第一銀行第e理財網」{0}", item.Title);
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
                    MailServerOption mailServerOption = new MailServerOption(settings);
                    using (var client = mailServerOption.ToSMTPClient())
                    {
                        foreach (var item in Mails)
                        {
                            var encoding = Encoding.UTF8;
                            using (MailMessage message = new MailMessage()
                            {
                                From = new MailAddress(mailServerOption.User, string.IsNullOrEmpty(mailServerOption.UserName) ? "第e理財網" : mailServerOption.UserName),
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
                                    _logger.Error($"週月報通知Mail發送To{item.MailTo}失敗:{ex.ToString()}");
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
