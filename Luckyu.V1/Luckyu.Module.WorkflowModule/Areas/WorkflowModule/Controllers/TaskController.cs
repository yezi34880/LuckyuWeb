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

        #region 对话框
        public IActionResult ApproveForm()
        {
            return View();
        }
        public IActionResult ReadForm()
        {
            return View();
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
        public async Task<IActionResult> Approve(string taskId, int result, string opinion)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = await taskBLL.Approve(taskId, result, opinion, loginInfo, _hubContext);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult AddUser(string taskId, List<string> userIds)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.AddUser(taskId, userIds, loginInfo);
            return Json(res);
        }
        #endregion

        #region Home 展示
        public IActionResult HomeShow()
        {
            var jqPage = new JqgridPageRequest
            {
                page = 1,
                rows = 5,
                sidx = "createtime",
                sord = "DESC"
            };
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = taskBLL.Page(jqPage, 1, loginInfo);
            var dic = new Dictionary<string, object>();
            dic.Add("Task", page.rows);
            return Success(dic);
        }
        #endregion

    }
}
