using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ApiRequestModel
{
    /// <summary>
    /// 用户登录接口实体
    /// </summary>
    public class UserLoginInfo
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
