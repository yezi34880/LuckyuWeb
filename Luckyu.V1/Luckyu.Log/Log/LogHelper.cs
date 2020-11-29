using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using NLog.Config;

namespace Luckyu.Log
{
    public class LogHelper
    {
        static LogHelper()
        {
            var configPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/nlog.config";
            if (!System.IO.File.Exists(configPath))
            {
                throw new Exception("没有nlog.config文件");
            }
            LogManager.Configuration = new XmlLoggingConfiguration(configPath);
        }

        public static string ErrorFormat(Exception ex, sys_logEntity log)
        {
            var strMsg = new StringBuilder();
            strMsg.Append($"1 信息>>用户 {log.user_name}({log.user_id})  IP  {log.ip_address}  Host  {log.host}  时间  {log.log_time}  设备  {log.device} \r\n");
            strMsg.Append($"2 来源>> {log.module} \r\n");
            strMsg.Append($"3 异常>> {ex.Message} \r\n");
            strMsg.Append($"4 实例>> {ex.StackTrace} \r\n");
            if (ex.InnerException != null)
            {
                strMsg.Append($"5 内部异常>>{ex.InnerException.Message}\r\n {ex.InnerException.StackTrace} \r\n");
            }
            var msg = strMsg.ToString();
            return msg;
        }


        public static string InfoFormat(string info, sys_logEntity log)
        {
            var strMsg = new StringBuilder();
            strMsg.Append($"1 信息>>用户 {log.user_name}({log.user_id})  IP  {log.ip_address}  Host  {log.host}  时间  {log.log_time}  设备  {log.device} \r\n");
            strMsg.Append($"2 来源>> {log.module} \r\n");
            strMsg.Append($"3 内容>> {info} \r\n");
            var msg = strMsg.ToString();
            return msg;
        }

    }
}
