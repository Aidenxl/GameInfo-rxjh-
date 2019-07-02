using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DataInfo
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 进程ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 当前等级
        /// </summary>
        public int CurrentLv { get; set; }
        /// <summary>
        /// 每秒经验
        /// </summary>
        public int MillisecondExperience { get; set; }
        /// <summary>
        /// 每秒金币
        /// </summary>
        public int MillisecondGold { get; set; }
        /// <summary>
        /// 启动时当前经验
        /// </summary>
        public int CurrentExperience { get; set; }
        /// <summary>
        /// 启动时所需要的经验
        /// </summary>
        public int NeedExperience { get; set; }
        /// <summary>
        /// 启动时金币
        /// </summary>
        public int CurrentGold { get; set; }
        /// <summary>
        /// 时间计数
        /// </summary>
        public double TimeCount { get; set; }
    }
}
