using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBModel
{
    public class RegCode : BaseModel
    {
        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        public int Term { get; set; }
        /// <summary>
        /// 是否使用0 未使用    1已使用
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        /// 激活类型 0本地版 1网络版
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// 使用者账号或者设备码
        /// </summary>
        public string UseAccountOrMachine { get; set; }
    }
}
