using System.Security.Cryptography;
using System.Text;

namespace ControleEstoque.Web.Helpers
{
    public class CriptoHelper
    {
        public static string HashMD5(string val)
        {
            var bytes = Encoding.ASCII.GetBytes(val);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(bytes);
            var ret = string.Empty;

            for (int i = 0; i < hash.Length; i++)
            {
                ret += hash[i].ToString("x2");
            }
            return ret;
        }
    }
}