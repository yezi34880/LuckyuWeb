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
            var res = userBLL.CheckLogin(username, password, "MobileLogin", HttpContext);
            if (res.code == 200)
            {
                return Success("/MobileModule/MHome/Index");
            }
            else
            {
                return Fail(res.info);
            }
        }

    }
}
