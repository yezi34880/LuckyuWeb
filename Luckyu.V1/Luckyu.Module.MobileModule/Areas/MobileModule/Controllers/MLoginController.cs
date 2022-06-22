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

        [HttpPost, AjaxOnly]
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
                if (vcode.IsEmpty() || vcode.ToLower() != verifycode.ToLower())
                {
                    return Fail("验证码错误或过期，请刷新");
                }
            }
            var res = userBLL.CheckLogin(username, password, "MobileLogin", HttpContext);
            if (res.code == 200)
            {
                HttpContext.Session.Remove("session_wrongnum");
                return Success("/MobileModule/MHome/Index");
            }
            else
            {
                var wrongnum1 = HttpContext.Session.GetInt32("session_wrongnum");
                var wrongnum2 = wrongnum1.HasValue ? wrongnum1.Value + 1 : 1;
                HttpContext.Session.SetInt32("session_wrongnum", wrongnum2);

                return Fail(res.info, new { wrongnum = wrongnum2 });
            }

        }

    }
}
