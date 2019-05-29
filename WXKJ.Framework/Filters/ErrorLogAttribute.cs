using Microsoft.AspNetCore.Mvc.Filters;
using WXKJ.Framework.Logs;
using WXKJ.Framework.Logs.Extensions;

namespace WXKJ.Framework.Webs.Filters {
    /// <summary>
    /// 错误日志过滤器
    /// </summary>
    public class ErrorLogAttribute : ExceptionFilterAttribute {
        /// <summary>
        /// 异常处理
        /// </summary>
        public override void OnException( ExceptionContext context ) {
            if( context == null )
                return;
            var log = Log.GetLog( context ).Caption( "WebApi全局异常" );
            context.Exception.Log( log );
        }
    }
}