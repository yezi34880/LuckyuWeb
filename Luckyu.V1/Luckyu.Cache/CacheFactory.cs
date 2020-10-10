using Luckyu.Utility;

namespace Luckyu.Cache
{
    public class CacheFactory
    {
        public static ICache Create()
        {
            var config = AppSettingsHelper.GetAppSetting("CacheMode");
            if (config == "redis")
            {
                return new CacheByRedis();
            }
            else
            {
                return CacheByMemory.GetInstance();
            }
        }

        /// <summary>
        /// 多域名 缓存后缀 先留着
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomain()
        {
            return "";
        }
    }
}
