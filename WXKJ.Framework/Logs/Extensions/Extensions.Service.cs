using System;
using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WXKJ.Framework.Logs.Abstractions;
using WXKJ.Framework.Logs.Core;
using WXKJ.Framework.Logs.Formats;
using WXKJ.Framework.Logs.NLog;

namespace WXKJ.Framework.Logs.Extensions {
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 注册NLog日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddNLog( this IServiceCollection services ) {
            services.TryAddScoped<ILogProviderFactory, LogProviderFactory>();
            services.TryAddSingleton<ILogFormat, ContentFormat>();
            services.TryAddScoped<ILogContext, LogContext>();
            services.TryAddScoped<ILog, Log>();
        }

        /// <summary>
        /// 注册Exceptionless日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configAction">配置操作</param>
        public static void AddExceptionless( this IServiceCollection services, Action<ExceptionlessConfiguration> configAction ) {
            services.TryAddScoped<ILogProviderFactory, Exceptionless.LogProviderFactory>();
            services.TryAddSingleton( typeof( ILogFormat ), t => NullLogFormat.Instance );
            services.TryAddScoped<ILogContext, Exceptionless.LogContext>();
            services.TryAddScoped<ILog, Log>();
            configAction?.Invoke( ExceptionlessClient.Default.Configuration );
        }
    }
}
