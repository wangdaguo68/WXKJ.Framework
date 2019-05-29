using System;

namespace WXKJ.Framework.Extensions
{
    /// <summary>
    /// http扩展
    /// </summary>
    public static class HttpContextExtension
    {
        public static IServiceProvider ServiceProvider;
        /// <summary>
        /// 获取当前http上下文
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                var factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                var context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

    }
}
