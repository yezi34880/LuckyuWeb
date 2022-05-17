using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class LuckyuHelper
    {
        public static DateTime MinDate = DateTime.Parse("1900-1-1");
        public static DateTime MaxDate = DateTime.Parse("3000-1-1");

        // 系统标识
        public static string AppID = AppSettingsHelper.GetAppSetting("AppID");

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// 获取Type默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
