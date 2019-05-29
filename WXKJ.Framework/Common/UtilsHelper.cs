using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using WXKJ.Framework.Helpers;
using Convert = System.Convert;
using Random = System.Random;
using Regex = System.Text.RegularExpressions.Regex;
using String = System.String;
using Thread = System.Threading.Thread;

namespace WXKJ.Framework.Common
{
    public static class Utils
    {
        public const int PageSize = 20;

        /// <summary>
        /// 统计分页总数
        /// </summary>
        /// <returns>The paged count.</returns>
        /// <param name="itemCount">Item count.</param>
        public static int TotalPagedCount(int itemCount)
        {
            return (itemCount - 1) / PageSize + 1;
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <returns>The Enum descripttion.</returns>
        /// <param name="enumItemName">Enum item name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string EnumDescription<T>(this T enumItemName)
        {
            try
            {
                FieldInfo fi = enumItemName.GetType()
                    .GetField(enumItemName.ToString());
                if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                        is DescriptionAttribute[] attributes &&
                    attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return enumItemName.ToString();
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 模型验证
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string ModelStateMessage(this ModelStateDictionary modelState)
        {
            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
            var sb = new StringBuilder();
            foreach (var item in errorFieldsAndMsgs)
            {
                //获取键
                var fieldKey = item.Key;
                sb.Append($",'{fieldKey}'");
            }

            sb.Append("参数不正确！");

            return sb.ToString().Substring(1);
        }

        /// <summary>
        /// Stream 转换 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(Stream stream)
        {
            // 64K 缓存
            var buffer = new byte[64 * 1024];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
        /// <summary>
        /// stream转文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public static void StreamToFile(Stream stream, string fileName)

        {

            // 把 Stream 转换成 byte[] 

            var bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 

            var fs = new FileStream(fileName, FileMode.Create);

            var bw = new BinaryWriter(fs);

            bw.Write(bytes);

            bw.Close();

            fs.Close();

        }
        /// <summary>
        /// 文件转stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream FileToStream(string fileName)

        {

            // 打开文件 

            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[] 

            var bytes = new byte[fileStream.Length];

            fileStream.Read(bytes, 0, bytes.Length);

            fileStream.Close();

            // 把 byte[] 转换成 Stream 

            Stream stream = new MemoryStream(bytes);

            return stream;

        }
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>

        public static byte[] StreamToBytes(Stream stream)

        {

            var bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }


        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        public static Stream BytesToStream(byte[] bytes)

        {

            Stream stream = new MemoryStream(bytes);

            return stream;

        }
        /// <summary>
        /// 校验文件MD5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMD5OfString(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (var t in retVal)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 通过文件二进制长度计算文件大小
        /// </summary>
        /// <returns>The file size.</returns>
        /// <param name="length">Length.</param>
        public static string GetFileSize(long length)
        {
            var fileSize = "";
            if (length < 1024.00)
            {
                fileSize = length.ToString("F1") + " Byte";
            }
            else if (length >= 1024.00 && length < 1048576)
            {
                fileSize = (length / 1024.00).ToString("F1") + " K";
            }
            else if (length >= 1048576 && length < 1073741824)
            {
                fileSize = (length / 1024.00 / 1024.00).ToString("F1") + " M";
            }
            else if (length >= 1073741824)
            {
                fileSize = (length / 1024.00 / 1024.00 / 1024.00).ToString("F1") + " G";
            }

            return fileSize;
        }
        /// <summary>
        /// 对密码进行MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            var cl = password;
            //string pwd = "";
            var md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            var s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        /// <summary>  
        /// 获取当前使用的IP  
        /// </summary>  
        /// <returns></returns>  
        public static string GetLocalIP()
        {
            var result = RunApp("route", "print", true);
            var m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try
                {
                    var c = new System.Net.Sockets.TcpClient();
                    c.Connect("www.baidu.com", 80);
                    var ip = ((IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
                    c.Close();
                    return ip;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>  
        /// 获取本机主DNS  
        /// </summary>  
        /// <returns></returns>  
        public static string GetPrimaryDNS()
        {
            var result = RunApp("nslookup", "", true);
            var m = Regex.Match(result, @"\d+\.\d+\.\d+\.\d+");
            return m.Success ? m.Value : null;
        }

        /// <summary>
        /// 运行一个控制台程序并返回其输出参数。  
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="recordLog"></param>
        /// <returns></returns>
        public static string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }

                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = filename,
                        CreateNoWindow = true,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };
                proc.Start();

                using (var sr = new StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    Thread.Sleep(100); //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行  
                    //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应  
                    if (!proc.HasExited) //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行  
                    {
                        //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行  
                        proc.Kill();
                    }

                    var txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                        Trace.WriteLine(txt);
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取年月（例如：201806）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetYearMonth(DateTime time)
        {
            {
                var year = time.Year;
                var month = time.Month;
                return year * 100 + month;
            }
        }
        /// <summary>
        /// 获取年月日（例如：20180606）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetYearMonthDay(DateTime time)
        {
            {
                var year = time.Year;
                var month = time.Month;
                var day = time.Day;
                return year * 10000 + month * 100 + day;
            }
        }
        /// <summary>
        /// 拆分字符串成List列表
        /// </summary>
        /// <param name="field"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitToList(string field, string symbol = null)
        {
            if (field == null) return new List<string>();
            symbol = string.IsNullOrEmpty(symbol) ? "," : symbol;
            var list = field.Split(new[] { symbol }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return list;
        }

        /// <summary>
        /// 拆分字符串成数组
        /// </summary>
        /// <param name="field"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string[] SplitToArray(string field, string symbol = null)
        {
            if (field == null) return new string[0];
            symbol = string.IsNullOrEmpty(symbol) ? "," : symbol;
            var list = field.Split(new[] { symbol }, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        /// <summary>
        /// 拆分字符串成数组
        /// </summary>
        /// <param name="field"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string[] SplitToArray(string field, string[] symbol = null)
        {
            if (field == null) return new String[0];
            symbol = symbol ?? new[] { "," };
            var list = field.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        /// <summary>
        /// 拆分字符串成string
        /// </summary>
        /// <param name="field"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string SplitToString(List<string> field, string symbol = null)
        {
            if (field == null || !field.Any()) return "";
            symbol = string.IsNullOrEmpty(symbol) ? "," : symbol;
            return string.Join(symbol, field.ToArray());
        }

        /// <summary>
        /// 获取年月（例如：201806）
        /// </summary>
        /// <param name="strtime"></param>
        /// <returns></returns>
        public static int GetYearMonth(string strtime)
        {
            var time = Convert.ToDateTime(strtime);
            return GetYearMonth(time);
        }
        /// <summary>
        /// 反射获取对象属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public static PropertyInfo[] GetProperties<T>(this T model)
        {
            //取得m的Type实例
            var t = typeof(T);
            return t.GetProperties(); //直接根据属性的名字获取其值
        }

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (string.IsNullOrEmpty(sInput))
                return null;
            var sInput1 = sInput.ToLower();
            var output = sInput;
            var pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }


        /// <summary>
        /// 检查危险字符(Field)
        /// </summary>
        /// <returns></returns>
        public static string FilterForField(string sInput)
        {
            if (string.IsNullOrEmpty(sInput))
                return null;
            var sInput1 = sInput.ToLower();
            var output = sInput;
            var pattern = @"exec|insert|delete|update|master|truncate|declare|chr(";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary>
        /// <param name="word"></param>
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            return InText != null && word.Split('|').Any(i => (InText.ToLower().IndexOf(i + " ", StringComparison.Ordinal) > -1) || (InText.ToLower().IndexOf(" " + i, StringComparison.Ordinal) > -1));
        }
        #endregion

        /// <summary>
        /// 根据命名规则格式化指定路径的文件（全）名。
        /// </summary>
        /// <param name="originFileName">指定路径的文件（全）名</param>
        /// <param name="pathFormat"></param>
        /// <returns></returns>
        public static string PathFormat(string originFileName, string pathFormat)
        {
            /*"upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* 上传保存路径,可以自定义保存路径和文件名格式 */
            /* {filename} 会替换成原文件名,配置这项需要注意中文乱码问题 */
            /* {rand:6} 会替换成随机数,后面的数字是随机数的位数 */
            /* {time} 会替换成时间戳 */
            /* {yyyy} 会替换成四位年份 */
            /* {yy} 会替换成两位年份 */
            /* {mm} 会替换成两位月份 */
            /* {dd} 会替换成两位日期 */
            /* {hh} 会替换成两位小时 */
            /* {ii} 会替换成两位分钟 */
            /* {ss} 会替换成两位秒 */
            /* 非法字符 \ : * ? " < > | */
            /* 具体请看线上文档: fex.baidu.com/ueditor/#use-format_upload_filename */

            if (String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{filename}{rand:6}";
            }

            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            originFileName = invalidPattern.Replace(originFileName, "");

            string extension = Path.GetExtension(originFileName);
            string filename = Path.GetFileNameWithoutExtension(originFileName);
            //文件名规则
            pathFormat = pathFormat.Replace("{filename}", filename);
            //随机数规则
            //在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串
            //evaluator,一个自定义方法，它检查每个匹配项，并返回原始匹配字符串或替换字符串。
            pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat, delegate (Match match)
            {
                var digit = 6;
                if (match.Groups.Count > 2)
                {
                    digit = Convert.ToInt32(match.Groups[2].Value);
                }
                var rand = new Random(Guid.NewGuid().GetHashCode());
                return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
            });
            //替换
            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));

            return pathFormat + extension;
        }
        /// <summary>
        /// Get请求，支持添加请求头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerValue">Header头信息键值对</param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> headerValue = null)
        {
            #region 关闭SSL认证
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            #endregion
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Proxy = null;
            request.Method = "GET";
            if (headerValue != null)
            {
                foreach (var item in headerValue)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }
            var response = request.GetResponse() as HttpWebResponse;
            var respStream = response.GetResponseStream();
            string resultJson;
            using (var reader = new StreamReader(respStream, Encoding.UTF8))
            {
                resultJson = reader.ReadToEnd();
                reader.Dispose();
                reader.Close();
            }
            response.Close();
            respStream.Close();
            return resultJson;
        }
        /// <summary>
        /// 文件url路径转换为流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerValue">Header头信息键值对</param>
        /// <returns></returns>
        public static Stream HttpGetStream(string url, Dictionary<string, string> headerValue = null)
        {
            #region 关闭SSL认证
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            #endregion
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Proxy = null;
            if (headerValue != null)
            {
                foreach (var item in headerValue)
                {
                    request.Headers[item.Key] = item.Value;
                }
            }
            var response = request.GetResponse() as HttpWebResponse;
            var respStream = response.GetResponseStream();

            //FileStream fs = File.Create("D:\\test333.docx");

            //long length = response.ContentLength;


            //int i = 0;
            //do
            //{
            //    byte[] buffer = new byte[1024];

            //    i = respStream.Read(buffer, 0, 1024);

            //    fs.Write(buffer, 0, i);

            //} while (i > 0);

            //fs.Close();
            //string resultJson;
            //using (var reader = new StreamReader(respStream, Encoding.UTF8))
            //{
            //    resultJson = reader.ReadToEnd();
            //    reader.Dispose();
            //    reader.Close();
            //}
            //response.Close();
            //respStream.Close();
            return respStream;
        }
        /// <summary>
        /// post请求，支持添加请求头
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="reqParams">参数</param>
        /// <param name="headerValue">请求头</param>
        /// <returns></returns>
        public static string HttpPost(string url, string reqParams, Dictionary<string, string> headerValue = null)
        {
            #region 关闭SSL认证
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            #endregion
            var wReq = WebRequest.Create(url);
            wReq.ContentType = "application/json";
            wReq.Method = "POST";
            if (headerValue != null)
            {
                foreach (var item in headerValue)
                {
                    wReq.Headers[item.Key] = item.Value;
                }
            }
            //wReq.Headers.Add("Token", token);
            //-----传参-----
            var btBodys = Encoding.UTF8.GetBytes(reqParams);
            //wReq.Headers.Add("Authorization", "bearer " + token);
            wReq.ContentLength = btBodys.Length;
            using (var wsr = wReq.GetRequestStream())
            {
                wsr.Write(btBodys, 0, btBodys.Length);
            }
            //-----接收数据-----
            var wResp = wReq.GetResponse();
            var respStream = wResp.GetResponseStream();

            string resultJson;
            using (var reader = new StreamReader(respStream, Encoding.UTF8))
            {
                resultJson = reader.ReadToEnd();
                reader.Dispose();
                reader.Close();
            }
            wResp.Close();
            respStream.Close();

            return resultJson;
        }
        /// <summary>
        /// put请求，支持添加请求头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqParams"></param>
        /// <param name="headerValue"></param>
        /// <returns></returns>
        public static string HttpPut(string url, string reqParams, Dictionary<string, string> headerValue = null)
        {
            #region 关闭SSL认证
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            #endregion
            var wReq = WebRequest.Create(url);
            wReq.ContentType = "application/json";
            wReq.Method = "PUT";
            if (headerValue != null)
            {
                foreach (var item in headerValue)
                {
                    wReq.Headers[item.Key] = item.Value;
                }
            }
            //wReq.Headers.Add("Token", token);
            //-----传参-----
            var btBodys = Encoding.UTF8.GetBytes(reqParams);
            //wReq.Headers.Add("Authorization", "bearer " + token);
            wReq.ContentLength = btBodys.Length;
            using (var wsr = wReq.GetRequestStream())
            {
                wsr.Write(btBodys, 0, btBodys.Length);
            }
            //-----接收数据-----
            var wResp = wReq.GetResponse();
            var respStream = wResp.GetResponseStream();

            string resultJson;
            using (var reader = new StreamReader(respStream, Encoding.UTF8))
            {
                resultJson = reader.ReadToEnd();
                reader.Dispose();
                reader.Close();
            }
            wResp.Close();
            respStream.Close();

            return resultJson;
        }
        /// <summary>
        /// 关闭SSL认证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }
        /// <summary>
        /// json数据压缩
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string Compress(string json)
        {
            StringBuilder sb = new StringBuilder();
            using (StringReader reader = new StringReader(json))
            {
                int ch = -1;
                int lastch = -1;
                bool isQuoteStart = false;
                while ((ch = reader.Read()) > -1)
                {
                    if ((char)lastch != '\\' && (char)ch == '\"')
                    {
                        if (!isQuoteStart)
                        {
                            isQuoteStart = true;
                        }
                        else
                        {
                            isQuoteStart = false;
                        }
                    }
                    if (!Char.IsWhiteSpace((char)ch) || isQuoteStart)
                    {
                        sb.Append((char)ch);
                    }
                    lastch = ch;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Code(string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decode(string str)
        {
            var bytes = Convert.FromBase64String(str);
            return Encoding.Default.GetString(bytes);
        }
        /// <summary>
        /// 读取本地文件
        /// </summary>
        /// <param name="fileName">文件名全称</param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName)

        {

            FileStream pFileStream = null;
            var pReadByte = new byte[0];

            try

            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                var r = new BinaryReader(pFileStream);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }

            catch

            {

                return pReadByte;

            }

            finally

            {
                pFileStream?.Close();

            }

        }
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetPhysicalPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;
            var rootPath = Web.RootPath;
            return string.IsNullOrWhiteSpace(rootPath) ? Path.GetFullPath(relativePath) : $"{Web.RootPath}\\{relativePath.Replace("/", "\\").TrimStart('\\')}";
        }

        /// <summary>
        /// 获取wwwroot路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetWebRootPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;
            var rootPath = Web.WebRootPath;
            return string.IsNullOrWhiteSpace(rootPath) ? Path.GetFullPath(relativePath) : $"{Web.WebRootPath}\\{relativePath.Replace("/", "\\").TrimStart('\\')}";
        }
    }

    /// <summary>
    /// distinct扩展方法，支持Lambda表达式对于对象字段的去重
    /// </summary>
    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
    }

    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private readonly Func<T, V> keySelector;

        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }

    public static class DynamicLinqExpressions
    {

        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }

    }

    /// <summary>
    /// 模型绑定
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ModelAttribute : Attribute
    {
        /// <summary>
        /// 初始化模型绑定
        /// </summary>
        /// <param name="model">模型</param>
        public ModelAttribute(string model = "model")
        {
            Model = model;
        }

        /// <summary>
        /// 模型
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 忽略模型绑定，设置为true则不设置模型绑定
        /// </summary>
        public bool Ignore { get; set; }
    }

}
