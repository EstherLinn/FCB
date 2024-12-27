using Feature.Wealth.ScheduleAgent.Models.Mail;
using Feature.Wealth.ScheduleAgent.Schedules.Mail;
using Foundation.Wealth.Manager;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    internal class DiscountActivityRespository : IMailInfo<MemberRecordItemModel>, IMailRecord<MailRecord>
    {
        private readonly ILoggerService _logger;
        private readonly ID titleField = new ID("{42526CEA-3641-467D-B0FE-222ADF528CE8}");
        private readonly ID linkField = new ID("{BEB152FF-A1C5-465D-ABB1-4B944171AE05}");
        public DiscountActivityRespository(ILoggerService logger)
        {
            this._logger = logger;
        }
        public IEnumerable<MemberRecordItemModel> GetDiscounts()
        {
            List<MemberRecordItemModel> DiscountItems = new List<MemberRecordItemModel>();
            string sql = $"Select A.PlatFormId,A.MemberEmail,B.ItemId FROM FCB_Member as A ,[PublishItemRecord] AS B  WHERE A.PlatFormId is not null and  B.IsSend = @isSend AND B.Type=@type";
            var para = new { isSend = false, type = MailInfoTypeEnum.優惠活動.ToString() };
            DiscountItems = DbManager.Custom.ExecuteIList<MemberRecordItemModel>(sql, para, commandType: CommandType.Text)?.ToList();
            if (DiscountItems.Any() && !string.IsNullOrEmpty(DiscountItems.FirstOrDefault()?.ItemId))
            {
                using (new LanguageSwitcher("zh-TW"))
                {
                    Database db = Database.GetDatabase("web");
                    foreach (var item in DiscountItems)
                    {
                        var targetItem = db.GetItem(item.ItemId);
                        if (targetItem != null && !string.IsNullOrEmpty(ItemUtils.GetFieldValue(targetItem, titleField)))
                        {
                            item.Title = ItemUtils.GetFieldValue(targetItem, titleField);
                            item.Url = ItemUtils.GeneralLink(targetItem, linkField).Url;
                            if (!string.IsNullOrEmpty(item.Url))
                            {
                                Uri originalUri = new Uri(item.Url);
                                item.Url = originalUri.AbsolutePath.ToString();
                            }
                        }
                    }
                }
            }
            return DiscountItems;
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
            //優惠活動無需mail 但記錄通知
            if (infos == null || !infos.Any())
            {
                _logger.Info("無新的優惠活動");
                return;
            }
            var GroupByMember = infos.GroupBy(x => x.PlatFormId);

            foreach (var group in GroupByMember)
            {
                if (string.IsNullOrEmpty(group.First().Title))
                {
                    continue;
                }
                List<MailRecord> mailRecords = new List<MailRecord>();
                foreach (var item in group)
                {
                    mailRecords.Add(new MailRecord
                    {
                        PlatFormId = item.PlatFormId,
                        InfoDateTime = DateTime.Now,
                        InfoContent = item.Title,
                        InfoLink = item.Url,
                        MailInfoType = MailInfoTypeEnum.優惠活動.ToString(),
                        HaveRead = false
                    });
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
