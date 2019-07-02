using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// redis帮助类
/// </summary>
namespace Models.Helper
{
    public class RedisHelper
    {
        private readonly static string REDIS_CONN = "118.24.18.189:6379,allowAdmin=true,password=ylj970301";
        private readonly static string REDIS_IP = "118.24.18.189";
        private readonly static int REDIS_PORT = 6379;


        private ConnectionMultiplexer redis = null;
        private IDatabase database = null;
        private IServer server = null;
        private int mydb = 0;
        private static string _defultKey;
        public RedisHelper(string defultKey, int db = 0)
        {
            _defultKey = defultKey;
            Init(db);
        }
        public void Init(int db)
        {
            try
            {
                mydb = db;
                if (redis == null)
                {
                    redis = ConnectionMultiplexer.Connect(REDIS_CONN);
                    database = redis.GetDatabase(mydb);
                    server = redis.GetServer(REDIS_IP, REDIS_PORT);
                    redis.ErrorMessage += Redis_ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器失败！！！,尝试重新连接");
                Init(db);
            }

        }


        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Redis_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //LogHelper.WriteLog("Redis", new Exception(e.Message));
        }
        /// <summary>
        /// 添加 Key 的前缀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AddKeyPrefix(string key)
        {
            return $"{_defultKey}:{key}";
        }
        /// <summary>
        /// 通过key获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            return database.StringGet(key);
        }

        /// <summary>
        /// 新增key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool StringSet(string key, string val, TimeSpan? exp = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            return database.StringSet(key, val, exp);
        }

        /// <summary>
        /// 新增key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool ObjectSet(string key, object val, TimeSpan? exp = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(val);
            return database.StringSet(key, json, exp);
        }

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IEnumerable<RedisKey> LikeKeys(string pattern = "*")
        {
            return server.Keys(database: mydb, pattern: pattern);
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetLikeKeyValue(string pattern = "*")
        {
            IEnumerable<RedisKey> list = LikeKeys(pattern);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    dic.Add(item, StringGet(item));
                }
            }
            return dic;
        }

        /// <summary>
        /// 查询指定主键
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetListKeyValue(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            IEnumerable<RedisValue> list = ListRange(redisKey);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    dic.Add(item, StringGet(item));
                }
            }
            return dic;
        }



        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public IEnumerable<RedisValue> ListRange(string redisKey)
        {
            redisKey = AddKeyPrefix(redisKey);
            return database.ListRange(redisKey);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDelete(string key)
        {
            key = AddKeyPrefix(key);
            return database.KeyDelete(key);
        }

    }
}
