using System;
using System.IO;
using Luckyu.Cache;

namespace Luckyu.Utility
{
    public class FileHelper
    {
        #region 获取文件版本号版本号
        private static string RunMode = AppSettingsHelper.GetAppSetting("RunMode");
        private static string cacheKeyContent = CacheFactory.GetCurrentDomain() + "luckyu_filepath_";
        private static string cacheKeyVersion = CacheFactory.GetCurrentDomain() + "luckyu_fileverion_";

        public static string GetFileContent(string filePath)
        {
            string content;
            if (RunMode == "debug")
            {
                content = GetFileContent1(filePath);
            }
            else
            {
                var cache = CacheFactory.Create();
                var filecontent = cache.Read<string>(cacheKeyContent + filePath);
                if (filecontent.IsEmpty())
                {
                    content = GetFileContent1(filePath);
                    cache.Write(cacheKeyContent + filePath, content);
                }
                else
                {
                    content = filecontent;
                }
            }
            return content;
        }

        /// <summary>
        /// 文件自动生成版本号  debug模式下每次读取文件生成版本号 方便调试 release模式下 版本号放入缓存维护
        /// </summary>
        public static string GetFileVersion(string filePath)
        {
            if (RunMode == "debug")
            {
                return GetFileVersion1(filePath);
            }
            else
            {
                var cache = CacheFactory.Create();
                var fileVersion = cache.Read<string>(cacheKeyVersion + filePath);
                if (fileVersion.IsEmpty())
                {
                    string hashvalue = GetFileVersion1(filePath);
                    cache.Write(cacheKeyVersion + filePath, hashvalue);
                    return hashvalue;
                }
                else
                {
                    return fileVersion;
                }
            }
        }

        /// <summary>
        /// 生成文件版本号,根据文件最后编辑时间MD5
        /// </summary>
        private static string GetFileVersion1(string filePath)
        {
            string hashSHA1 = "";
            // 检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(filePath))
            {
                // 获取文件最后编辑时间
                var lastwriteTime = File.GetLastWriteTime(filePath).ToString("yyyyMMddHHmmss");
                hashSHA1 = EncrypHelper.MD5_Encryp(lastwriteTime);
            }
            return hashSHA1;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        private static string GetFileContent1(string filePath)
        {
            string content = "";
            // 检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(filePath))
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        #endregion

        #region 获取文件路径
        public static string MapDirectoryPath(string relativePath)
        {
            // AppContext.BaseDirectory /bin/Debug文件夹路径
            // Environment.CurrentDirectory 代码路径
            // Directory.GetCurrentDirectory()  代码路径
            // 对于发布文件 两个路径相同
            relativePath = relativePath.TrimStart('\\').TrimStart('/');
            var serverPath = Path.Combine(Environment.CurrentDirectory, relativePath);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }
            return serverPath;
        }

        public static string MapPath(string relativePath)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(relativePath);

            relativePath = relativePath.TrimStart('\\').TrimStart('/');
            if (!relativePath.Contains("Views"))
            {
                relativePath = "wwwroot\\" + relativePath;
            }
            var serverPath = Path.Combine(Environment.CurrentDirectory, relativePath);
            if (relativePath.Contains("Areas"))  // 还没找到 内嵌资源不编译的方法 以后再说 先搞个方便调试的
            {
                if (LuckyuHelper.IsDebug())  // 放弃内嵌资源的做法 使用复制js css的做法 单挑时还是不方便, 毕竟只有生成时才复制,方便调试还是先读文件
                {
                    DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                    var dirName = relativePath.Replace("Areas/", "");
                    dirName = dirName.Substring(0, dirName.IndexOf('/'));
                    dirName = $"Luckyu.Module.{dirName}";
                    var dir1 = dir.Parent.GetDirectories(dirName, SearchOption.TopDirectoryOnly);
                    if (dir1 != null && dir1.Length > 0)
                    {
                        serverPath = Path.Combine(dir1[0].FullName, relativePath);
                        return serverPath;
                    }
                }
            }
            logger.Info(serverPath);
            return serverPath;
        }
        #endregion

        #region 合并文件夹
        public static string Combine(string path1, string path2)
        {
            path2 = path2.Trim('/').Trim('\\');
            return Path.Combine(path1, path2);
        }
        #endregion

    }
}
