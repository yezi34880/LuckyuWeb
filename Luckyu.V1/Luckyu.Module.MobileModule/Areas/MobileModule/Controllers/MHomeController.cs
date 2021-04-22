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
    /// 首页  /MobileModule/MHome
    /// </summary>
    [Area("MobileModule")]
    public class MHomeController : MvcControllerBase
    {
        #region Index
        public IActionResult Index()
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            ViewBag.UserInfo = loginInfo;
            return View();
        }
        #endregion

        #region EditPwd
        public IActionResult EditPwdForm()
        {
            return View();
        }
        #endregion
    }
}
