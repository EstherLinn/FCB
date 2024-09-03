using Feature.Wealth.ScheduleAgent.Models.Media;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    internal class MediaItemRandomRepository
    {
        private readonly ILoggerService _logger;
        private const string Language = "zh-TW";
        private static Database MasterDB => Sitecore.Configuration.Factory.GetDatabase("master");
        private static Database WebDB => Sitecore.Configuration.Factory.GetDatabase("web");

        public Item JobItem { get; set; }

        public MediaItemRandomRepository(ILoggerService logger)
        {
            this._logger = logger;
        }

        public void Initialize(Item jobItem)
        {
            this.JobItem = jobItem;
        }

        /// <summary>
        /// 變更 Media Item 排序
        /// </summary>
        public void ChangeMediaItemSorting()
        {
            List<Item> items = GenerateDataPool(this.JobItem);
            var mediaItems = GetMediaItemModel(items);
            var randomizedMediaItems = GenerateRandom(mediaItems.ToList());

            var result = randomizedMediaItems?.Select(r =>
            {
                r.SortOrder = randomizedMediaItems.IndexOf(r) + 1;
                return r;
            });

            EditMediaItem(result, MasterDB);
            EditMediaItem(result, WebDB);
        }

        /// <summary>
        /// 產生 Items Pool
        /// </summary>
        /// <param name="settings"></param>
        private List<Item> GenerateDataPool(Item settings)
        {
            var list = new List<Item>();

            try
            {
                using (new LanguageSwitcher(LanguageManager.GetLanguage(Language)))
                {
                    using (new DatabaseSwitcher(MasterDB))
                    {
                        string fieldName = settings[Templates.SourceSetting.Fields.SourceFieldName];
                        string datasourcePath = settings[Templates.SourceSetting.Fields.Datasource];
                        Item datasource = ItemUtils.GetItem(datasourcePath);    // Job Setting Datasource 指向的資料源

                        if (datasource == null)
                        {
                            this._logger.Info("找不到 Datasource。");
                            return null;
                        }

                        if (string.IsNullOrEmpty(fieldName))
                        {
                            this._logger.Info("未設定 Source Field Name。");
                            return null;
                        }

                        string rootItemPath = datasource[fieldName];
                        Item rootItem = ItemUtils.GetItem(rootItemPath);    // 資料源指定的 Root Item

                        if (rootItem == null)
                        {
                            this._logger.Info("找不到 Datasource 指定的 Root Item。");
                            return null;
                        }

                        var children = rootItem.GetChildren();
                        list = children?.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }

            return list;
        }

        /// <summary>
        /// 產生 MediaItemModel
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private IEnumerable<MediaItemModel> GetMediaItemModel(List<Item> children)
        {
            if (children != null && children.Any())
            {
                foreach (Item item in children)
                {
                    int defaultSortOrder = 0;
                    MediaItemModel media = new MediaItemModel
                    {
                        Id = item.ID.ToString(),
                        SortOrder = int.TryParse(item[Sitecore.FieldIDs.Sortorder], out int sortOrder) ? sortOrder : defaultSortOrder
                    };
                    yield return media;
                }
            }
        }

        /// <summary>
        /// 產生隨機排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mediaItems"></param>
        /// <returns></returns>
        private List<T> GenerateRandom<T>(List<T> mediaItems)
        {
            List<T> randomizedMediaItems = new List<T>();

            if (mediaItems == null || !mediaItems.Any())
            {
                return randomizedMediaItems;
            }

            Random rnd = new Random();
            int count = mediaItems.Count;

            for (int i = count - 1 ; i >= 0 ; i--)
            {
                int index = rnd.Next(0, i);
                randomizedMediaItems.Add(mediaItems[index]);
                mediaItems.RemoveAt(index);
            }

            return randomizedMediaItems;
        }

        /// <summary>
        /// 編輯 MediaItem
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="db"></param>
        private void EditMediaItem(IEnumerable<MediaItemModel> collection, Database db)
        {
            if (collection == null || !collection.Any())
            {
                return;
            }

            using (new SecurityDisabler())
            {
                foreach (var model in collection)
                {
                    Item item = db.GetItem(model.Id, LanguageManager.GetLanguage(Language));

                    if (item != null && item.HasContextLanguage())
                    {
                        try
                        {
                            item.Editing.BeginEdit();
                            item[Sitecore.FieldIDs.Sortorder] = Convert.ToString(model.SortOrder * 100);
                            item.Editing.EndEdit();
                        }
                        catch (Exception ex)
                        {
                            item.Editing.CancelEdit();
                            this._logger.Error(ex.Message, ex);
                        }
                    }
                }
            }
        }
    }
}