using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WXKJ.Framework.Common;
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace WXKJ.Framework.Middleware
{
    /// <summary>
    /// 全局异常中间件
    /// </summary>
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            //记录日志
            var logger = LogManager.GetCurrentClassLogger();
            logger.Log(LogLevel.Error, exception.Message);
            //返回友好的提示
            var response = context.Response;

            //状态码
            if (exception is UnauthorizedAccessException)
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else
                response.StatusCode = (int)HttpStatusCode.BadRequest;

            response.ContentType = context.Request.Headers["Accept"];
            var result = new Result(StateCode.Fail,exception.Message);

            if (response.ContentType.ToLower() == "application/xml")
            {
                await response.WriteAsync(Object2XmlString(result)).ConfigureAwait(false);
            }
            response.ContentType = "application/json";
            await response.WriteAsync(JsonConvert.SerializeObject(result)).ConfigureAwait(false);
        }

        /// <summary>
        /// 对象转为Xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string Object2XmlString(object o)
        {
            var sw = new StringWriter();
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(sw, o);
            }
            catch
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Dispose();
            }
            return sw.ToString();
        }
    }
}
