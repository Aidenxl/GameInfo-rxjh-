using Models.Helper;

namespace Models
{
    public class ComputeRegistrationCode
    {
        public static string Compute(string Machinecode)
        {
            return Md5Help.MD5Encrypt32(Machinecode + "1209026461");
        }
    }
}
