using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Luckyu.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Luckyu.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var urls = AppSettingsHelper.GetAppSetting("ServerUrls");
            Host.CreateDefaultBuilder(args) .ConfigureWebHostDefaults(webBuilder =>
            {
                var host = webBuilder.UseStartup<Startup>();
                if (!string.IsNullOrEmpty(urls))
                {
                    host = host.UseUrls(urls);
                }
            }).Build().Run();
        }
    }
}

