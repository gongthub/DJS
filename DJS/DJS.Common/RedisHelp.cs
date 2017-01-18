using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class RedisHelp
    {
        /// <summary>
        /// redis服务器地址
        /// </summary>
        private static string redisServer = ConfigHelp.redisServerPath;
        
        /// <summary>
        /// redis服务器端口
        /// </summary>
        private static int redisServerPort = Convert.ToInt32(ConfigHelp.redisServerPortPath);
          

        #region 单例模式创建对象
        //单例模式创建对象
        private static RedisHelp _redisHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        RedisHelp()
        {
        }

        public static RedisHelp redisHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _redisHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _redisHelp)
                        {
                            _redisHelp = new RedisHelp(); 
                        }
                    }
                }
                return _redisHelp;
            }
        }
        #endregion

        #region 添加一个值 +bool Add<T>(string key, T value)
        /// <summary>
        /// 添加一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add<T>(string key, T value)
        {
            bool ret = true;
            using (RedisClient redisClient=new RedisClient())
            {
                ret = redisClient.Add<T>(key, value);
            }
            return ret;
        } 
        #endregion
         
        #region 设置一个值 +bool Set<T>(string key, T value)
        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            bool ret = true;
            using (RedisClient redisClient = new RedisClient())
            {
                ret = redisClient.Set<T>(key, value);
            }
            return ret;
        } 
        #endregion
         
        #region 获取一个值 +T Get<T>(string key)
        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            using (RedisClient redisClient = new RedisClient())
            {
                return redisClient.Get<T>(key);
            }
        }
        #endregion

        #region 删除一个值 +long Del(string key)
        /// <summary>
        /// 删除一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Del(string key)
        {
            using (RedisClient redisClient = new RedisClient())
            {
                return redisClient.Del(key);
            }
        }
        #endregion
           
        #region 删除一个值 +long Del(string key)
        /// <summary>
        /// 删除一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long DelById<T>(string key)
        { 

            using (RedisClient redisClient = new RedisClient())
            {
                return redisClient.Del(key);
            }
        }
        #endregion



    }
}
