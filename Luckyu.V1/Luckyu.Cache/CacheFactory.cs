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
                return new CacheByMemory();
            }
        }

        /// <summary>
        /// 多缓存 缓存后缀 先留着
        /// </summary>
        /// <returns></returns>
        public static string CachePrefix()
        {
            return LuckyuHelper.AppID ;
        }
    }
}
