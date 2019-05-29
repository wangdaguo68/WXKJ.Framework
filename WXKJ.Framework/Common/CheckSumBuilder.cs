using System.Security.Cryptography;
using System.Text;

namespace OA.Application.Common
{
    /// <summary>
    /// 哈希加密
    /// </summary>
    public static class CheckSumBuilder
    {
        public static string GetCheckSum(string appsecret, string nonce, string curtime)
        {
            return SHA1(appsecret + nonce + curtime);
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <returns>返回40位UTF8 大写</returns>  
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8).ToLower();
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var bytes_in = encode.GetBytes(content);
            var bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            var sb = new StringBuilder();
            foreach (var b in bytes_out)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为小写
            return sb.ToString().ToLower();
        }
    }
}
