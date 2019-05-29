using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WXKJ.Infrastructure;

namespace WXKJ.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var uri = AppSettingConfig.AppSettings.Server_Uri;
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            return WebHost.CreateDefaultBuilder(args).UseConfiguration(config).UseUrls(uri).UseKestrel().UseIISIntegration().UseStartup<Startup>().Build();
        }
    }
}
