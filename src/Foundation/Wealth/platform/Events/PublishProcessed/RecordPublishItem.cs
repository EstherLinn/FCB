﻿using Foundation.Wealth.Manager;
using Sitecore.Data;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Foundation.Wealth.Events.PublishProcessed
{
    public class RecordPublishItem
    {
        private const string _WeekMonthlyNewsTemplatedId = "{192BE530-AE32-4B53-A3DF-DE92987C4D7C}";
        private const string _PromotionTemplatedId = "{DB64A2B5-C321-447B-A98C-57E2E8A3B65A}";
        private readonly ID promotionTitleField = new ID("{42526CEA-3641-467D-B0FE-222ADF528CE8}");
        private readonly ID weekMonthlyTitleField = new ID("{D5501C04-3956-4A2F-AE0D-00DEB77FA4B5}");

        public void ItemProcessed(object sender, EventArgs args)
        {
            var itemProcessedEventArgs = args as ItemProcessedEventArgs;
            if (itemProcessedEventArgs != null)
            {
                if (!itemProcessedEventArgs.Context.PublishHelper.TargetItemExists(itemProcessedEventArgs.Context.ItemId))
                {
                    return;
                }
                var processedItem = itemProcessedEventArgs.Context.PublishHelper.GetTargetItem(itemProcessedEventArgs.Context.ItemId);
                if (processedItem != null && processedItem.Language.ToString() == "zh-TW")
                {
                    if (processedItem.TemplateID == new ID(_WeekMonthlyNewsTemplatedId) || processedItem.TemplateID == new ID(_PromotionTemplatedId))
                    {
                        var title = processedItem.TemplateID == new ID(_WeekMonthlyNewsTemplatedId) ? ItemUtils.GetFieldValue(processedItem, weekMonthlyTitleField) : ItemUtils.GetFieldValue(processedItem, promotionTitleField);
                        if (string.IsNullOrEmpty(title))
                        {
                            return;
                        }
                        var itemId = processedItem.ID.ToString();
                        var type = processedItem.TemplateID == new ID(_WeekMonthlyNewsTemplatedId) ? "週月報" : "優惠活動";
                        string strSql = $@"MERGE PublishItemRecord AS Target
                            USING (SELECT @ItemId) AS Source (ItemId) ON Target.ItemId = Source.ItemId
                            WHEN NOT MATCHED THEN
                            INSERT (ItemId,Title,Type,IsSend) values (@ItemId,@Title,@Type,@IsSend);";
                        var para = new
                        {
                            ItemId = itemId,
                            Title = title,
                            Type = type,
                            IsSend = false
                        };
                        var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                        Logger.Database.Info($"{type} 發布項目記錄 Guid={itemId},記錄到Table {affectedRows != 0}");
                    }
                }
            }
        }
    }
}
