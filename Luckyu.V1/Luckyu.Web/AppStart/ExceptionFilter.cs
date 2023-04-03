using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit;
using MimeKit.Text;
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
                log.user_name = $"{loginInfo.loginname}-{ loginInfo.realname}";
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
                context.Result = new ContentResult() { Content = context.Exception.Message };
            }

        }

        private async void SendEmail(string message)
        {
            try
            {
                var AppID = AppSettingsHelper.GetAppSetting("AppID");
                var host = AppSettingsHelper.GetAppSetting("EmailSenderHost");
                var port = AppSettingsHelper.GetAppSetting("EmailSenderPort").ToInt();
                var senderid = AppSettingsHelper.GetAppSetting("EmailSenderAddress");
                var senderpwd = AppSettingsHelper.GetAppSetting("EmailSenderPassword");
                var sendername = AppSettingsHelper.GetAppSetting("EmailSenderName");
                var toname = AppSettingsHelper.GetAppSetting("EmailToName");
                var toaddress = AppSettingsHelper.GetAppSetting("EmailToAddress");

                if (senderid.IsEmpty() || toaddress.IsEmpty())
                {
                    return;
                }

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.MessageSent += (sender, args) => { };
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await smtp.ConnectAsync(host, port, SecureSocketOptions.Auto);
                    await smtp.AuthenticateAsync(senderid, senderpwd);

                    var messageToSend = new MimeMessage
                    {
                        Sender = new MailboxAddress(sendername, senderid),
                        Subject = AppID + " Error",
                    };
                    messageToSend.From.Add(new MailboxAddress(sendername, senderid));
                    messageToSend.Body = new TextPart(TextFormat.Plain) { Text = message };
                    messageToSend.To.Add(new MailboxAddress(toname, toaddress));
                    await smtp.SendAsync(messageToSend);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

            }

        }

    }
}
