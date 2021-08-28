using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET;
using Luckyu.App.OA;
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
    /// 新闻  /MobileModule/MNews
    /// </summary>
    [Area("MobileModule")]
    public class MNewsController: MvcControllerBase
    {
        #region Var
        private NewsBLL newsBLL = new NewsBLL();

        #endregion

        #region Index
        public IActionResult ShowIndex()
        {
            return View();
        }

        public IActionResult ShowForm()
        {
            return View();
        }
        #endregion
    }
}
