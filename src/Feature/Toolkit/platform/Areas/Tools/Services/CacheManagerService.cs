using Feature.Wealth.Toolkit.Areas.Tools.Models.CacheManager;

namespace Feature.Wealth.Toolkit.Areas.Tools.Services
{
    public class CacheManagerService
    {
        public CacheDataHandler GetHttpRunTimeCache() => new HttpRuntimeCacheHandler();

        public CacheDataHandler GetMemoryCache() => new MemoryCacheHandler();

        public SitecoreCacheHandler GetSitecoreCache() => new();
    }
}