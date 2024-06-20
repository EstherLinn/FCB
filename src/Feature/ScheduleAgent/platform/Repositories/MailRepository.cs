using Feature.Wealth.Account.Models.ReachInfo;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Schedules.Mail;
using Foundation.Wealth.Manager;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Sitecore.Configuration;
using System.Web;
using Sitecore.Text;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class MailRepository : IMailInfo<MemebrReachInfo>, IMailRecord<MailRecord>
    {
        private readonly MailServerOption mailServerOption = new MailServerOption();
        private readonly ILog Log = Logger.Account;

        public List<MemebrReachInfo> GetAllMemebrReachInfos()
        {
            List<MemebrReachInfo> memebrReachInfos = null;
            memebrReachInfos = DbManager.Custom.ExecuteIList<MemebrReachInfo>("sp_ReachInfoList", null, commandType: CommandType.StoredProcedure)?.ToList();
            return memebrReachInfos;
        }

        public void InsertMailRecords(IEnumerable<MailRecord> mailRecords)
        {
            var sql = $"INSERT INTO MailRecord (PlatFormId, MailInfoType,InfoContent,InfoLink,InfoDateTime,HaveRead) " +
                       $"VALUES (@PlatFormId, @MailInfoType,@InfoContent,@InfoLink,@InfoDateTime,@HaveRead)";
            DbManager.Custom.ExecuteNonQuery(sql, mailRecords, CommandType.Text);
        }

        public void SendMail(IEnumerable<MemebrReachInfo> memebrReachInfos)
        {
            if (memebrReachInfos == null || !memebrReachInfos.Any())
            {
                Log.Info("empty");
                return;
            }
            var GroupByMember = memebrReachInfos.GroupBy(x => x.PlatFormId);
            List<MailSchema> Mails = new List<MailSchema>();
            var homeUrl = $"https://";
            var cdHostName = Settings.GetSetting("CDHostName");
            homeUrl += cdHostName;
            foreach (var group in GroupByMember)
            {
                List<MailRecord> mailRecords = new List<MailRecord>();
                var Mail1 = group.Where(x => x.InfoType == "1" && x.InvestType.ToLower() == "fund");
                var Mail2 = group.Where(x => x.InfoType == "1" && x.InvestType.ToLower() != "fund");
                var Mail3 = group.Where(x => x.InfoType == "2" && x.RiseValue != null);
                var Mail4 = group.Where(x => x.InfoType == "2" && x.FallValue != null);

                if (Mail1 != null)
                {
                    var Mail = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Mail1)
                    {
                        Mail.MailTo = item.MemberEmail;

                        if ((item.PriceValue < item.ReachValue && item.NewestValue >= item.ReachValue) || (item.PriceValue > item.ReachValue && item.NewestValue <= item.ReachValue))
                        {
                            sb.Append(string.Format("<p>截至{0}(淨值基準日)止，您關注的「<span style='color:red;'> {1} </span>」最新淨值已達到您設定的目標價格{2}，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.ReachValue));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}", item.ProductName),
                                InfoLink = "",
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                        }
                    }
                    Mail.Topic = $@"【第一銀行 第e理財網】 基金商品最新淨值已達您設定的目標價格！趕快抓住投資機會！🚀 ";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", homeUrl));
                        Mail.Content = sb.ToString();
                        Mails.Add(Mail);
                    }
                }

                if (Mail2 != null)
                {
                    var Mail = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Mail2)
                    {
                        Mail.MailTo = item.MemberEmail;
                        if ((item.PriceValue < item.ReachValue && item.NewestValue >= item.ReachValue) || (item.PriceValue > item.ReachValue && item.NewestValue <= item.ReachValue))
                        {
                            sb.Append(string.Format("<p>截至{0}(收盤價基準日)止，您關注的「<span style='color:red;'> {1} </span>」最新淨值已達到您設定的目標價格{2}，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.ReachValue));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}", item.ProductName),
                                InfoLink = "",
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                        }
                    }
                    Mail.Topic = $@"【第一銀行 第e理財網】ETF/國外股票商品已達您設定的目標價格！趕快抓住投資機會！📈";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", homeUrl));
                        Mail.Content = sb.ToString();
                        Mails.Add(Mail);
                    }
                }

                if (Mail3 != null)
                {
                    var Mail = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Mail3)
                    {
                        Mail.MailTo = item.MemberEmail;
                        if (item.NewestValue >= item.RiseValue)
                        {
                            sb.Append(string.Format("<p>截至{0}(收盤價基準日)止，您關注的「<span style='color:red;'> {1} </span>」漲幅達{2}%，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.RisePercent));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}", item.ProductName),
                                InfoLink = "",
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                        }
                    }
                    Mail.Topic = $@"【第一銀行 第e理財網】 信託商品已達您設定的漲幅囉！";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", homeUrl));
                        Mail.Content = sb.ToString();
                        Mails.Add(Mail);
                    }
                }

                if (Mail4 != null)
                {
                    var Mail = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in Mail4)
                    {
                        Mail.MailTo = item.MemberEmail;
                        if (item.NewestValue <= item.FallValue)
                        {
                            sb.Append(string.Format("<p>截至{0}(收盤價基準日)止，您關注的「<span style='color:red;'> {1} </span>」跌幅達{2}%，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.FallPercent));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}", item.ProductName),
                                InfoLink = "",
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                        }
                    }
                    Mail.Topic = $@"【第一銀行 第e理財網】 信託商品已達您設定的跌幅囉！";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", homeUrl));
                        Mail.Content = sb.ToString();
                        Mails.Add(Mail);
                    }
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
                                }
                                catch (Exception ex)
                                {
                                    Log.Error($"到價通知Mail發送失敗:{ex.Message}");
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

        }
    }
}
