using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.ApiReturnModel
{
    public class ReturnGameInfo
    {
        public int Status { get; set; }
        public GameInfoModel GameInfoModel { get; set; }
    }
}
