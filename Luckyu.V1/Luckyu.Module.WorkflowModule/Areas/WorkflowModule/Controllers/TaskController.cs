using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private UserBLL userBLL = new UserBLL();

        private readonly IHubContext<MessageHub> _hubContext;
        public TaskController(IHubContext<MessageHub> messageHubContext)
        {
            _hubContext = messageHubContext;
        }

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

        #region 调整流程位置
        public IActionResult ModifyForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetTaskScheme(string instanceId)
        {
            var res = taskBLL.GetTaskScheme(instanceId);
            return Json(res);
        }

        public IActionResult GetHelpMePage(JqgridPageRequest jqPage, string instanceId)
        {
            var res = taskBLL.GetHelpMePage(jqPage, instanceId);
            return Json(res);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public IActionResult DeleteTask(string taskId)
        {
            var res = taskBLL.DeleteTask(taskId);
            return Json(res);
        }

        public IActionResult GetAddUserPage(JqgridPageRequest jqPage, string instanceId)
        {
            var res = taskBLL.GetAddUserPage(jqPage, instanceId);
            return Json(res);
        }
        /// <summary>
        /// 删除任务下用户
        /// </summary>
        public IActionResult DeleteTaskAuth(string authId)
        {
            var res = taskBLL.DeleteTaskAuth(authId);
            return Json(res);
        }

        /// <summary>
        /// 删除审批历史
        /// </summary>
        /// <param name="authId"></param>
        /// <returns></returns>
        public IActionResult DeleteHistory(string historyId)
        {
            var res = taskBLL.DeleteHistory(historyId);
            return Json(res);
        }
        #endregion

        #region 通用单据查看自身审批日志页面（包括多次流程实例信息）

        public IActionResult LogFormByProcessId(string processId)
        {
            var instances = taskBLL.GetInstanceList(r => r.process_id == processId);
            return View(instances);
        }

        #endregion

        #region Interface
        [AjaxOnly, HttpPost]
        public async Task<IActionResult> Create(string flowCode, string processId, string processName, string processContent, string submitUserId, string departmentId, string companyId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            if (!submitUserId.IsEmpty())
            {
                var user = userBLL.GetEntityByCache(r => r.user_id == submitUserId);
                loginInfo = new UserModel();
                loginInfo = user.Adapt<UserModel>();
            }
            if (!departmentId.IsEmpty())
            {
                loginInfo.department_id = departmentId;
            }
            if (!companyId.IsEmpty())
            {
                loginInfo.company_id = companyId;
            }
            var res = await taskBLL.Create(flowCode, processId, processName, processContent, loginInfo, _hubContext);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public async Task<IActionResult> Approve(string taskId, int result, string opinion, int returnType)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = await taskBLL.Approve(taskId, result, opinion, returnType, loginInfo, _hubContext);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult AddUser(string taskId, List<string> userIds, string remark)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.AddUser(taskId, userIds, remark, loginInfo);
            return Json(res);
        }
        [AjaxOnly, HttpPost]
        public IActionResult HelpMe(string taskId, List<string> userIds, string remark)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.HelpMe(taskId, userIds, remark, loginInfo);
            return Json(res);
        }
        #endregion

    }
}
