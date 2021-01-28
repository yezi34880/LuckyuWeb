using Luckyu.Utility;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luckyu.Cache
{
    public class CacheByRedis : ICache
    {
        public static string connectionString = "";

        private ConnectionMultiplexer redis;
        private IDatabase db;

        public CacheByRedis()
        {
            if (connectionString.IsEmpty())
            {
                var conString = AppSettingsHelper.GetConnectionString("Redis");
                if (conString.IsEmpty())
                {
                    throw new Exception("请先调用CacheByRedis.SetConnectString方法设置connectionString");
                }
                connectionString = conString;
            }
            redis = ConnectionMultiplexer.Connect(connectionString);
            db = redis.GetDatabase(0);
        }

        public T Read<T>(string cacheKey) where T : class
        {
            var res = db.StringGet(cacheKey);
            if (!res.HasValue)
            {
                return default(T);
            }
            var result = JsonConvert.DeserializeObject<T>(res);
            return result;
        }

        public void Remove(string cacheKey)
        {
            db.KeyDelete(cacheKey);
        }

        public void RemoveAll()
        {
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            server.FlushDatabase(); // to wipe a single database, 0 by default
            server.FlushAllDatabases(); // to wipe all databases
        }

        public void RemoveAllwithoutLogin()
        {
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            //FlushDatabase didn't work for me: got error admin mode not enabled error
            //server.FlushDatabase();
            var keys = server.Keys();
            var strStar = CacheFactory.CachePrefix() + "login_";
            foreach (var key in keys)
            {
                var k = key.ToString();
                if (k.StartsWith(strStar))
                {
                    continue;
                }
                db.KeyDelete(k);
            }
        }

        public void Write<T>(string cacheKey, T value) where T : class
        {
            var str = JsonConvert.SerializeObject(value);
            db.StringSet(cacheKey, str);
        }

        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan) where T : class
        {
            var str = JsonConvert.SerializeObject(value);
            db.StringSet(cacheKey, str, timeSpan);
        }
    }
}
