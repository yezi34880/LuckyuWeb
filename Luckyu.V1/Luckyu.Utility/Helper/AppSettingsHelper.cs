using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Utility
{
    public class AppSettingsHelper
    {
        public static string GetConnectionString(string key)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            return configuration.GetConnectionString(key);
        }

        public static string GetAppSetting(string key)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var appSettings = configuration.GetSection("AppSettings");
            return appSettings[key];
        }

        public static IConfigurationRoot AppsettingRoot
        {
            get
            {
                var builder = new ConfigurationBuilder()
                     .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                return builder.Build();
            }
        }


    }
}
