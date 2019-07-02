using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DBModel
{
    public class LoginInfo : BaseModel
    {
        public string Account { get; set; }
        public DateTime OutTime { get; set; }
        public string Token { get; set; }
    }
}
