using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;


namespace EyouSoft.Cache.Facade
{
    public class CacheRefreshAction : ICacheItemRefreshAction
    {
        public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
        {
            // Item has been removed from cache. Perform desired actions here, based upon
            // the removal reason (e.g. refresh the cache with the item).
        }
    }

    #region 带时间截缓存
    /// <summary>
    /// 带时间截缓存
    /// </summary>
    /// <typeparam name="T">缓存对象类型</typeparam>
    [Serializable]
    public class EyouSoftCacheTime<T>
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        public T Data
        {
            get;
            set;
        }
        /// <summary>
        /// 时间截
        /// </summary>
        public DateTime UpdateTime
        {
            get;
            set;
        }
    }
    #endregion

    #region 系统缓存
    /// <summary>
    /// 系统缓存
    /// </summary>
    public class EyouSoftCache
    {
        private static ICacheManager instance = null;
        private static object lockHelper = new object();

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, object value)
        {
            GetCacheService().Add(key, value);
        }
        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AbsoluteTime"></param>
        public static void Add(string key, object value, DateTime AbsoluteTime)
        {
            GetCacheService().Add(key, value, new CacheItemPriority(), null, new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(AbsoluteTime));
        }
        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="RefreshAction"></param>
        /// <param name="AbsoluteTime"></param>
        public static void Add(string key, object value, CacheRefreshAction RefreshAction, DateTime AbsoluteTime)
        {
            GetCacheService().Add(key, value, new CacheItemPriority(), RefreshAction, new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(AbsoluteTime));
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return GetCacheService().GetData(key);
        }
        /// <summary>
        /// 清除缓存对象
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            GetCacheService().Remove(key);
        }
        /// <summary>
        /// 单体模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        private static ICacheManager GetCacheService()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = CacheFactory.GetCacheManager("SysCache");
                    }
                }
            }
            return instance;
        }

    }
    #endregion

    #region memcached
    /// <summary>
    /// memcached
    /// </summary>
    public class Memcache
    {
        private static ICacheManager instance = null;
        private static object lockHelper = new object();

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, object value)
        {
            GetCacheService().Add(key, value);
        }
        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AbsoluteTime"></param>
        public static void Add(string key, object value, DateTime AbsoluteTime)
        {
            GetCacheService().Add(key, value, new CacheItemPriority(), null, new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(AbsoluteTime));
        }
        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="RefreshAction"></param>
        /// <param name="AbsoluteTime"></param>
        public static void Add(string key, object value, CacheRefreshAction RefreshAction, DateTime AbsoluteTime)
        {
            GetCacheService().Add(key, value, new CacheItemPriority(), RefreshAction, new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(AbsoluteTime));
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return GetCacheService().GetData(key);
        }
        /// <summary>
        /// 清除缓存对象
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            GetCacheService().Remove(key);
        }
        /// <summary>
        /// 单体模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        private static ICacheManager GetCacheService()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = CacheFactory.GetCacheManager("Memcache");
                    }
                }
            }
            return instance;
        }
    }
    #endregion
}
