using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfig(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("文件不存在");
            }
            var file = new FileInfo(path);
            var builder = new ConfigurationBuilder()
                 .SetBasePath(file.DirectoryName)
                 .AddJsonFile(file.Name, optional: false, reloadOnChange: true);
            return builder.Build();
        }

        public static string GetValue(string path, string key)
        {
            if (!File.Exists(path))
            {
                throw new Exception("文件不存在");
            }
            var file = new FileInfo(path);
            var builder = new ConfigurationBuilder()
                 .SetBasePath(file.DirectoryName)
                 .AddJsonFile(file.Name, optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var appSettings = configuration.GetSection(key);
            return appSettings[key];
        }
    }
}
