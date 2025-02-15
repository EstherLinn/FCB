﻿using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Service.Models.WhiteListIp
{
    public class ApiWhiteListSetting
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly string whiteListCacheKey = $"Fcb_WhiteListCache";

        public static WhiteListModel GetOrSetWhiteListCache()
        {
            var cacheData = _cache.Get(whiteListCacheKey) as WhiteListModel;
            if (cacheData != null)
            {
                return cacheData;
            }

            Item item = ItemUtils.GetItem(Template.WhiteList.Root);

            // 未上節點，預設返回空白名單和未啟用白名單功能
            if (item == null)
            {
                return new WhiteListModel
                {
                    WhiteList = new List<string>(),
                    IpIsAllow = false,
                    DisableLog = false,
                };
            }

            string whiteListString = ItemUtils.GetFieldValue(item, Template.WhiteList.Fields.IPList);
            var whiteList = new List<string>();

            if (!string.IsNullOrWhiteSpace(whiteListString))
            {
                whiteList = whiteListString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim())
                                            .ToList();
            }

            bool ipAllow = ItemUtils.IsChecked(item, Template.WhiteList.Fields.IPAllow);
            bool disableLog = ItemUtils.IsChecked(item, Template.WhiteList.Fields.DisableLog);

            // 存入快取，有效期設置為30分鐘
            var cacheEntry = new WhiteListModel
            {
                WhiteList = whiteList,
                IpIsAllow = ipAllow,
                DisableLog = disableLog
            };
            _cache.Set(whiteListCacheKey, cacheEntry, DateTimeOffset.Now.AddMinutes(30));

            return cacheEntry;
        }

        public static List<string> ApiWhiteList() => GetOrSetWhiteListCache().WhiteList;

        public static bool CkeckApiAllow() => GetOrSetWhiteListCache().IpIsAllow;

        public static void Log(LogLevel level, string message, string ip)
        {
            if (GetOrSetWhiteListCache().DisableLog)
            {
                return;
            }

            string logMessage = $"[AuthorizationFilter] {message} IP: {ip}";

            switch (level)
            {
                case LogLevel.Info:
                    Logger.Api.Info(logMessage);
                    break;
                case LogLevel.Warn:
                    Logger.Api.Warn(logMessage);
                    break;
                default:
                    Logger.Api.Info(logMessage);
                    break;
            }
        }
    }

    public class WhiteListModel
    {
        public bool IpIsAllow { get; set; }
        public bool DisableLog { get; set; }
        public List<string> WhiteList { get; set; }
    }

    public enum LogLevel
    {
        Info,
        Warn,
    }

    public struct Template
    {
        public struct WhiteList
        {
            public static readonly ID Root = new ID("{A83635DC-5D9F-46AC-842B-97B5629A8CCE}");

            public struct Fields
            {
                /// <summary>
                /// IP白名單
                /// </summary>
                public static readonly string IPList = "{196B6280-ED1C-45FD-9CD7-E97B9E2F9F7F}";

                public static readonly string IPAllow = "{A8E18B9D-E778-41F0-9028-0460CB9D930C}";

                public static readonly string DisableLog = "{E4613237-6F24-4FE0-A87B-E39AB8C86B6A}";

            }
        }
    }
}