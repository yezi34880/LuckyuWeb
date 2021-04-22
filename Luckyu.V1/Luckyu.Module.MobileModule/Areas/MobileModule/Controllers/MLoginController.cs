using System;
using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.Cache;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.MobileModule.Controllers
{
    /// <summary>
    /// 登录  /MobileModule/MLogin
    /// </summary>
    [AllowAnonymous]
    [Area("MobileModule")]
    public class MLoginController : MvcControllerBase
    {
        #region Var
        private UserBLL userBLL = new UserBLL();

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerifyCode()
        {
            var vcode = VerifyCodeHelper.GetCode();
            var bytes = VerifyCodeHelper.GetVerifyCode(vcode);
            HttpContext.Session.SetString("m_session_verifycode", EncrypHelper.MD5_Encryp(vcode.ToLower()));
            return File(bytes, @"image/Gif");
        }

        [HttpPost, AjaxOnly]
        public IActionResult CheckLogin(string username, string password, string verifycode)
        {
            var vcode = HttpContext.Session.GetString("m_session_verifycode");
            if (vcode.IsEmpty() || vcode != EncrypHelper.MD5_Encryp(verifycode.ToLower()))
            {
                return Fail("验证码错误或过期，请刷新");
            }
            var res = userBLL.CheckLogin(username, password);
            var log = new sys_logEntity();
            var request = HttpContext.Request;

            var info = DeviceDetector.GetInfoFromUserAgent(request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                log.device = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ") + (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }
            log.app_name = LuckyuHelper.AppID;
            log.host = request.Host.Host;
            log.ip_address = HttpContext.GetRequestIp();
            log.log_type = (int)LogType.Login;
            log.log_time = DateTime.Now;
            log.module = "MobileLogin";
            log.log_content = $"用户 { username } 登录";
            if (res.code == 200)
            {
                LoginUserInfo.Instance.SetLogin(res.data.loginname, HttpContext, LuckyuHelper.AppID);
                log.log_content += $"  登录成功 {res.data.realname}-{res.data.loginname}";
                log.op_type = "成功";
                log.user_id = res.data.user_id;
                log.user_name = res.data.realname;
                LogBLL.WriteLog(log);
                return Success("/MobileModule/MHome/Index");
            }
            else
            {
                log.log_content += "  登录失败 " + res.info;
                log.op_type = "失败";
                LogBLL.WriteLog(log);
                return Fail(res.info);
            }
        }

    }
}
