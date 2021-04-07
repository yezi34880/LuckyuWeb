using System;
using System.Collections.Generic;

namespace Luckyu.Utility
{
    /// <summary>
    /// 描 述：验证扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 检测空值,为null则抛出ArgumentNullException异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> list)
        {
            return list == null || list.Count < 1;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
                return true;
            return IsEmpty(value.Value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid value)
        {
            if (value == Guid.Empty)
                return true;
            return false;
        }

        public static bool IsEmpty(this DateTime value)
        {
            return value <= LuckyuHelper.MinDate;
        }
        public static bool IsEmpty(this DateTime? value)
        {
            return !value.HasValue || value.Value <= LuckyuHelper.MinDate;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this object value)
        {
            return value == null || string.IsNullOrWhiteSpace(value.ToString());
        }


    }
}
