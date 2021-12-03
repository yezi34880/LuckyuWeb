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
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.GetFormData(instanceId, taskId, historyId, loginInfo);
            return Json(res);
        }

        [HttpGet]
        public IActionResult GetTaskLog(string processId, string instanceId)
        {
            var historys = taskBLL.GetHistoryTaskList(processId, instanceId);
            return Json(historys);
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
        public async Task<IActionResult> Approve(string taskId, int result, string opinion, int returnType, string authors)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            Dictionary<string, List<KeyValue>> dicAuthor = authors.ToObject<Dictionary<string, List<KeyValue>>>();
            var res = await taskBLL.Approve(taskId, result, opinion, returnType, dicAuthor, loginInfo, _hubContext);
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
