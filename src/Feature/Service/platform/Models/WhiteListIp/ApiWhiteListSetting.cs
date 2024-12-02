using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Service.Models.WhiteListIp
{
    public class ApiWhiteListSetting
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly string whiteListCacheKey = $"Fcb_WhiteListCache";
        private static readonly string ckeckApiAllowCacheKey = $"Fcb_CkeckApiAllowCache";

        public static List<string> ApiWhiteList()
        {
            // 嘗試從快取中取得白名單數據
            var cacheData = _cache.Get(whiteListCacheKey) as List<string>;

            if (cacheData != null)
            {
                return cacheData;
            }

            // 從資料源獲取白名單數據
            List<string> result = new List<string>();
            Item item = ItemUtils.GetItem(Template.WhiteList.Root);

            // 未上節點，預設返回 null
            if (item == null)
            {
                return null;
            }

            string whiteListString = ItemUtils.GetFieldValue(item, Template.WhiteList.Fields.IPList);

            if (!string.IsNullOrWhiteSpace(whiteListString))
            {
                result = whiteListString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .ToList();

                // 將獲取的白名單數據存入快取，有效期設置為30分鐘
                _cache.Set(whiteListCacheKey, result, DateTimeOffset.Now.AddMinutes(30));
            }

            return result;
        }

        public static bool CkeckApiAllow()
        {
            // 嘗試從快取中取得允許 API 的設置
            var cacheData = _cache.Get(ckeckApiAllowCacheKey) as string;

            if (!string.IsNullOrEmpty(cacheData))
            {
                return cacheData == "1";
            }

            // 從資料源獲取 IP 允許設置
            Item item = ItemUtils.GetItem(Template.WhiteList.Root);

            // 未上節點，預設返回 true
            if (item == null)
            {
                return true;
            }

            string ipAllow = ItemUtils.GetFieldValue(item, Template.WhiteList.Fields.IPAllow);
            bool result = ipAllow == "1";

            // 將 IP 允許設置存入快取，有效期設置為30分鐘
            _cache.Set(ckeckApiAllowCacheKey, ipAllow, DateTimeOffset.Now.AddMinutes(30));

            return result;
        }
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
            }
        }
    }
}
