using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Cache;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Luckyu.Module.MobileModule.Controllers
{
    /// <summary>
    /// 流程  /MobileModule/MWorkflow
    /// </summary>
    [Area("MobileModule")]
    public class MWorkflowController : MvcControllerBase
    {
        #region Var
        private WFTaskBLL taskBLL = new WFTaskBLL();
        private UserBLL userBLL = new UserBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Form
        public IActionResult Form(string taskId, string instanceId, string processId, string historyId)
        {
            return View();
        }

        #endregion
    }
}
