using Sitecore;
using System;
using System.Collections.Generic;

namespace Feature.Wealth.Toolkit.Areas.Tools.Models.CacheManager
{
    public class CacheInfo
    {
        public CacheInfo() { }
        public CacheInfo(string key, string type, DateTime? expireDate)
        {
            Key = key;
            Type = type;
            ExpireDate = expireDate.HasValue ? DateUtil.ToServerTime(expireDate.Value).ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;
        }

        public string Key { get; set; }
        public string Type { get; set; }
        public string ExpireDate { get; set; }

        #region for Sitecore

        public bool Enable { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Size { get; set; }
        public string MaxSize { get; set; }
        public string SeverityLevel { get; set; }
        public string Description { get; set; }

        #endregion
    }

    public class Return
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Env => Environment.MachineName;
        public string Time => DateUtil.ToServerTime(DateTime.UtcNow).ToString("yyyy/MM/dd HH:mm:ss");
        public Caches Caches { get; set; }
    }

    public class Caches
    {
        public IEnumerable<CacheInfo> List { get; set; }

        /// <summary>
        /// Cache 總項目
        /// </summary>
        public long Entries { get; set; }

        /// <summary>
        /// 使用大小
        /// </summary>
        public string TotalSize { get; set; }

        public string TotalMaxSize { get; set; }

        /// <summary>
        /// Cache
        /// </summary>
        public int TotalCaches { get; set; }
    }
}