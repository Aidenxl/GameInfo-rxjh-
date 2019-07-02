using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ApiReturnModel
{
    public class ReturnLoginInfo
    {
        public string Account { get; set; }

        public string Token { get; set; }

        public DateTime OutTime { get; set; }
    }
}
