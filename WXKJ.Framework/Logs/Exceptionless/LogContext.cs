using System;
using WXKJ.Framework.Helpers;
using WXKJ.Framework.Logs.Internal;

namespace WXKJ.Framework.Logs.Exceptionless {
    /// <summary>
    /// Exceptionless日志上下文
    /// </summary>
    public class LogContext : WXKJ.Framework.Logs.Core.LogContext {
        /// <summary>
        /// 创建日志上下文信息
        /// </summary>
        protected override LogContextInfo CreateInfo() {
            return new LogContextInfo {
                TraceId = Guid.NewGuid().ToString(),
                Stopwatch = GetStopwatch(),
                Url = Web.Url
            };
        }
    }
}
