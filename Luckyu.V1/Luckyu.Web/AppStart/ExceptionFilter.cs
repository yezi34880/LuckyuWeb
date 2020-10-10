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
            var entity = new sys_logEntity();
            var info = DeviceDetector.GetInfoFromUserAgent(request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                entity.device = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ") + (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }
            entity.host = request.Host.Host;
            entity.ip_address = context.HttpContext.GetRequestIp();
            entity.log_type = (int)LogType.Exception;
            entity.op_type = "异常";
            entity.log_time = DateTime.Now;
            entity.module = context.ActionDescriptor.DisplayName;
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(context.HttpContext);
            if (loginInfo != null)
            {
                entity.user_id = loginInfo.user_id;
                entity.user_name = loginInfo.realname;
            }
            entity.log_content = LogHelper.ErrorFormat(context.Exception, entity);

            logger.Error(entity.log_content);
            LogBLL.WriteLog(entity);
            SendEmail(entity.log_content);

            if (request.IsAjaxRequest())
            {
                var json = new ResponseResult
                {
                    code = 500,
                    info = "服务器内部错误。" + context.Exception.Message,
                };
                context.Result = new JsonResult(json);
            }

        }

        private void SendEmail(string msg)
        {

        }

    }
}
