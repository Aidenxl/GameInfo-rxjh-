using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ApiRequestModel
{
    /// <summary>
    /// 获得游戏信息接口实体
    /// </summary>
    public class GetGameInfo
    {
        public string Account { get; set; }
        public string Token { get; set; }
    }
}
