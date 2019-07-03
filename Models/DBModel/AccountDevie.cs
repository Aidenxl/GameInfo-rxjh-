using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBModel
{
    public class AccountDevie:BaseModel
    {
        public string Account { get; set; }
        public string DeviceCode { get; set; }
    }
}
