using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WXKJ.Framework
{
    public sealed class AppSettingConfig
    {
        private static AppSettings _instance;
        private static readonly object _lock = new object();

        public static AppSettings AppSettings
        {
            get
            {
                if (null != _instance) return _instance;
                lock (_lock)
                {
                    if (null != _instance) return _instance;
                    var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true)
                        .Build();
                    var sp = new ServiceCollection().AddOptions()
                        .Configure<AppSettings>(config.GetSection("AppSettings"))
                        .BuildServiceProvider();
                    var hosting = sp.GetService<IOptions<AppSettings>>();
                    _instance = hosting.Value;
                }
                return _instance;
            }
        }

    }
    public class AppSettings
    {
        /// <summary>
        /// API服务地址
        /// </summary>
        public string Server_Uri { set; get; }
    }
}
