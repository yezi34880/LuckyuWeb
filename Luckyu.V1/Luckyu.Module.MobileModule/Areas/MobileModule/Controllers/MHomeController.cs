using System;
using System.Collections.Generic;
using System.Linq;
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
        #region Var
        private ModuleBLL moduleBLL = new ModuleBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            ViewBag.UserInfo = loginInfo;

            List<sys_moduleEntity> listSelfModule;
            var modules = moduleBLL.GetModuleTreeByUser(loginInfo, 1, out listSelfModule);
            loginInfo.module_ids = listSelfModule.Select(r => r.module_id).ToList();

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
