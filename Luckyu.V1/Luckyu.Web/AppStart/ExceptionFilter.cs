using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Luckyu.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var request = context.HttpContext.Request;
            var log = new sys_logEntity();
            var info = DeviceDetector.GetInfoFromUserAgent(request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                log.device = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ") + (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }
            log.app_name = LuckyuHelper.AppID;
            log.host = request.Host.Host;
            log.ip_address = context.HttpContext.GetRequestIp();
            log.log_type = (int)LogType.Exception;
            log.op_type = "异常";
            log.log_time = DateTime.Now;
            log.module = context.ActionDescriptor.DisplayName;
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(context.HttpContext);
            if (loginInfo != null)
            {
                log.user_id = loginInfo.user_id;
                log.user_name = loginInfo.realname;
            }
            log.log_content = LogHelper.ErrorFormat(context.Exception, log);

            logger.Error(log.log_content);
            LogBLL.WriteLog(log);
            SendEmail(log.log_content);

            if (request.IsAjaxRequest())
            {
                var json = new ResponseResult
                {
                    code = 500,
                    info = "服务器内部错误。" + context.Exception.Message,
                };
                context.Result = new JsonResult(json);
            }
            else
            {
                context.Result = new ContentResult() { Content  = context.Exception.Message };
            }

        }

        private void SendEmail(string msg)
        {

        }

    }
}
