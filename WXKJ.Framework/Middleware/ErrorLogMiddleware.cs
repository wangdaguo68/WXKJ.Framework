using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WXKJ.Framework.Exceptions;
using System;
using System.Threading.Tasks;

namespace WXKJ.Framework.Middleware
{
    public class ErrorLogMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化错误日志中间件
        /// </summary>
        /// <param name="next">方法</param>
        public ErrorLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="loggerFactory"></param>
        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    WriteLog(context, ex, loggerFactory);
                }
                throw;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void WriteLog(HttpContext context, Exception ex, ILoggerFactory loggerFactory)
        {
            if (context == null)
                return;
            loggerFactory.CreateLogger<ErrorLogMiddleware>().LogError("全局异常捕获" + $"，状态码：{context.Response.StatusCode}" + ex);
        }
    }
}
