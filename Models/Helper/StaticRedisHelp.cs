namespace Models.Helper
{
    /// <summary>
    /// 单例redis对象
    /// </summary>
    public class StaticRedisHelp
    {
        private static RedisHelper RedisHelper { get; set; }
        static StaticRedisHelp()
        {
            //RedisHelper = new RedisHelper(LoginInfo.UserId);
            RedisHelper = new RedisHelper("1209026461");
        }

        /// <summary>
        /// 获得redis连接
        /// </summary>
        /// <returns></returns>
        public static RedisHelper GetRedis()
        {
            return RedisHelper;
        }

        /// <summary>
        /// 重置redis连接
        /// </summary>
        public static void RestRedis()
        {
            //RedisHelper = new RedisHelper(LoginInfo.UserId);
            RedisHelper = new RedisHelper("1209026461");
        }
    }
}
