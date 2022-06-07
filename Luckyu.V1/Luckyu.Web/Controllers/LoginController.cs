using System;
using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.Cache;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : MvcControllerBase
    {
        #region Var
        private UserBLL userBLL = new UserBLL();

        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        [HttpGet]
        public IActionResult VerifyCode()
        {
            var vcode = VerifyCodeHelper.GetCode();
            var bytes = VerifyCodeHelper.GetVerifyCode(vcode);
            HttpContext.Session.SetString("session_verifycode", EncrypHelper.MD5_Encryp(vcode.ToLower()));
            return File(bytes, @"image/Gif");
        }

        [HttpPost, AjaxOnly]
        //[HandlerValidateAntiForgeryToken]
        public IActionResult CheckLogin(string username, string password, string verifycode)
        {
            var wrongnum = HttpContext.Session.GetInt32("session_wrongnum");
            if (!wrongnum.IsEmpty() && wrongnum > 2)
            {
                if (verifycode.IsEmpty())
                {
                    return Fail("请输入验证码", new { wrongnum });
                }
                var vcode = HttpContext.Session.GetString("session_verifycode");
                if (vcode.IsEmpty() || vcode != EncrypHelper.MD5_Encryp(verifycode.ToLower()))
                {
                    return Fail("验证码错误或过期，请刷新");
                }
            }
            var res = userBLL.CheckLogin(username, password, "Login", HttpContext);
            if (res.code == 200)
            {
                return Success("/Home/Index");
            }
            else
            {
                var wrongnum1 = HttpContext.Session.GetInt32("session_wrongnum");
                var wrongnum2 = wrongnum1.HasValue ? wrongnum1.Value + 1 : 1;
                HttpContext.Session.SetInt32("session_wrongnum", wrongnum2);

                return Fail(res.info, new { wrongnum = wrongnum2 });
            }
        }
        #endregion

        #region OutLogin
        public IActionResult OutLogin()
        {
            LoginUserInfo.Instance.Logout(HttpContext);
            return Success();
        }
        #endregion
    }
}
