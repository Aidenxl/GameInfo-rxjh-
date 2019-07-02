using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ApiReturnModel
{
    public class ReturnBase<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }

    public class ReturnBase
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
