using Newtonsoft.Json;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Data.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using SitecoreCaching = Sitecore.Caching;

namespace Feature.Wealth.Toolkit.Areas.Tools.Models.CacheManager
{
    public abstract class CacheDataHandler
    {
        /// <summary>
        /// Get all cache
        /// </summary>
        /// <returns></returns>
        public abstract Caches GetCaches();
        /// <summary>
        /// Get cache via key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public abstract string GetCache(string cacheKey);
        /// <summary>
        /// Remove cache via key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public abstract (bool Success, string Message) RemoveCache(string cacheKey);
        /// <summary>
        /// Remove all cache
        /// </summary>
        /// <returns></returns>
        public abstract (bool Success, string Message) RemoveCaches();

        /// <summary>
        /// 取得快取過期時間
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public abstract DateTime? GetCacheExpiryDateTime(string cacheKey);

        private string DefaultToString(object obj)
        {
            string result;
            try
            {
                if (obj == null)
                {
                    return "Object is Null";
                }

                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    string json = JsonConvert.SerializeObject(obj, new SitecoreCustomConverter());
                    //無公開屬性可供序列化
                    if (json == "{}")
                    {
                        result = obj.ToString();
                    }
                    else
                    {
                        result = json;
                    }
                }
                else
                {
                    result = "Empty String";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        protected string ParseObjectToResult(object cacheObj, string key)
        {
            string result;
            if (cacheObj != null)
            {
                var objType = cacheObj.GetType();

                if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Lazy<>))
                {
                    dynamic obj = cacheObj;
                    if (obj.IsValueCreated)
                    {
                        cacheObj = obj.Value;
                    }
                    else
                    {
                        return "Lazy Object Value is Null";
                    }
                }

                if (objType == typeof(string))
                {
                    return DefaultToString(cacheObj);
                }

                if (objType == typeof(DateTime))
                {
                    return ((DateTime)cacheObj).ToString("yyyy/MM/dd HH:mm:ss");
                }

                if (objType == typeof(XmlDocument))
                {
                    var xml = (XmlDocument)cacheObj;
                    return HttpUtility.HtmlEncode(xml.InnerXml);
                }

                if (objType == typeof(DataTable))
                {
                    var dataTable = (DataTable)cacheObj;
                    return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                }

                if (objType == typeof(Item))
                {
                    if (cacheObj is Item sitecoreItem)
                    {
                        return $"{sitecoreItem.ID}|{sitecoreItem.DisplayName}";
                    }

                    return $"{key} Not Found";
                }

                if (typeof(IEnumerable).IsAssignableFrom(objType))
                {
                    #region List Type

                    if (cacheObj is IEnumerable<object> list)
                    {
                        object firstObj = list.FirstOrDefault();
                        if (firstObj != null)
                        {
                            try
                            {
                                result = JsonConvert.SerializeObject(cacheObj, new SitecoreCustomConverter());
                            }
                            catch
                            {
                                result = DefaultToString(cacheObj);
                            }
                        }
                        else
                        {
                            result = DefaultToString(cacheObj);
                        }
                    }
                    else
                    {
                        result = DefaultToString(cacheObj);
                    }

                    return result;

                    #endregion List Type
                }

                return DefaultToString(cacheObj);
            }

            result = $"Cache key {key} ,Not Exist";
            return result;
        }
    }

    public class HttpRuntimeCacheHandler : CacheDataHandler
    {
        public override Caches GetCaches()
        {
            var result = new List<CacheInfo>();
            var source = HttpRuntime.Cache.GetEnumerator();

            while (source.MoveNext())
            {
                if (source.Key is string key)
                {
                    object cacheObj = HttpRuntime.Cache.Get(key);
                    if (cacheObj != null)
                    {
                        result.Add(new CacheInfo(key, cacheObj.GetType().FullName, GetCacheExpiryDateTime(key)));
                    }
                }
            }

            return new Caches { List = result.OrderBy(k => k.Key.Length) };
        }
        public override string GetCache(string cacheKey) => ParseObjectToResult(HttpRuntime.Cache.Get(cacheKey), cacheKey);
        public override (bool Success, string Message) RemoveCache(string cacheKey)
        {
            try
            {
                object cacheObj = HttpRuntime.Cache.Get(cacheKey);
                if (cacheObj != null)
                {
                    HttpRuntime.Cache.Remove(cacheKey);
                }

                if (HttpRuntime.Cache.Get(cacheKey) == null)
                {
                    return (true, "Success");
                }

                return (false, "Remove Failed");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public override (bool Success, string Message) RemoveCaches()
        {
            var source = HttpRuntime.Cache.GetEnumerator();
            var errors = new List<string>();
            while (source.MoveNext())
            {
                try
                {
                    if (source.Key != null)
                    {
                        HttpRuntime.Cache.Remove(source.Key.ToString());
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Key:{source.Key}，ErrorMessage:{ex.Message}");
                }
            }

            if (errors.Any())
            {
                return (false, string.Join("|", errors));
            }

            return (true, "Success");
        }

        /// <summary>
        /// reference: https://stackoverflow.com/questions/344479/how-can-i-get-the-expiry-datetime-of-an-httpruntime-cache-object#:~:text=Here%27s%20my%20code%20for%20people%20using%20.NET%204.7
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public override DateTime? GetCacheExpiryDateTime(string cacheKey)
        {
            try
            {
                object aspnetCacheStoreProvider = HttpRuntime.Cache.GetType().GetProperty("InternalCache", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(HttpRuntime.Cache, null);
                object intenralCacheStore = aspnetCacheStoreProvider.GetType().GetField("_cacheInternal", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(aspnetCacheStoreProvider);
                Type typeCacheGetOptions = HttpRuntime.Cache.GetType().Assembly.GetTypes().Where(d => d.Name == "CacheGetOptions").FirstOrDefault();
                object entry = intenralCacheStore.GetType().GetMethod("DoGet", BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Any, new[] { typeof(bool), typeof(string), typeCacheGetOptions }, null).Invoke(intenralCacheStore, new Object[] { true, cacheKey, 1 });
                PropertyInfo utcExpiresProperty = entry.GetType().GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
                return (DateTime)utcExpiresProperty.GetValue(entry, null);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class MemoryCacheHandler : CacheDataHandler
    {
        public override Caches GetCaches()
        {
            return new Caches { List = MemoryCache.Default.Select(cache => new CacheInfo(cache.Key, cache.Value.GetType().FullName, GetCacheExpiryDateTime(cache.Key))) };
        }
        public override string GetCache(string cacheKey) => ParseObjectToResult(MemoryCache.Default.Get(cacheKey), cacheKey);
        public override (bool Success, string Message) RemoveCache(string cacheKey)
        {
            try
            {
                var memoryCache = MemoryCache.Default;
                object cacheObj = memoryCache.Get(cacheKey);
                if (cacheObj != null)
                {
                    memoryCache.Remove(cacheKey);
                }

                if (memoryCache.Get(cacheKey) == null)
                {
                    return (true, "Success");
                }

                return (false, "Remove Failed");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public override (bool Success, string Message) RemoveCaches()
        {
            var cache = MemoryCache.Default;
            var allKeys = cache.Select(k => k.Key);
            var errors = new List<string>();
            foreach (string key in allKeys)
            {
                try
                {
                    cache.Remove(key);
                }
                catch (Exception ex)
                {
                    errors.Add($"Key:{key}，ErrorMessage:{ex.Message}");
                }
            }

            var source = HttpRuntime.Cache.GetEnumerator();

            if (errors.Any())
            {
                return (false, string.Join("|", errors));
            }

            return (true, "Success");
        }

        public override DateTime? GetCacheExpiryDateTime(string cacheKey)
        {
            try
            {
                object entry = MemoryCache.Default.GetType().GetMethod("GetEntry", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MemoryCache.Default, new object[] { cacheKey });
                if (entry != null)
                {
                    PropertyInfo utcAbsExpProperty = entry.GetType().GetProperty("UtcAbsExp", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (utcAbsExpProperty != null)
                    {
                        return (DateTime)utcAbsExpProperty.GetValue(entry);
                    }
                }
            }
            catch (Exception)
            {
               // nothing
            }
            return null;
        }
    }

    public class SitecoreCacheHandler
    {
        public Caches GetCaches()
        {
            var caches = SitecoreCaching.CacheManager.GetAllCaches();
            Array.Sort(caches, new CacheComparer());
            var result = caches.Select(cacheInfo => CreateSitecoreCacheInfo(cacheInfo, () => { })).ToList();

            return new Caches
            {
                List = result,
                Entries = SitecoreCaching.CacheManager.GetStatistics().TotalCount,
                TotalSize = MainUtil.FormatSize(SitecoreCaching.CacheManager.GetStatistics().TotalSize),
                TotalMaxSize = MainUtil.FormatSize(result.Sum(info => StringUtil.ParseSizeString(info.MaxSize))),
                TotalCaches = result.Count
            };
        }
        public string GetCache(string cacheKey)
        {
            //TODO: 未完成抓單一緩存
            var cache = SitecoreCaching.CacheManager.GetAllCaches().SingleOrDefault(c => c.Name == cacheKey);
            if (cache is ICache aiCache)
            {
                string[] keys = aiCache.GetCacheKeys();
                foreach (string key in keys) { }

                string message = $@"Showing Total <strong>{aiCache.Count}</strong> Cache entries for Key <strong>{cacheKey}</strong>";
            }
            else
            {
                string message = "We only show cache values which are of string type. But you can still clear that particular cache from this page.";
            }

            return "";
        }

        public Caches RemoveCache(string key)
        {
            var caches = SitecoreCaching.CacheManager.GetAllCaches();
            Array.Sort(caches, new CacheComparer());

            var result = caches.Select(cache => CreateSitecoreCacheInfo(cache, () =>
            {
                if (cache.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    cache.Clear();
                }
            })).ToArray();

            return new Caches
            {
                List = result,
                TotalMaxSize = MainUtil.FormatSize(result.Sum(info => StringUtil.ParseSizeString(info.MaxSize))),
                TotalCaches = result.Count()
            };
        }
        public Caches RemoveCaches()
        {
            SitecoreCaching.CacheManager.ClearAllCaches();
            return GetCaches();
        }

        private static CacheInfo CreateSitecoreCacheInfo(ICacheInfo cache, Action action)
        {
            action();

            long size = cache.Size;
            long maxSize = cache.MaxSize;
            double thresholdValue = 0;
            if (maxSize > 0)
            {
                thresholdValue = size / (double)maxSize * 100;
            }

            string severityLevel;
            string description = "N/A";
            switch (thresholdValue)
            {
                // If ThresholdValue is grater than 80%
                // It's an ALERT
                case > 80:
                    severityLevel = "Alert";
                    description = @$"Time to tune this cache! Reason : 80% exceeded.
                            New Cache Size should be (following 50 % Increment rule) : {MainUtil.FormatSize(maxSize + maxSize * 50 / 100)}.
                            % of Usage : {thresholdValue}";
                    break;
                case >= 50:
                    severityLevel = "Warning";
                    description = @"50% of this cache is being utilized. Its not a big reason to worry. But good to keep an eye on this.";
                    break;
                default:
                    severityLevel = "Normal";
                    break;
            }

            return new CacheInfo
            {
                Enable = cache.Enabled,
                Name = cache.Name,
                Count = cache.Count,
                Size = MainUtil.FormatSize(cache.Size),
                MaxSize = MainUtil.FormatSize(cache.MaxSize),
                SeverityLevel = severityLevel,
                Description = description
            };
        }
    }
}