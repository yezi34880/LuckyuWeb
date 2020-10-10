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
        public IActionResult GetFormData(string taskId)
        {
            var task = taskBLL.GetTaskEnttity(r => r.task_id == taskId);
            if (task == null)
            {
                return Fail("该数据不存在");
            }
            var instance = taskBLL.GetInstanceEnttity(r => r.instance_id == task.instance_id);
            if (instance == null)
            {
                return Fail("该数据不存在");
            }
            var scheme = instance.schemejson.ToObject<WFSchemeModel>();
            var node = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();

            var historys = taskBLL.GetHistoryTaskList(task.process_id, task.instance_id);
            var dic = new Dictionary<string, object>
            {
                {"Task",task },
                {"Node",node },
                {"Scheme",  instance.schemejson },
                { "History",historys}
            };
            return Success(dic);
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

        #region Interface
        [AjaxOnly, HttpPost]
        public IActionResult Create(string flowCode, string processId, string processName)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Create(flowCode, processId, processName, loginInfo);
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

        [AjaxOnly, HttpPost]
        public IActionResult Finish(string instanceId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Finish(instanceId, loginInfo);
            return Json(res);
        }
        #endregion

    }
}
