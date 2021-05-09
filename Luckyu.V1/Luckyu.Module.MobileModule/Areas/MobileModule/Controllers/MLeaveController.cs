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
    /// 请假  /MobileModule/MLeave
    /// </summary>
    [Area("MobileModule")]
    public class MLeaveController : MvcControllerBase
    {
        public IActionResult Form()
        {
            return View();
        }

    }
}
