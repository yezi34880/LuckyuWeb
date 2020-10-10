using System;

namespace Luckyu.Cache
{
    /// <summary>
    /// 描 述：定义缓存接口
    /// </summary>
    public interface ICache
    {
        #region Key-Value
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <returns></returns>
        T Read<T>(string cacheKey) where T : class;
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        void Write<T>(string cacheKey, T value) where T : class;
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="expireTime">到期时间</param>
        void Write<T>(string cacheKey, T value, TimeSpan timeSpan) where T : class;
        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        void Remove(string cacheKey);
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        void RemoveAll();
        /// <summary>
        /// 移除全部缓存 但保留登录信息
        /// </summary>
        void RemoveAllwithoutLogin();
        #endregion
    }

}
