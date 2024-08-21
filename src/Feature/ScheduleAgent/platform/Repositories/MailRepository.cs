using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Schedules.Mail;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Sitecore.Configuration;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Data.Items;
using FluentFTP.Helpers;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class MailRepository : IMailInfo<MemebrReachInfo>, IMailRecord<MailRecord>
    {
        private readonly ILoggerService _logger;
        private readonly ID memberRelatedRoot = new ID("{1D504FEF-3A5D-41A5-AFFF-96555715C615}");
        private readonly ID focusListLink = new ID("{4DD89F12-BD12-4B73-9001-1231EFB8078C}");
        public MailRepository(ILoggerService logger)
        {
            this._logger = logger;
        }
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

        public void SendMail(IEnumerable<MemebrReachInfo> memebrReachInfos, Item settings)
        {
            if (memebrReachInfos == null || !memebrReachInfos.Any())
            {
                _logger.Info("無新的到價通知");
                return;
            }
            var groupByMember = memebrReachInfos.GroupBy(x => x.PlatFormId);
            List<MailSchema> mails = new List<MailSchema>();
            var homeUrl = $"https://";
            var cdHostName = Settings.GetSetting("CDHostName");
            homeUrl += cdHostName;
            string focusUrl;
            using (new LanguageSwitcher("zh-TW"))
            {
                Database db = Database.GetDatabase("web");
                focusUrl = ItemUtils.GeneralLink(db.GetItem(memberRelatedRoot), focusListLink)?.Url;
                focusUrl = new Uri(focusUrl).AbsolutePath.ToString();
            }
            foreach (var group in groupByMember)
            {
                //過濾無email之會員
                if (string.IsNullOrEmpty(group.First().MemberEmail))
                {
                    continue;
                }
                List<MailRecord> mailRecords = new List<MailRecord>();
                var mail1 = group.Where(x => x.InfoType == "1" && x.InvestType.ToLower() == "fund");//基金目標價格
                var mail2 = group.Where(x => x.InfoType == "1" && x.InvestType.ToLower() != "fund");//etf、國外股票收盤價
                var mail3 = group.Where(x => x.InfoType == "2" && x.RiseValue != null);//全部商品漲幅
                var mail4 = group.Where(x => x.InfoType == "2" && x.FallValue != null);//全部商品跌幅

                if (mail1 != null)
                {
                    var mailSchema = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    mailSchema.MailTo = mail1.First().MemberEmail;
                    foreach (var item in mail1)
                    {
                        //設定時的產品價格<設定之到價價格 && 最新淨值 >=設定之到價價格 || 設定時的產品價格>設定之到價價格 && 最新淨值 <=設定之到價價格
                        if ((item.PriceValue < item.ReachValue && item.NewestValue >= item.ReachValue) || (item.PriceValue > item.ReachValue && item.NewestValue <= item.ReachValue))
                        {
                            sb.Append(string.Format("<p>截至{0}(淨值基準日)止，您關注的「<span style='color:red;'> {1} </span>」最新淨值已達到您設定的目標價格{2}，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.ReachValue));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}已達到您設定的目標價格囉 !", item.ProductName),
                                InfoLink = string.Format("{0}?id={1}", focusUrl, item.InvestId),
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                            sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", string.Format("{0}?id={1}", homeUrl + focusUrl, item.InvestId)));
                        }
                    }
                    mailSchema.Topic = $@"【第一銀行 第e理財網】 基金商品最新淨值已達您設定的目標價格！趕快抓住投資機會！🚀 ";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        mailSchema.Content = sb.ToString();
                        mails.Add(mailSchema);
                    }
                }

                if (mail2 != null)
                {
                    var mailSchema = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    mailSchema.MailTo = mail1.First().MemberEmail;
                    foreach (var item in mail2)
                    {
                        //設定時的產品價格<設定之到價價格 && 最新淨值 >=設定之到價價格 || 設定時的產品價格>設定之到價價格 && 最新淨值 <=設定之到價價格
                        if ((item.PriceValue < item.ReachValue && item.NewestValue >= item.ReachValue) || (item.PriceValue > item.ReachValue && item.NewestValue <= item.ReachValue))
                        {
                            sb.Append(string.Format("<p>截至{0}(收盤價基準日)止，您關注的「<span style='color:red;'> {1} </span>」收盤價已達到您設定的目標價格{2}，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestId + item.ProductName, item.ReachValue));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0} 收盤價已達到您設定的目標價格囉 !", item.ProductName),
                                InfoLink = string.Format("{0}?id={1}", focusUrl, item.InvestId),
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                            sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", string.Format("{0}?id={1}", homeUrl + focusUrl, item.InvestId)));
                        }
                    }
                    mailSchema.Topic = $@"【第一銀行 第e理財網】ETF/國外股票商品已達您設定的目標價格！趕快抓住投資機會！📈";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        mailSchema.Content = sb.ToString();
                        mails.Add(mailSchema);
                    }
                }

                if (mail3 != null)
                {
                    var mailSchema = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    mailSchema.MailTo = mail1.First().MemberEmail;
                    foreach (var item in mail3)
                    {
                        //最新淨值or開盤價>=設定之漲幅價格
                        if (item.NewestValue >= item.RiseValue)
                        {
                            sb.Append(string.Format("<p>截至{0}({1}基準日)止，您關注的「<span style='color:red;'> {2} </span>」漲幅達{3}%，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestType.ToLower() == "fund" ? "淨值" : "收盤價", item.InvestId + item.ProductName, item.RisePercent));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}已達您設定的漲幅囉！", item.ProductName),
                                InfoLink = string.Format("{0}?id={1}", focusUrl, item.InvestId),
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                            sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", string.Format("{0}?id={1}", homeUrl + focusUrl, item.InvestId)));
                        }
                    }
                    mailSchema.Topic = $@"【第一銀行 第e理財網】 信託商品已達您設定的漲幅囉！";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        mailSchema.Content = sb.ToString();
                        mails.Add(mailSchema);
                    }
                }

                if (mail4 != null)
                {
                    var mailSchema = new MailSchema();
                    StringBuilder sb = new StringBuilder();
                    mailSchema.MailTo = mail1.First().MemberEmail;
                    foreach (var item in mail4)
                    {
                        //最新淨值or開盤價<=設定之跌幅價格
                        if (item.NewestValue <= item.FallValue)
                        {
                            sb.Append(string.Format("<p>截至{0}({1}基準日)止，您關注的「<span style='color:red;'> {2} </span>」跌幅達{3}%，若欲調整相關通知，請登入第e理財進行操作，感謝您的配合，謝謝</p>", item.NewestDate, item.InvestType.ToLower() == "fund" ? "淨值" : "收盤價", item.InvestId + item.ProductName, item.FallPercent));
                            mailRecords.Add(new MailRecord
                            {
                                PlatFormId = item.PlatFormId,
                                InfoDateTime = DateTime.Now,
                                InfoContent = string.Format("{0}已達您設定的跌幅囉！", item.ProductName),
                                InfoLink = string.Format("{0}?id={1}", focusUrl, item.InvestId),
                                MailInfoType = MailInfoTypeEnum.到價通知.ToString(),
                                HaveRead = false
                            });
                            sb.Append(string.Format("<p>第e理財網連結：<a href='{0}' target='_blank' style='color:red;'>{0}</a></p>", string.Format("{0}?id={1}", homeUrl + focusUrl, item.InvestId)));
                        }
                    }
                    mailSchema.Topic = $@"【第一銀行 第e理財網】 信託商品已達您設定的跌幅囉！";
                    if (sb.Length != 0)
                    {
                        sb.Insert(0, "<p>親愛的客戶您好：</p><p>第一銀行提醒您，</p>");
                        mailSchema.Content = sb.ToString();
                        mails.Add(mailSchema);
                    }
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
                                    client.Send(message);
                                    _logger.Error($"到價通知Mail發送To:{item.MailTo}");
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error($"到價通知Mail發送失敗:{ex.ToString}");
                                    //發信有問題，不記錄到table
                                    string[] topicWord = ["收盤價", "漲幅", "跌幅"];
                                    if (item.Topic.Contains("基金商品最新淨值"))
                                    {
                                        mailRecords.RemoveAll(x => !x.InfoContent.ContainsAny(topicWord));
                                    }
                                    else if (item.Topic.Contains("ETF/國外股票"))
                                    {
                                        mailRecords.RemoveAll(x => x.InfoContent.Contains("收盤價"));
                                    }
                                    else if (item.Topic.Contains("漲幅"))
                                    {
                                        mailRecords.RemoveAll(x => x.InfoContent.Contains("漲幅"));

                                    }
                                    else if (item.Topic.Contains("跌幅"))
                                    {
                                        mailRecords.RemoveAll(x => x.InfoContent.Contains("跌幅"));
                                    }
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
