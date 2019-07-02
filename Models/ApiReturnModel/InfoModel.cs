using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ApiReturnModel
{
    /// <summary>
    /// 挂机详情信息
    /// </summary>
    public class InfoModel
    {
        /// <summary>
        /// 当前经验
        /// </summary>
        public int CurrentExperience { get; set; }

        /// <summary>
        /// 当前金币
        /// </summary>
        public int CurrentGold { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 需要经验
        /// </summary>
        public int NeedExperience { get; set; }

        /// <summary>
        /// 武勋
        /// </summary>
        public int WX { get; set; }

        /// <summary>
        /// 经验比例
        /// </summary>
        public string ExperienceRatio { get; set; }

        /// <summary>
        /// 每分钟经验
        /// </summary>
        public int MinutesExperience { get; set; }

        /// <summary>
        /// 每分钟金币
        /// </summary>
        public int MinutesGold { get; set; }

        /// <summary>
        /// 预计升级时间(秒)
        /// </summary>
        public int UpGradeTime { get; set; }

        /// <summary>
        /// 本次挂机登录时间
        /// </summary>
        public double LoginTime { get; set; }
    }
}
