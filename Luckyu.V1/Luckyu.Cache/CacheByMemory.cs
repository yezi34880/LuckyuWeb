using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Cache
{
    public class CacheByMemory : ICache
    {
        private static MemoryCache myCache;

        public static CacheByMemory GetInstance()
        {
            if (myCache == null)
            {
                MemoryCacheOptions cacheOps = new MemoryCacheOptions()
                {
                    //##注意netcore中的缓存是没有单位的，缓存项和缓存的相对关系
                    //缓存满了时，压缩20%（即删除20份优先级低的缓存项）
                    CompactionPercentage = 0.2,
                    //五秒钟查找一次过期项
                    ExpirationScanFrequency = TimeSpan.FromSeconds(5)
                };
                myCache = new MemoryCache(cacheOps);
            }
            return new CacheByMemory();
        }

        //缓存的配置
        public T Read<T>(string cacheKey) where T : class
        {
            var result = myCache.Get<T>(cacheKey);
            return result;
        }

        public void Remove(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));
            myCache.Remove(cacheKey);
        }

        public void RemoveAll()
        {
            var allKeys = GetCacheKeys();
            foreach (var key in allKeys)
            {
                Remove(key);
            }
        }

        public void RemoveAllwithoutLogin()
        {
            var allKeys = GetCacheKeys();
            var strStar = CacheFactory.GetCurrentDomain() + "luckyu_login_";
            foreach (var key in allKeys)
            {
                if (key.StartsWith(strStar))
                {
                    continue;
                }
                Remove(key);
            }

        }

        public void Write<T>(string cacheKey, T value) where T : class
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            myCache.Set<T>(cacheKey, value, TimeSpan.FromDays(1));
        }

        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan) where T : class
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            myCache.Set<T>(cacheKey, value, timeSpan);
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = myCache.GetType().GetField("_entries", flags).GetValue(myCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
