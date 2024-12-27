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
            string sql = $"Select A.PlatFormId,A.MemberEmail,B.ItemId FROM FCB_Member as A ,[PublishItemRecord] AS B  WHERE A.PlatFormId is not null  AND B.IsSend = @isSend AND B.Type=@type";
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
            //周月報無需mail 但記錄通知
            if (infos == null || !infos.Any())
            {
                _logger.Info("無新的周月報");
                return;
            }
            var GroupByMember = infos.GroupBy(x => x.PlatFormId);
            foreach (var group in GroupByMember)
            {
                List<MailRecord> mailRecords = new List<MailRecord>();
                foreach (var item in group)
                {
                    mailRecords.Add(new MailRecord
                    {
                        PlatFormId = item.PlatFormId,
                        InfoDateTime = DateTime.Now,
                        InfoContent = item.Title,
                        InfoLink = item.Url,
                        MailInfoType = MailInfoTypeEnum.週月報.ToString(),
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
