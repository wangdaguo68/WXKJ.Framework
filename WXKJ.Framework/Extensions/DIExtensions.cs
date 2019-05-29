using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class DIExtensions
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            return services;
        }
        /// <summary>
        /// 服务注入，用法Startup.cs的Configure方法中app.UserDI()
        /// using (var scope = DI.ServiceProvider.CreateScope())
        /// {
        ///     var service = scope.ServiceProvider.GetRequiredService<XXX>();
        /// }
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UserDI(this IApplicationBuilder builder)
        {
            DI.ServiceProvider = builder.ApplicationServices;
            return builder;
        }
    }

    public static class DI
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
