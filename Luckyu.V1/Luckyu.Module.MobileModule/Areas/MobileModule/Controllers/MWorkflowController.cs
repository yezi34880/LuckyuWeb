using System;
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
            var historys = taskBLL.GetHistoryTaskList(processId, instanceId);
            ViewBag.Historys = historys;

            var formType = "instance";
            if (taskId.IsEmpty() && historyId.IsEmpty())
            {
                formType = "instance";
            }
            else if (!taskId.IsEmpty() && historyId.IsEmpty())
            {
                formType = "task";
            }
            else if (taskId.IsEmpty() && !historyId.IsEmpty())
            {
                formType = "history";
            }
            ViewBag.FormType = formType;

            var instance = taskBLL.GetInstanceEnttity(r => r.instance_id == instanceId);
            ViewBag.Instance = instance;
            var scheme = instance.schemejson.ToObject<WFSchemeModel>();
            var node = new WFSchemeNodeModel();
            if (formType == "instance")
            {
                node = scheme.nodes.Where(r => r.type == "startround").FirstOrDefault();
            }
            else if (formType == "task")
            {
                var task = taskBLL.GetTaskEnttity(r => r.task_id == taskId);
                node = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
            }
            else if (formType == "history")
            {
                var history = taskBLL.GetHistoryEnttity(r => r.task_id == taskId);
                node = scheme.nodes.Where(r => r.id == history.node_id).FirstOrDefault();
            }
            ViewBag.Forms = node != null ? node.forms : null;

            return View();

        }

        #endregion
    }
}
