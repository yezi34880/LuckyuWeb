using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.WorkflowModule.Controllers
{
    /// <summary>
    /// 任务  /WorkflowModule/Task
    /// </summary>
    [Area("WorkflowModule")]
    public class TaskController : MvcControllerBase
    {
        #region Var
        private WFTaskBLL taskBLL = new WFTaskBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqPage, int tasktype)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = taskBLL.Page(jqPage, tasktype, loginInfo);
            return Json(page);
        }
        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetFormData(string instanceId, string taskId, string historyId)
        {
            var res = taskBLL.GetFormData(instanceId, taskId, historyId);
            return Json(res);
        }

        [HttpGet]
        public IActionResult GetTaskLog(string processId, string instanceId)
        {
            var historys = taskBLL.GetHistoryTaskList(processId, instanceId);
            return Json(historys);
        }
        #endregion

        #region 对话框
        public IActionResult ApproveForm()
        {
            return View();
        }
        #endregion

        #region LogFormByProcessId 单据查看自身审批日志

        public IActionResult LogFormByProcessId(string processId)
        {
            var instances = taskBLL.GetInstanceList(r => r.process_id == processId);
            return View(instances);
        }

        #endregion

        #region Interface
        [AjaxOnly, HttpPost]
        public IActionResult Create(string flowCode, string processId, string processName, string processContent)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Create(flowCode, processId, processName, processContent, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult Approve(string taskId, int result, string opinion)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Approve(taskId, result, opinion, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult AddUser(string taskId, string userId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.AddUser(taskId, userId, loginInfo);
            return Json(res);
        }
        #endregion

    }
}
